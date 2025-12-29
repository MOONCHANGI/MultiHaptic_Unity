using UnityEngine;
using UnityEngine.Playables;

namespace Haptics
{
    [System.Serializable]
    public class HapticEventPlayableBehaviour : PlayableBehaviour
    {
        [HideInInspector]
        public string HapticCommand;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (HapticManager.Instance == null) return;
            if (string.IsNullOrEmpty(HapticCommand)) return;

            // 1. 먼저 '&'를 기준으로 여러 명령어를 쪼갭니다.
            // 예: "pumpL=180&pumpR=180" -> ["pumpL=180", "pumpR=180"]
            string[] commands = HapticCommand.Split('&');

            foreach (var cmd in commands)
            {
                // 빈 문자열 무시
                if (string.IsNullOrWhiteSpace(cmd)) continue;

                // 2. 각 명령어를 '='로 분리하여 장비명과 값을 추출합니다.
                string[] parts = cmd.Split('=');
                if (parts.Length == 2)
                {
                    string device = parts[0].Trim();
                    int value = 0;

                    if (int.TryParse(parts[1], out value))
                    {
                        // 매니저에게 값 등록
                        // (매니저가 나중에 알아서 다시 &로 묶어서 보냅니다)
                        HapticManager.Instance.SetFrameValue(device, value);
                    }
                }
            }
        }
    }
}