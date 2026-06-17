using InGame.MyManager;
using InGame.MyManager.Global;
using InGame.MyObject;
using InGame.MyUI.MyUIInterface;
using MyUtil.GameMode;
using UnityEngine;
using UnityEngine.EventSystems;

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
                isSurrender = 1 // 항복 여부(1 = true)
            };

            string json = JsonUtility.ToJson(gameOverInfo);

            if (GameModeManager.Instance.CurrentGameMode.UseServer())
                NetworkManager.Instance.Socket.Emit("gameOver", json);

            EventSystem.current.SetSelectedGameObject(null); // 선택한 객체 초기화
        }
    }
}
// 마지막 작성 일자: 2026.03.26