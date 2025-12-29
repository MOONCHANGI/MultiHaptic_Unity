using UnityEngine;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine.Timeline;
using Haptics; // HapticEventClip이 있는 네임스페이스

// HapticEventClip 타입의 클립을 타임라인에서 그릴 때 이 에디터를 사용합니다.
[CustomTimelineEditor(typeof(HapticEventClip))]
public class HapticClipEditor : ClipEditor
{
    // [기능 2] 인스펙터에서 값을 바꿀 때마다 라벨(이름) 자동 업데이트
    public override void OnClipChanged(TimelineClip clip)
    {
        var hapticClip = clip.asset as HapticEventClip;
        if (hapticClip == null) return;

        string command = hapticClip.hapticCommand;

        // 명령어가 비어있지 않다면 이름 업데이트
        if (!string.IsNullOrEmpty(command))
        {
            // 예: "pumpL=180" -> "pumpL (180)" 형태로 보기 좋게 변경
            // 만약 &로 묶인 복합 명령이라면 그대로 표시하거나 첫 번째만 표시
            clip.displayName = FormatDisplayName(command);
        }
    }

    // [기능 1] 세기에 따라 클립 배경색 변경
    public override void DrawBackground(TimelineClip clip, ClipBackgroundRegion region)
    {
        var hapticClip = clip.asset as HapticEventClip;
        if (hapticClip == null) return;

        // 1. 명령어에서 세기(Value) 추출
        float intensity = GetMaxIntensity(hapticClip.hapticCommand);

        // 2. 세기에 따른 색상 계산 (0 ~ 180 기준)
        // 0에 가까우면 파란색/흰색, 180에 가까우면 빨간색이 되도록 보간
        // (Color.Lerp를 사용하여 색상을 섞습니다)
        float t = Mathf.Clamp01(intensity / 180f);

        // 약함(0) = 짙은 파랑, 강함(180) = 붉은색
        Color lowIntensityColor = new Color(0.2f, 0.2f, 0.6f, 1f);
        Color highIntensityColor = new Color(0.8f, 0.2f, 0.2f, 1f);

        Color finalColor = Color.Lerp(lowIntensityColor, highIntensityColor, t);

        // 3. 배경 그리기
        EditorGUI.DrawRect(region.position, finalColor);
    }

    // --- 헬퍼 함수들 ---

    // 명령어 문자열을 보기 좋은 라벨로 변환
    private string FormatDisplayName(string command)
    {
        // "pumpL=180" -> "="를 기준으로 나눔
        string[] parts = command.Split('=');
        if (parts.Length == 2)
        {
            return $"{parts[0]} ({parts[1]})";
        }
        // 복합 명령(&)이거나 형식이 다르면 원래 문자열 반환
        return command;
    }

    // 명령어에서 가장 강한 세기 값을 추출 (색상 결정용)
    private float GetMaxIntensity(string command)
    {
        if (string.IsNullOrEmpty(command)) return 0;

        float maxVal = 0;

        // "pumpL=100&vibR=180" 같은 복합 명령도 고려
        string[] commands = command.Split('&');
        foreach (var cmd in commands)
        {
            string[] parts = cmd.Split('=');
            if (parts.Length == 2)
            {
                if (float.TryParse(parts[1], out float val))
                {
                    if (val > maxVal) maxVal = val;
                }
            }
        }
        return maxVal;
    }
}