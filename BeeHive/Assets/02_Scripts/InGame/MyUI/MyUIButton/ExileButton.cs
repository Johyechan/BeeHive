using InGame.MyManager;
using InGame.MyUI.MyUIInterface;
using UnityEngine;

namespace InGame.MyUI.MyUIButton
{
    // 작성자: 조혜찬
    // 추방 버튼 클래스
    public class ExileButton : MonoBehaviour, IUIClick
    {
        private string _targetID; // 나가게 할 대상의 클라이언트 ID
        // 위 변수의 프로퍼티 변수
        public string TargetID { get => _targetID; set => _targetID = value; }

        // 클릭 시 실행될 함수
        public void OnUIClick()
        {
            var socket = NetworkManager.Instance.Socket; // 서버와 통신하기 위한 객체 받기

            if(socket != null) // 서버와 통신하기 위한 객체가 null이 아닐 때
            {
                if(SceneMgr.Instance.CurrentRoomID != "") // 현재 방이 존재할 때
                {
                    // 방을 나갈 때 필요한 값을 가지는 구조체
                    LeaveRoomInfo leaveRoomInfo = new LeaveRoomInfo()
                    {
                        roomID = SceneMgr.Instance.CurrentRoomID, // 현재 방 ID
                        targetID = _targetID
                    };

                    string json = JsonUtility.ToJson(leaveRoomInfo); // 방 나갈 때 필요한 값을 가지는 구조체를 Json 형태로 변환
                    socket.Emit("leaveRoom", json); // 서버에 leaveRoom 이벤트 전달
                }
            }
        }
    }
}
// 마지막 작성 일자: 2025.08.18