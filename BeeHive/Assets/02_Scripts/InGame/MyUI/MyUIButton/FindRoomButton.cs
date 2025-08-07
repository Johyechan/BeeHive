using InGame.MyManager;
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
            NetworkManager.Instance.Socket.Emit("joinRoom", _roomIDField.text); // 서버에 방을 찾고 있다고 신호를 보냄
            _roomIDField.text = "";
        }
    }
}
// 마지막 작성 일자: 2025.08.07