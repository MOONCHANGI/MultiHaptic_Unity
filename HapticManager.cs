using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Haptics
{
    public class HapticManager : MonoBehaviour
    {
        public static HapticManager Instance { get; private set; }

        [Header("Wi-Fi Settings")]
        public string espIpAddress = "192.168.4.1";

        // 이번 프레임에 적용될 장비들의 값 (Key: 장비이름, Value: 세기)
        private Dictionary<string, int> _currentFrameValues = new Dictionary<string, int>();

        // 마지막으로 ESP에 전송 성공한 값 (중복 전송 방지용)
        private Dictionary<string, int> _lastSentValues = new Dictionary<string, int>();

        // 관리할 장비 목록 (여기에 등록된 것만 0으로 초기화됨)
        private readonly string[] _knownDevices = new string[]
        {
            "pumpL", "pumpR", "valveL", "valveR", "vibL", "vibR", "pelt"
        };

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeDictionaries();
            }
            else
            {
                Destroy(gameObject);
            }

            if (!string.IsNullOrEmpty(espIpAddress) && !espIpAddress.StartsWith("http://"))
            {
                espIpAddress = "http://" + espIpAddress;
            }
        }

        private void InitializeDictionaries()
        {
            foreach (var dev in _knownDevices)
            {
                _currentFrameValues[dev] = 0;
                _lastSentValues[dev] = 0;
            }
        }

        // 1. 매 프레임 시작 시: 모든 장비 값을 0으로 리셋 (Timeline이 아무 말 없으면 꺼진 것으로 간주)
        private void Update()
        {
            foreach (var key in _knownDevices)
            {
                _currentFrameValues[key] = 0;
            }
        }

        // 2. 타임라인 클립들이 이 함수를 호출해서 "나 켜져 있어!"라고 보고함
        public void SetFrameValue(string deviceName, int value)
        {
            if (_currentFrameValues.ContainsKey(deviceName))
            {
                // 같은 장비에 여러 클립이 겹치면 더 큰 값을 우선시 (선택사항)
                if (value > _currentFrameValues[deviceName])
                {
                    _currentFrameValues[deviceName] = value;
                }
            }
            else
            {
                _currentFrameValues[deviceName] = value;
            }
        }

        // 3. 모든 업데이트가 끝난 후(LateUpdate), 변경사항을 모아서 전송
        private void LateUpdate()
        {
            StringBuilder queryBuilder = new StringBuilder();
            bool hasChanges = false;

            foreach (var kvp in _currentFrameValues)
            {
                string device = kvp.Key;
                int newValue = kvp.Value;

                // 이전에 보낸 값과 다를 때만 전송 목록에 추가
                if (!_lastSentValues.ContainsKey(device) || _lastSentValues[device] != newValue)
                {
                    if (hasChanges) queryBuilder.Append("&"); // 두 번째부터는 & 붙임

                    queryBuilder.Append($"{device}={newValue}");

                    // 보냈다고 가정하고 상태 업데이트
                    _lastSentValues[device] = newValue;
                    hasChanges = true;
                }
            }

            // 변경된 게 하나라도 있으면 전송!
            if (hasChanges)
            {
                string finalQuery = queryBuilder.ToString();
                StartCoroutine(SendRequestRoutine(finalQuery));
            }
        }

        private IEnumerator SendRequestRoutine(string query)
        {
            // query 예시: "pumpL=0&pumpR=180"
            string url = $"{espIpAddress}/servo?{query}";

            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                // 아주 빠르게 보내고 잊어버림 (Fire and Forget)
                yield return request.SendWebRequest();
            }
        }

        // 외부(버튼 등)에서 강제로 보낼 때 쓰는 함수
        public void SendCommandDirect(string command)
        {
            StartCoroutine(SendRequestRoutine(command));
        }
    }
}