using DG.Tweening;
using InGame.MyManager;
using InGame.MyManager.Global;
using MyUtil;
using UnityEngine.Localization.Settings;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using InGame.MyManager.Enum;

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

        [SerializeField] private Button _gameStartButton; // 게임 시작 버튼
        [SerializeField] private Button _readyButton; // 게임 준비 버튼
        [SerializeField] private Button _exileButton; // 추방 버튼
        [SerializeField] private Button _roomManagerChangeButton; // 방장 변경 버튼

        private PlayerInfoUISettingHandler _playerUISettingHandler; // 플레이어 정보UI를 변경하는 핸들러

        private RoomInfo _roomInfo; // 방 정보 변수

        private PlayerUI _currentPlayerUI; // 현재 클라이언트의 플레이어
        // 현재의 클라이언트 플레이어의 프로퍼티
        public PlayerUI CurrentPlayer { get => _currentPlayerUI; }

        private void Awake()
        {
            _playerUISettingHandler = new PlayerInfoUISettingHandler(_players, _gameStartButton, _readyButton, _exileButton, _roomManagerChangeButton);

            var socket = NetworkManager.Instance.Socket;

            if (socket != null) // 서버와 통신하기 위한 Socket.IO 객체가 null이 아니라면
            {
                if (SceneMgr.Instance.CurrentRoomID != "") // 현재 방 ID가 비어있지 않을 경우
                {
                    socket.Emit("getRoomInfo", SceneMgr.Instance.CurrentRoomID); // 서버에 방 정보를 가져오는 이벤트 호출, 현재 방 ID를 매개 변수로 보내기
                }

                socket.On("canStartGame", _ =>
                {
                    if (NetworkManager.Instance.IsClientOver) // 클라이언트가 종료 되었다면
                        return; // 반환

                    MainThreadDispatcher.Enqueue(() => _gameStartButton.interactable = true); // 게임 시작 버튼 활성화
                });

                socket.On("cantStartGame", _ =>
                {
                    if (NetworkManager.Instance.IsClientOver) // 클라이언트가 종료 되었다면
                        return; // 반환

                    MainThreadDispatcher.Enqueue(() => _gameStartButton.interactable = false);// 게임 시작 버튼 비활성화
                });

                socket.On("roomInfo", (data) =>
                {
                    if (NetworkManager.Instance.IsClientOver) // 클라이언트가 종료 되었다면
                        return; // 반환

                    string json = data.GetValue().ToString(); // string 형태로 값 받기
                    _roomInfo = JsonUtility.FromJson<RoomInfo>(json); // RoomInfo 형태로 json 값을 변경
                    RoomUISet(); // 방 UI 세팅
                });

                socket.On("goGame", _ =>
                {
                    if (NetworkManager.Instance.IsClientOver) // 클라이언트가 종료 되었다면
                        return; // 반환

                    MainThreadDispatcher.Enqueue(() =>
                    {
                        Sequence sequence = DOTween.Sequence()
                            .AppendCallback(() =>
                            {
                                SceneMgr.Instance.ChangeCurrentSceneFlow(SceneFlowType.GoGame);// 게임 씬으로 이동하는 흐름으로 변경
                                SceneMgr.Instance.LoadScene(); // 씬 전환
                            })
                            .AppendCallback(() =>
                            {
                                for(int i = 0; i < _roomInfo.players.Length; i++)
                                {
                                    if (_roomInfo.players[i].isRoomManager)
                                    {
                                        if (_roomInfo.players[i].id == NetworkManager.Instance.CurrentPlayerID)
                                        {
                                            socket.Emit("setTeam", SceneMgr.Instance.CurrentRoomID);
                                            break;
                                        }
                                    }
                                }
                            }); // 팀을 정해달라는 이벤트를 서버에게 전달(현재 방 ID)
                    });
                });
            }
        }

        private void OnDisable()
        {
            NetworkManager.Instance.Socket.Off("canStartGame");
            NetworkManager.Instance.Socket.Off("cantStartGame");
            NetworkManager.Instance.Socket.Off("roomInfo");
            NetworkManager.Instance.Socket.Off("goLobby");
            NetworkManager.Instance.Socket.Off("goGame");
        }

        // 언어가 바뀌었을 때 실행될 함수
        public void OnLanguageChange(bool isOn)
        {
            if(isOn) // 토글이 켜졌을 때
            {
                RoomUISet(); // UI 초기화
            }
        }

        // 방 UI 세팅 함수
        private void RoomUISet()
        {

            MainThreadDispatcher.Enqueue(() =>
            {
                string str = LocalizationSettings.StringDatabase.GetLocalizedString(
                    "Room",
                    "Room_UI_RoomIDTitle",
                    new object[] { _roomInfo.ID }
                );

                _roomID.text = str; // 메인 스레드에서 방 ID UI 변경
            }); 
            MainThreadDispatcher.Enqueue(() =>
            {
                string str = LocalizationSettings.StringDatabase.GetLocalizedString(
                    "Room",
                    "Room_UI_RoomName",
                    new object[] { _roomInfo.Name }
                );

                _roomName.text = str;
            }); // 메인 스레드에서 방 이름 UI 변경
            MainThreadDispatcher.Enqueue(() =>
            {
                string str = LocalizationSettings.StringDatabase.GetLocalizedString(
                    "Room",
                    "Room_UI_MaxPlayer",
                    new object[] { _roomInfo.players.Length, _roomInfo.maxPlayer }
                );

                _maxPlayer.text = str;
            }); // 메인 스레드에서 인원 수 UI 변경
            MainThreadDispatcher.Enqueue(() => _playerUISettingHandler.RoomInfo = _roomInfo); // 방 정보 공유
            MainThreadDispatcher.Enqueue(() => FindCurrentPlayer(_roomInfo)); // 방 정보 공유
            MainThreadDispatcher.Enqueue(() => _playerUISettingHandler.Init()); // 플레이어 정보 UI에 관련해서 변경을 하는 함수 실행

            bool isRoomManager = false;

            for (int i = 0; i < _roomInfo.players.Length; i++)
            {
                if (_roomInfo.players[i].isRoomManager) // 방장이라면
                {
                    isRoomManager = NetworkManager.Instance.CurrentPlayerID == _roomInfo.players[i].id; // 현재 클라이언트 방장 여부 할당
                }
            }

            if (isRoomManager) // 현재 클라이언트가 방장이라면
            {
                int count = 0;

                _exileButton.gameObject.SetActive(true); // 추방 버튼 활성화
                _roomManagerChangeButton.gameObject.SetActive(true); // 방장 변경 버튼 활성화

                _exileButton.interactable = _roomInfo.players.Length > 1; // 방에 있는 플레이어가 자신 밖에 없다면 추방 버튼 클릭 비활성화
                _roomManagerChangeButton.interactable = _roomInfo.players.Length > 1; //  방에 있는 플레이어가 자신 밖에 없다면 방장 버튼 클릭 비활성화

                for (int i = 0; i < _roomInfo.players.Length; i++) // 현재 방에 있는 플레이어 순회
                {
                    if (_roomInfo.players[i].isReady) // 해당 플레이어가 준비가 되어있다면
                    {
                        count++; // 카운팅
                    }
                }

                if (count >= _roomInfo.maxPlayer) // 카운팅이 최대 플레이어 수 이상이라면
                {
                    NetworkManager.Instance.Socket.Emit("playGameButtonOn");
                }
            }
            else // 방장이 아니라면
            {
                _exileButton.gameObject.SetActive(false); // 추방 버튼 비활성화
                _roomManagerChangeButton.gameObject.SetActive(false); // 방장 변경 버튼 비활성화
            }

            RoomReady.Gate.Completed(); // 방 준비 완료
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
// 마지막 작성 일자: 2026.04.19