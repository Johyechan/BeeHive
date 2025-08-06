using InGame.MyUI.MyUIInterface;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace InGame.MyUI
{
    // 작성자: 조혜찬
    // 종료 버튼 클래스
    public class QuitButton : MonoBehaviour, IUIClick
    {
        public void OnUIClick()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false; // 에디터에서 플레이 모드 종료
#else
            Application.Quit(); // 실제 빌드 환경에서 종료
#endif
        }
    }
}
// 마지막 작성 일자: 2025.08.06