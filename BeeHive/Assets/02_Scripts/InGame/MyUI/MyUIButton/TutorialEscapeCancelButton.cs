using InGame.MyUI.MyUIInterface;
using UnityEngine;

namespace InGame.MyUI.MyUIButton
{
    // 작성자: 조혜찬
    // 튜토리얼 종료 취소 버튼
    public class TutorialEscapeCancelButton : MonoBehaviour, IUIClick
    {
        public void OnUIClick()
        {
            Time.timeScale = 1; // 시간 다시 흐르기
        }
    }
}
// 마지막 작성 일자: 2026.06.02