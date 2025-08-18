using InGame.MyManager;
using InGame.MyUI.MyUIInterface;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.MyUI
{
    // 작성자: 조혜찬
    // 방을 만드는 버튼
    public class MakeRoomButton : MonoBehaviour, IUIClick
    {
        [SerializeField] private GameObject _loadingUI; // 로딩창 UI

        [SerializeField] private TMP_InputField _roomNameField; // 방 이름을 적을 InputField

        [SerializeField] private Toggle _twoPlayer; // 2인 플레이어 방인지 토글을 통해 확인
        [SerializeField] private Toggle _threePlayer; // 3인 플레이어 방인지 토글을 통해 확인

        // 클릭 시 실행될 함수
        public void OnUIClick()
        {
            CreateRoomValue roomValue = new CreateRoomValue(); // 방을 만들 때 필요한 값을 가지는 구조체
            roomValue.roomName = _roomNameField.text; // 방 이름 할당
            _roomNameField.text = "";
            if (_twoPlayer.isOn) // 2인 플레이어 방이라면
                roomValue.maxPlayer = 2; // 최대 입장 가능 인원 수 2
            else if(_threePlayer.isOn) // 3인 플레이어 방이라면
                roomValue.maxPlayer = 3; // 최대 입장 가능 인원 수 3
            else
            {
                Debug.Log("인원 수를 선택해야 합니다");
                return;
            }
            roomValue.socketName = NetworkManager.Instance.CurrentClientName; // 현재 클라이언트 닉네임

            string json = JsonUtility.ToJson(roomValue); // JSON 형태로 감싸기

            NetworkManager.Instance.Socket.Emit("createRoom", json); // 방 생성을 서버에 요청
        }
    }
}
// 마지막 작성 일자: 2025.08.18