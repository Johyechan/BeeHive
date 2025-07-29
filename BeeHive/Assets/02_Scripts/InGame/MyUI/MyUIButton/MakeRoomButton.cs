using InGame.MyManager;
using InGame.MyUI.MyUIInterface;
using Mirror;
using TMPro;
using UnityEngine;

namespace InGame.MyUI
{
    // 작성자: 조혜찬
    // 방을 만드는 버튼
    public class MakeRoomButton : MonoBehaviour, IUIButton
    {
        [SerializeField] private TMP_InputField _inputField; // 방 이름을 적을 InputField

        // 클릭 시 실행될 함수
        public void OnUIButtonClick()
        {
            MyNetworkManager myNetMgr = MyNetworkManager.singleton as MyNetworkManager;
            NetworkManager.singleton.StartHost(); // 새로운 서버 IP 만들기 + 현재 클라이언트 입장

            RegisterRoomMessage msg = new RegisterRoomMessage
            {
                roomName = _inputField.text,
                ip = myNetMgr.MainServerIP
            };

            NetworkClient.Send(msg); // 클라이언트에서 서버로 RegisterRoomMessage 형식으로 메세지 보내기
            Debug.Log("방 만들기");
        }
    }
}
// 마지막 작성 일자: 2025.07.29