using UnityEngine;

// 🔴 같은 namespace Haptics 안에 있으므로, 위 파일의 Manager를 바로 가져다 씁니다.
namespace Haptics
{
    public class DistanceHaptic : MonoBehaviour
    {
        [Header("Target Settings")]
        public Transform targetPlayer;
        public string playerTag = "Player";

        [Header("Haptic Parameters")]
        public string commandType = "vibR";

        public float maxDistance = 5.0f;
        public float minDistance = 1.0f;

        [Range(0, 180)] public int minIntensity = 0;
        [Range(0, 180)] public int maxIntensity = 180;

        [Header("Optimization")]
        public float updateInterval = 0.1f;
        public int changeThreshold = 5;

        private float _timer;
        private int _lastSentValue = -1;

        private void Start()
        {
            if (targetPlayer == null)
            {
                var playerObj = GameObject.FindGameObjectWithTag(playerTag);
                if (playerObj != null) targetPlayer = playerObj.transform;
            }
        }

        private void Update()
        {
            if (targetPlayer == null) return;

            _timer += Time.deltaTime;
            if (_timer < updateInterval) return;
            _timer = 0f;

            CalculateAndSend();
        }

        private void CalculateAndSend()
        {
            float dist = Vector3.Distance(transform.position, targetPlayer.position);
            int intensity = 0;

            if (dist <= maxDistance)
            {
                float t = Mathf.InverseLerp(maxDistance, minDistance, dist);
                intensity = (int)Mathf.Lerp(minIntensity, maxIntensity, t);
            }

            bool isDiffBig = Mathf.Abs(intensity - _lastSentValue) >= changeThreshold;
            bool isTurningOff = (_lastSentValue > 0 && intensity == 0);

            if (isDiffBig || isTurningOff)
            {
                if (HapticManager.Instance != null)
                {
                    HapticManager.Instance.SendCommand($"{commandType}={intensity}");
                    _lastSentValue = intensity;
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, maxDistance);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, minDistance);
        }
    }
}