using InGame.MyManager;
using InGame.MyManager.Global;
using InGame.MyObject;
using InGame.MyUI.MyUIInterface;
using UnityEngine;

namespace InGame.MyUI.MyUIButton
{
    // 작성자: 조혜찬
    // 항복 버튼
    public class SurrenderButton : MonoBehaviour, IUIClick
    {
        // 클릭 시 실행될 함수
        public void OnUIClick()
        {
            GameOverInfo gameOverInfo = new GameOverInfo()
            {
                roomID = SceneMgr.Instance.CurrentRoomID, // 현재 방 ID
                loseTeamType = (int)TeamManager.Instance.CurrentTeamType, // 자신의 팀 타입
            };

            string json = JsonUtility.ToJson(gameOverInfo);

            NetworkManager.Instance.Socket.Emit("gameOver", json);
        }
    }
}
// 마지막 작성 일자: 2026.02.03