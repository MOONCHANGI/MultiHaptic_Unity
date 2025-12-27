using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

#if UNITY_EDITOR
using UnityEditor.Timeline; // 에디터 전용 기능
#endif

namespace Haptics
{
    // 이 트랙은 HapticEventClip만 올릴 수 있고, HapticDevice 오브젝트와 연결됨
    [TrackClipType(typeof(HapticEventClip))]
    [TrackBindingType(typeof(HapticDevice))]
    public class HapticControlTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject owner, int inputCount)
        {
            return Playable.Create(graph, inputCount);
        }

        // 클립을 새로 만들 때 자동으로 이름과 명령어를 설정해주는 편의 기능
        protected override void OnCreateClip(TimelineClip clip)
        {
#if UNITY_EDITOR
            var director = TimelineEditor.inspectedDirector;
            if (director == null) return;

            var trackBinding = director.GetGenericBinding(this) as HapticDevice;
            var myClip = clip.asset as HapticEventClip;

            if (trackBinding != null && myClip != null)
            {
                // 바인딩된 장비의 기본 명령어를 클립에 복사
                myClip.hapticCommand = trackBinding.defaultCommand;
                clip.displayName = trackBinding.defaultCommand; 
            }
#endif
            base.OnCreateClip(clip);
        }
    }
}