using InGame.MyManager;
using InGame.MyManager.Global;
using InGame.MyUI.MyUIInterface;
using UnityEngine;
using UnityEngine.EventSystems;

namespace InGame.MyUI.MyUIButton
{
    // 작성자: 조혜찬
    // 방 나가기 버튼 클래스
    public class LeaveRoomButton : MonoBehaviour, IUIClick
    {
        public void OnUIClick()
        {
            var socket = NetworkManager.Instance.Socket; // 서버와 통신하기 위한 객체 가져오기

            if(socket != null) // 서버와 통신하기 위한 객체가 null이 아니라면
            {
                if(SceneMgr.Instance.CurrentRoomID != "") // 방 ID가 있을 때
                {
                    LeaveRoomInfo leaveRoomInfo = new LeaveRoomInfo() // 방을 떠날 때 필요한 값을 가지는 구조체 생성
                    {
                        roomID = SceneMgr.Instance.CurrentRoomID, // 현재 방 ID 할당
                    };

                    string json = JsonUtility.ToJson(leaveRoomInfo); // JSON 형태로 변환
                    socket.Emit("leaveRoom", json);
                }
            }
            EventSystem.current.SetSelectedGameObject(null); // 선택한 객체 초기화
        }
    }
}
// 마지막 작성 일자: 2026.03.26