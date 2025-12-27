using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

// 🔴 아주 심플하고 명확합니다.
namespace Haptics
{
    public class HapticManager : MonoBehaviour
    {
        public static HapticManager Instance { get; private set; }

        [Header("Wi-Fi Settings")]
        public string espIpAddress = "192.168.4.1";

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            // IP 주소 포맷 자동 보정
            if (!string.IsNullOrEmpty(espIpAddress) && !espIpAddress.StartsWith("http://"))
            {
                espIpAddress = "http://" + espIpAddress;
            }
        }

        // 외부에서 호출하는 공용 함수
        public void SendCommand(string command)
        {
            if (string.IsNullOrEmpty(espIpAddress) || string.IsNullOrEmpty(command)) return;
            StartCoroutine(SendRequestRoutine(command));
        }

        private IEnumerator SendRequestRoutine(string command)
        {
            string sanitized = command.Replace(" ", "");
            string url = $"{espIpAddress}/servo?{sanitized}";

            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                yield return request.SendWebRequest();
            }
        }
    }
}