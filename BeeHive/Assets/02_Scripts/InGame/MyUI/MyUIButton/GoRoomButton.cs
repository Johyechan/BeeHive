using InGame.MyManager;
using InGame.MyUI.MyUIInterface;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace InGame.MyUI.MyUIButton
{
    // 작성자: 조혜찬
    // 방 씬으로 돌아가는 버튼
    public class GoRoomButton : MonoBehaviour, IUIClick
    {
        public void OnUIClick()
        {
            SceneManager.LoadScene(1); // 방 씬으로 이동
        }
    }
}
// 마지막 작성 일자: 2025.11.04