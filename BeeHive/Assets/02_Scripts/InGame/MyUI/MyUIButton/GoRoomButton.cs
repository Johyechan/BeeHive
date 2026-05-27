using InGame.MyManager;
using InGame.MyManager.Enum;
using InGame.MyManager.Global;
using InGame.MyUI.MyUIInterface;
using MyUtil.GameMode;
using UnityEngine;
using UnityEngine.EventSystems;

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
            };
            string json = JsonUtility.ToJson(goToRoomInfo); // Json 형태로 변환
            if (GameModeManager.Instance.CurrentGameMode.UseServer())
                NetworkManager.Instance.Socket.Emit("goToRoom", json);

            Time.timeScale = 1; // 시간 흐르기

            if(GameModeManager.Instance.CurrentGameMode.IsTutorial()) // 튜토리얼 일 경우
            {
                NetworkManager.Instance.Socket.Emit("tutorialOver"); // 튜토리얼 종료 이벤트 호출
                GameModeManager.Instance.SetMode(new MultiplayMode()); // 게임 모드를 멀티 플레이 모드(기본 모드) 초기화
                SceneMgr.Instance.ChangeCurrentSceneFlow(SceneFlowType.GoLobby);// 로비 씬으로 이동하는 흐름으로 변경
            }
            else // 튜토리얼이 아닐 경우
            {
                SceneMgr.Instance.ChangeCurrentSceneFlow(SceneFlowType.GoRoom);// 방 씬으로 이동하는 흐름으로 변경
            }
            SceneMgr.Instance.LoadScene(); // 씬 전환
            EventSystem.current.SetSelectedGameObject(null); // 선택한 객체 초기화
        }
    }
}
// 마지막 작성 일자: 2026.04.19