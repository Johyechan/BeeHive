using InGame.MyManager;
using InGame.MyManager.Global;
using InGame.MyUI.MyUIInterface;
using TMPro;
using UnityEngine;

namespace InGame.MyUI
{
    // 작성자: 조혜찬
    // 방을 찾는 버튼 클래스
    public class FindRoomButton : MonoBehaviour, IUIClick
    {
        [SerializeField] private TMP_InputField _roomIDField; // 방 ID를 적는 inputField

        public void OnUIClick()
        {
            // 방에 참가할 때 필요한 구조체 선언
            JoinRoomValue joinRoomValue = new JoinRoomValue 
            {
                roomID = _roomIDField.text, // 인풋 필드에 적은 텍스트를 ID에 할당
                socketName = NetworkManager.Instance.CurrentClientName // socketName을 현재 클라이언트의 닉네임으로 설정
            };

            string json = JsonUtility.ToJson(joinRoomValue); // 구조체를 Json 형태로 변환

            NetworkManager.Instance.Socket.Emit("joinRoom", json); // 서버에 방을 찾고 있다고 신호를 보냄
            _roomIDField.text = ""; // 인풋 필드를 빈 칸으로 초기화
        }
    }
}
// 마지막 작성 일자: 2026.02.03