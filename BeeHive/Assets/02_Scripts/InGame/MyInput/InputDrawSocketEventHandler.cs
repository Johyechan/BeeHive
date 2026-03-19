using InGame.MyManager;
using InGame.MyManager.Global;
using InGame.MyObject;
using MyUtil.GameMode;
using UnityEngine;

namespace InGame.MyInput
{
    // 작성자: 조혜찬
    // 소켓에 보낼 이벤트를 가지는 핸들러
    public class InputDrawSocketEventHandler
    {
        public void CallSocketEvent()
        {
            if (GameModeManager.Instance.CurrentGameMode.UseServer()) // 게임 서버를 사용하는 경우
            {
                DrawInfo drawInfo = new DrawInfo()
                {
                    roomID = SceneMgr.Instance.CurrentRoomID, // 현재 방 ID
                };

                string json = JsonUtility.ToJson(drawInfo); // Json 형태로 변환
                NetworkManager.Instance.Socket.Emit("draw", json); // 서버에 DrawCompleted 신호 보내기
            }
        }
    }
}
// 마지막 작성 일자: 2026.03.19