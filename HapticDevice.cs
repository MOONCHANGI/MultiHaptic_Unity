using UnityEngine;

namespace Haptics
{
    public class HapticDevice : MonoBehaviour
    {
        [Tooltip("이 오브젝트가 트랙에 연결되면, 새로 생성되는 클립에 이 명령어가 자동으로 입력됩니다.")]
        public string defaultCommand = "vibL=180";
    }
}