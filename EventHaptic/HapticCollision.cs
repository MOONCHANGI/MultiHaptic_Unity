using UnityEngine;
using System.Collections;

namespace Haptics
{
    public class HapticCollision : MonoBehaviour
    {
        [Header("Target Settings")]
        public string targetTag = "Player";
        public bool enablePhysicalCollision = true;

        [Header("Collision Settings")]
        public string impactCommand = "vibR=180";
        public float duration = 0.2f;

        private void OnCollisionEnter(Collision collision)
        {
            if (!enablePhysicalCollision) return;
            if (collision.gameObject.CompareTag(targetTag))
            {
                StartCoroutine(ProcessHapticRoutine());
            }
        }

        private void OnParticleCollision(GameObject other)
        {
            if (!enablePhysicalCollision) return;
            if (other.CompareTag(targetTag))
            {
                StartCoroutine(ProcessHapticRoutine());
            }
        }

        public void TriggerHaptic()
        {
            StartCoroutine(ProcessHapticRoutine());
        }

        private IEnumerator ProcessHapticRoutine()
        {
            if (HapticManager.Instance != null)
            {
                // [수정] 즉시 전송 (Direct)
                HapticManager.Instance.SendCommandDirect(impactCommand);
            }

            yield return new WaitForSeconds(duration);

            if (HapticManager.Instance != null)
            {
                var parts = impactCommand.Split('=');
                if (parts.Length > 0)
                {
                    // [수정] 끄는 것도 즉시 전송
                    HapticManager.Instance.SendCommandDirect($"{parts[0]}=0");
                }
            }
        }
    }
}