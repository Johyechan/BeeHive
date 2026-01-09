using InGame.MyManager;
using InGame.MyObject;
using UnityEngine;

namespace InGame.MyInput
{
    // 작성자: 조혜찬
    // 소켓에 보낼 이벤트를 가지는 핸들러
    public class InputDrawSocketEventHandler
    {
        public void CallSocketEvent()
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
// 마지막 작성 일자: 2026.01.09