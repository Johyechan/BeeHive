using InGame.MyManager;
using InGame.MyUI.MyUIInterface;
using Mirror;
using TMPro;
using UnityEngine;

namespace InGame.MyUI
{
    // 작성자: 조혜찬
    // 방을 찾는 버튼 클래스
    public class FindRoomButton : MonoBehaviour, IUIButton
    {
        [SerializeField] private TMP_InputField _inputField; // 방 이름을 적는 inputField

        public void OnUIButtonClick()
        {
            MyNetworkManager myNetMgr = MyNetworkManager.singleton as MyNetworkManager;

            SearchRoomMessage msg = new SearchRoomMessage
            {
                roomName = _inputField.text // 검색할 방 이름 할당
            };

            NetworkClient.Send(msg); // 클라이언트에서 서버로 SearchRoomMessage형식으로 메세지 보내기
        }
    }
}
// 마지막 작성 일자: 2025.07.29