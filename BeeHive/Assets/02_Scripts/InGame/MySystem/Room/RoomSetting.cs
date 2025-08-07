using TMPro;
using System;
using UnityEngine;
using InGame.MyManager;
using System.Runtime.CompilerServices;
using MyUtil;
using UnityEngine.UI;

namespace InGame.MySystem.Room
{
    [Serializable] // 직렬화하여 JsonUtility에서 파싱 가능하도록 변경
    public struct PlayerData
    {
        public string id; // 플레이어 ID
        public string nickName; // 플레이어 이름
        public bool isRoomManager; // 방장 여부
        public bool isReady; // 준비 완료 여부
    }

    [Serializable] // 직렬화하여 인스펙터 창에서 값을 할당 가능 + 플레이어 정보 구조체
    public struct PlayerInfo
    {
        public TMP_Text playerNameText; // 플레이어 이름
        public Toggle isRoomManagerToggle; // 방장 여부 토글
        public TMP_Text readyText; // 준비 여부 텍스트
        public Image readyImage; // 준비 여부 이미지
        public Button exileButton; // 추방 버튼
        public Button gameStartButton; // 게임 시작 버튼
        public Button readyButton; // 게임 준비 버튼
    }

    // 방 정보 구조체
    [Serializable] // 직렬화
    public struct RoomInfo
    {
        public string ID; // 방 ID 
        public string Name; // 방 이름
        public int maxPlayer; // 최대 입장 가능한 플레이어 수
        public string host; // 방장
        public PlayerData[] players; // 플레이어들 - 플레이어 정보 UI에 배치 값을 주기 위해 필요한 변수
    }
    // 작성자: 조혜찬
    // 룸 세팅 클래스
    public class RoomSetting : MonoBehaviour
    {
        [SerializeField] private TMP_Text _roomName; // 방 이름
        [SerializeField] private TMP_Text _roomID; // 방 ID
        [SerializeField] private PlayerInfo _player1; // player1 정보
        [SerializeField] private PlayerInfo _player2; // player2 정보
        [SerializeField] private PlayerInfo _player3; // player3 정보

        [SerializeField] private GameObject _player3UI; // player3 정보 UI 객체
        [SerializeField] private GameObject _vsUI2; // vs 두 번쨰 이미지 UI 객체

        private PlayerInfoUISettingHandler _playerUISettingHandler; // 플레이어 정보UI를 변경하는 핸들러

        private void Awake()
        {
            _playerUISettingHandler = new PlayerInfoUISettingHandler(_player1, _player2, _player3, _player3UI, _vsUI2);

            var socket = NetworkManager.Instance.Socket;

            if(socket != null ) // 서버와 통신하기 위한 Socket.IO 객체가 null이 아니라면
            {
                if(SceneMgr.Instance.CurrentRoomID != "") // 현재 방 ID가 비어있지 않을 경우
                {
                    socket.Emit("getRoomInfo", SceneMgr.Instance.CurrentRoomID); // 서버에 방 정보를 가져오는 이벤트 호출, 현재 방 ID를 매개 변수로 보내기
                }
            }

            socket.On("roomInfo", (data) =>
            {
                string json = data.GetValue().ToString(); // string 형태로 값 받기
                RoomInfo roomInfo = JsonUtility.FromJson<RoomInfo>(json); // RoomInfo 형태로 json 값을 변경
                MainThreadDispatcher.Enqueue(() => _roomID.text = $"방 ID: {roomInfo.ID}"); // 메인 스레드에서 방 ID UI 변경
                MainThreadDispatcher.Enqueue(() => _roomName.text = $"방 이름: {roomInfo.Name}"); // 메인 스레드에서 방 이름 UI 변경
                MainThreadDispatcher.Enqueue(() => _playerUISettingHandler.RoomInfo = roomInfo); // 방 정보 공유
                MainThreadDispatcher.Enqueue(() => _playerUISettingHandler.Init()); // 플레이어 정보 UI에 관련해서 변경을 하는 함수 실행
            });
        }
    }
}
// 마지막 작성 일자: 2025.08.07