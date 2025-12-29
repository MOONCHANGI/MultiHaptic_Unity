using UnityEngine;

namespace Haptics
{
    public class DistanceHaptic : MonoBehaviour
    {
        [Header("Target Settings")]
        public Transform targetPlayer;
        public string playerTag = "Player";

        [Header("Haptic Parameters")]
        public string commandType = "vibR"; // 예: vibR

        public float maxDistance = 5.0f;
        public float minDistance = 1.0f;

        [Range(0, 180)] public int minIntensity = 0;
        [Range(0, 180)] public int maxIntensity = 180;

        // [삭제] 프레임 버퍼 방식에서는 타이머가 필요 없습니다.
        // public float updateInterval = 0.1f; ...

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
            if (HapticManager.Instance == null) return;

            // 매 프레임 계산
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

            // [수정] 직접 명령(SendCommand) 대신, 프레임 버퍼에 값 등록(SetFrameValue)
            // 이렇게 하면 타임라인의 진동과 자연스럽게 섞입니다.
            HapticManager.Instance.SetFrameValue(commandType, intensity);
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