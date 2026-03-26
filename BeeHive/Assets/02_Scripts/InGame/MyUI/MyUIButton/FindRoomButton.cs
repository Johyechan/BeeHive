using InGame.MyManager;
using InGame.MyManager.Global;
using InGame.MyUI.MyUIInterface;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

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
            _roomIDField.text = ""; // 인풋 필드를 빈 칸으로 초기화
            EventSystem.current.SetSelectedGameObject(null); // 선택한 객체 초기화
        }
    }
}
// 마지막 작성 일자: 2026.03.26