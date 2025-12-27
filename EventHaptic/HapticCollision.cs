using UnityEngine;
using System.Collections;

namespace Haptics
{
    public class HapticCollision : MonoBehaviour
    {
        [Header("Target Settings")]
        [Tooltip("충돌을 감지할 대상의 태그 (기본값: Player)")]
        public string targetTag = "Player";

        [Tooltip("체크를 끄면 물리 충돌(몸통 박치기)에는 반응하지 않습니다. 오직 Event Trigger나 코드로만 작동합니다.")]
        public bool enablePhysicalCollision = true;

        [Header("Collision Settings")]
        public string impactCommand = "vibR=180";
        public float duration = 0.2f;

        // 1. 물리 충돌 (체크박스가 켜져 있을 때만 작동)
        private void OnCollisionEnter(Collision collision)
        {
            if (!enablePhysicalCollision) return;

            if (collision.gameObject.CompareTag(targetTag))
            {
                StartCoroutine(ProcessHapticRoutine());
            }
        }

        // 2. 파티클 충돌 (체크박스가 켜져 있을 때만 작동)
        private void OnParticleCollision(GameObject other)
        {
            if (!enablePhysicalCollision) return;

            if (other.CompareTag(targetTag))
            {
                StartCoroutine(ProcessHapticRoutine());
            }
        }

        // 3. 수동 실행 (Event Trigger용 - 항상 작동함)
        public void TriggerHaptic()
        {
            StartCoroutine(ProcessHapticRoutine());
        }

        private IEnumerator ProcessHapticRoutine()
        {
            if (HapticManager.Instance != null)
            {
                HapticManager.Instance.SendCommand(impactCommand);
            }

            yield return new WaitForSeconds(duration);

            if (HapticManager.Instance != null)
            {
                var parts = impactCommand.Split('=');
                if (parts.Length > 0)
                {
                    HapticManager.Instance.SendCommand($"{parts[0]}=0");
                }
            }
        }
    }
}