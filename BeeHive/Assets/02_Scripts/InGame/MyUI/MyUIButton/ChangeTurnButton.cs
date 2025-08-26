using InGame.MyEvent;
using InGame.MyManager;
using InGame.MyUI.MyUIInterface;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.MyUI.MyUIButton
{
    // 작성자: 조혜찬
    // 턴 변경 버튼 클래스
    public class ChangeTurnButton : MonoBehaviour, IUIClick
    {
        private Button _button; // 현재 버튼

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            TurnEvents.OnSetInteractable += SetInteractable;
        }

        private void OnDisable()
        {
            TurnEvents.OnSetInteractable -= SetInteractable;
        }

        private void SetInteractable(bool interactable)
        {
            _button.interactable = interactable;
        }

        // 클릭 시 실행될 함수
        public void OnUIClick()
        {
            var socket = NetworkManager.Instance.Socket; // 서버와 통신하기 위한 객체 받아오기

            if (socket != null) // 서버와 통신하기 위한 객체가 존재할 때
            {
                if(TurnManager.Instance.CurrentTeamType == TeamManager.Instance.CurrentTeamType) // 현재 턴의 팀이 내 팀일 경우
                    socket.Emit("changeTurn", SceneMgr.Instance.CurrentRoomID); // 서버에 턴 변경 이벤트 전달
            }
        }
    }
}
// 마지막 작성 일자: 2025.08.22