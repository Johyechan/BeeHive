using InGame.MyManager;
using InGame.MyManager.Global;
using InGame.MyUI.MyUIInterface;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace InGame.MyUI
{
    // 작성자: 조혜찬
    // 방을 만드는 버튼
    public class MakeRoomButton : MonoBehaviour, IUIClick
    {
        [SerializeField] private GameObject _loadingUI; // 로딩창 UI

        [SerializeField] private TMP_InputField _roomNameField; // 방 이름을 적을 InputField

        [SerializeField] private Toggle _isPublic; // 공개 방인지 토글을 통해 확인
        [SerializeField] private Toggle _isPrivate; // 비공개 방인지 토글을 통해 확인

        // 클릭 시 실행될 함수
        public void OnUIClick()
        {
            CreateRoomValue roomValue = new CreateRoomValue(); // 방을 만들 때 필요한 값을 가지는 구조체
            roomValue.roomName = _roomNameField.text; // 방 이름 할당
            _roomNameField.text = "";
            if (_isPublic.isOn) // 공개 방이라면
                roomValue.isPublic = true;
            else if(_isPrivate.isOn) // 비공개 방이라면
                roomValue.isPublic = false;
            else
            {
                UIManager.Instance.WarningUIMake("공개 여부를 선택하세요");
                EventSystem.current.SetSelectedGameObject(null); // 선택한 객체 초기화
                return;
            }

            string json = JsonUtility.ToJson(roomValue); // JSON 형태로 감싸기

            NetworkManager.Instance.Socket.Emit("createRoom", json); // 방 생성을 서버에 요청
            EventSystem.current.SetSelectedGameObject(null); // 선택한 객체 초기화
        }
    }
}
// 마지막 작성 일자: 2026.03.26