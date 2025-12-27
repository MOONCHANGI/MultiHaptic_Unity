using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Haptics
{
    [System.Serializable]
    public class HapticEventClip : PlayableAsset, ITimelineClipAsset
    {
        [Header("Haptic Settings")]
        public string hapticCommand = "";

        // 위에서 만든 Behaviour를 템플릿으로 사용
        public HapticEventPlayableBehaviour template = new HapticEventPlayableBehaviour();

        public ClipCaps clipCaps
        {
            get { return ClipCaps.None; }
        }

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<HapticEventPlayableBehaviour>.Create(graph, template);

            // 클립에 적힌 명령어를 실제 재생 로직(Behaviour)으로 복사
            HapticEventPlayableBehaviour clone = playable.GetBehaviour();
            clone.HapticCommand = hapticCommand;

            return playable;
        }
    }
}