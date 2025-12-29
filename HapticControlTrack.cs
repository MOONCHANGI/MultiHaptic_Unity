using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

namespace Haptics
{
    // =========================================================
    // 1. 기본 부모 트랙 (로직 담당)
    // =========================================================
    // 이 클래스는 타임라인의 기본 동작과 클립 이름 자동 설정 기능을 담당합니다.
    [TrackClipType(typeof(HapticEventClip))]
    [TrackBindingType(typeof(HapticDevice))]
    public class HapticControlTrack : TrackAsset
    {
        // [프레임 버퍼 방식]
        // 더 이상 복잡한 Mixer 스크립트를 연결할 필요가 없습니다.
        // 유니티 기본 믹서를 사용하며, 개별 클립들이 HapticManager에게 직접 값을 보고합니다.

        protected override void OnCreateClip(TimelineClip clip)
        {
#if UNITY_EDITOR
            var director = UnityEditor.Timeline.TimelineEditor.inspectedDirector;
            if (director == null) return;

            var trackBinding = director.GetGenericBinding(this) as HapticDevice;
            var myClip = clip.asset as HapticEventClip;

            // 트랙에 장비가 연결되어 있다면, 클립 생성 시 이름을 장비 명령어로 자동 변경
            if (trackBinding != null && myClip != null)
            {
                myClip.hapticCommand = trackBinding.defaultCommand;
                clip.displayName = trackBinding.defaultCommand; 
            }
#endif
            base.OnCreateClip(clip);
        }
    }

    // =========================================================
    // 2. 색상별 트랙 팔레트 (Visual)
    // =========================================================
    // 기능은 위 부모 클래스(HapticControlTrack)를 그대로 물려받고,
    // 오직 [TrackColor] 속성만 다릅니다.

    // 🔴 빨강 (진동 모터 추천)
    [TrackColor(1.0f, 0.2f, 0.2f)]
    [TrackClipType(typeof(HapticEventClip))]
    [TrackBindingType(typeof(HapticDevice))]
    public class HapticTrackRed : HapticControlTrack { }

    // 🔵 파랑 (에어 펌프 추천)
    [TrackColor(0.2f, 0.4f, 1.0f)]
    [TrackClipType(typeof(HapticEventClip))]
    [TrackBindingType(typeof(HapticDevice))]
    public class HapticTrackBlue : HapticControlTrack { }

    // 🟢 초록 (솔레노이드 밸브 추천)
    [TrackColor(0.2f, 1.0f, 0.2f)]
    [TrackClipType(typeof(HapticEventClip))]
    [TrackBindingType(typeof(HapticDevice))]
    public class HapticTrackGreen : HapticControlTrack { }

    // 🟡 노랑 (주의/강조용)
    [TrackColor(1.0f, 0.8f, 0.0f)]
    [TrackClipType(typeof(HapticEventClip))]
    [TrackBindingType(typeof(HapticDevice))]
    public class HapticTrackYellow : HapticControlTrack { }

    // 🟣 보라 (펠티어 소자 추천)
    [TrackColor(0.8f, 0.2f, 1.0f)]
    [TrackClipType(typeof(HapticEventClip))]
    [TrackBindingType(typeof(HapticDevice))]
    public class HapticTrackPurple : HapticControlTrack { }

    // ⚫ 검정 (기타/배경용)
    [TrackColor(0.1f, 0.1f, 0.1f)]
    [TrackClipType(typeof(HapticEventClip))]
    [TrackBindingType(typeof(HapticDevice))]
    public class HapticTrackDark : HapticControlTrack { }
}