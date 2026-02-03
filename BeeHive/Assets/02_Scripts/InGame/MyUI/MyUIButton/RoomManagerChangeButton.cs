using InGame.MyManager;
using InGame.MyManager.Global;
using InGame.MyUI.MyUIInterface;
using UnityEngine;

namespace InGame.MyUI.MyUIButton
{
    // 작성자: 조혜찬
    // 방장 변경 버튼 클래스
    public class RoomManagerChangeButton : MonoBehaviour, IUIClick
    {
        private int _targetIndex; // 방장 변경 대상으로 선택된 플레이어 인덱스
        // 플레이어 인덱스 프로퍼티
        public int TargetIndex { get => _targetIndex; set => _targetIndex = value; }

        public void OnUIClick()
        {
            var socket = NetworkManager.Instance.Socket; 

            if(socket != null)// 서버와 통신하기 위한 객체가 null이 아닐 경우
            {
                if(SceneMgr.Instance.CurrentRoomID != "") // 현재 방 ID가 있을 경우
                {
                    ChangeRoomManagerInfo roomManagerInfo = new ChangeRoomManagerInfo // 방장 변경에 필요한 값들을 가지는 구조체 생성
                    {
                        roomID = SceneMgr.Instance.CurrentRoomID, // 현재 방 ID 할당
                        targetIndex = _targetIndex // 인덱스 번호 할당
                    };

                    string json = JsonUtility.ToJson(roomManagerInfo); // 구조체를 json 형태로 변형

                    socket.Emit("changeRoomManager", json);
                }
            }
        }
    }
}
// 마지막 작성 일자: 2026.02.03