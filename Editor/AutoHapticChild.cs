using UnityEditor;
using UnityEngine;

namespace Haptics
{
    // 에디터가 켜질 때 실행됨
    [InitializeOnLoad]
    public class AutoHapticChild
    {
        static AutoHapticChild()
        {
            EditorApplication.hierarchyChanged += OnHierarchyChanged;
        }

        private static void OnHierarchyChanged()
        {
            if (Selection.activeGameObject == null) return;
            GameObject selectedGo = Selection.activeGameObject;

            if (selectedGo.transform.parent == null) return;

            // 부모가 HapticManager를 가지고 있는지 확인
            if (selectedGo.transform.parent.GetComponent<HapticManager>() != null)
            {
                // 자식에게 HapticDevice가 없으면 자동 추가
                if (selectedGo.GetComponent<HapticDevice>() == null)
                {
                    selectedGo.AddComponent<HapticDevice>();
                    // 네임스페이스 충돌 방지를 위해 명시적으로 UnityEngine.Debug 사용
                    UnityEngine.Debug.Log($"✨ '{selectedGo.name}'에 [Haptics] Device가 자동 추가되었습니다.");
                }
            }
        }
    }
}