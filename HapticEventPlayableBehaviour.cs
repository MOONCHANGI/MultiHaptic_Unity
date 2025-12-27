using UnityEngine;
using UnityEngine.Playables;

namespace Haptics
{
    [System.Serializable]
    public class HapticEventPlayableBehaviour : PlayableBehaviour
    {
        [HideInInspector]
        public string HapticCommand;

        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            // 🔴 [수정됨] Application -> UnityEngine.Application으로 명확하게 지정
            if (UnityEngine.Application.isPlaying == false) return;

            // 매니저가 없으면 중단
            if (HapticManager.Instance == null) return;

            // 명령어 전송
            if (!string.IsNullOrEmpty(HapticCommand))
            {
                HapticManager.Instance.SendCommand(HapticCommand);
            }
        }
    }
}