using InGame.MyUI.MyUIInterface;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace InGame.MyUI.MyUIButton
{
    // 작성자: 조혜찬
    // 임시 튜토리얼 종료 버튼
    public class TempTutorialButton : MonoBehaviour, IUIClick
    {
        public void OnUIClick()
        {
            SceneManager.LoadScene(1);
            EventSystem.current.SetSelectedGameObject(null); // 선택한 객체 초기화
        }
    }
}
// 마지막 작성 일자: 2026.03.26