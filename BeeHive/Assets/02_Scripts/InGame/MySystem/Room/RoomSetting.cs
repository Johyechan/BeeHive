using TMPro;
using System;
using UnityEngine;
using InGame.MyManager;
using MyUtil;
using UnityEngine.UI;

namespace InGame.MySystem.Room
{
    // 작성자: 조혜찬
    // 룸 세팅 클래스
    public class RoomSetting : MonoBehaviour
    {
        [SerializeField] private TMP_Text _roomName; // 방 이름
        [SerializeField] private TMP_Text _roomID; // 방 ID
        [SerializeField] private TMP_Text _maxPlayer; // 최대 인원 수

        [SerializeField] private PlayerUI[] _players; // 플레이어 정보 배열

        [SerializeField] private GameObject _player3UI; // player3 정보 UI 객체
        [SerializeField] private GameObject _vsUI2; // vs 두 번쨰 이미지 UI 객체

        [SerializeField] private Button _gameStartButton; // 게임 시작 버튼
        [SerializeField] private Button _readyButton; // 게임 준비 버튼

        private PlayerInfoUISettingHandler _playerUISettingHandler; // 플레이어 정보UI를 변경하는 핸들러

        private PlayerUI _currentPlayerUI; // 현재 클라이언트의 플레이어
        // 현재의 클라이언트 플레이어의 프로퍼티
        public PlayerUI CurrentPlayer { get => _currentPlayerUI; }

        private void Awake()
        {
            _playerUISettingHandler = new PlayerInfoUISettingHandler(_players, _player3UI, _vsUI2, _gameStartButton, _readyButton);

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
                MainThreadDispatcher.Enqueue(() => _maxPlayer.text = $"인원: {roomInfo.players.Length} / {roomInfo.maxPlayer}"); // 메인 스레드에서 인원 수 UI 변경
                MainThreadDispatcher.Enqueue(() => _playerUISettingHandler.RoomInfo = roomInfo); // 방 정보 공유
                MainThreadDispatcher.Enqueue(() => FindCurrentPlayer(roomInfo)); // 방 정보 공유
                MainThreadDispatcher.Enqueue(() => _playerUISettingHandler.Init()); // 플레이어 정보 UI에 관련해서 변경을 하는 함수 실행
            });
        }

        // 현재 클라이언트의 플레이어를 찾는 함수(방 정보)
        private void FindCurrentPlayer(RoomInfo roomInfo)
        {
            for(int i = 0; i < roomInfo.players.Length; i++) // 방 정보에서 방에 있는 플레이어 배열을 순회
            {
                if (roomInfo.players[i].id == NetworkManager.Instance.CurrentPlayerID) // 만약 같은 클라이언트 ID를 가지는 플레이어가 있다면
                {
                    _currentPlayerUI = _players[i]; // n번째 플레이어 정보를 할당
                    return; // 함수 나가기
                }
            }
        }
    }
}
// 마지막 작성 일자: 2025.08.07