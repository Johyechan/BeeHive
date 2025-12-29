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
            GoToRoomInfo goToRoomInfo = new GoToRoomInfo()
            {
                roomID = SceneMgr.Instance.CurrentRoomID, // 현재 방 ID
                clientID = NetworkManager.Instance.CurrentPlayerID // 현재 클라이언트 ID
            };
            string json = JsonUtility.ToJson(goToRoomInfo); // Json 형태로 변환
            NetworkManager.Instance.Socket.Emit("goToRoom", json);

            Time.timeScale = 1; // 시간 흐르기
            SceneManager.LoadScene(2); // 방 씬으로 이동
        }
    }
}
// 마지막 작성 일자: 2025.12.29