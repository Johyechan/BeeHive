using InGame.MyManager;
using MyUtil;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.MySystem.Room
{
    // 작성자: 조혜찬
    // 플레이어 정보 UI 세팅을 하는 핸들러 클래스
    public class PlayerInfoUISettingHandler
    {
        private PlayerUI[] _players = new PlayerUI[3]; // 플레이어 정보 배열

        private RoomInfo _roomInfo; // 방 정보
        // 방 정보 프로퍼티
        public RoomInfo RoomInfo { get => _roomInfo; set => _roomInfo = value; }

        private bool _isTwoPlayer; // 2인용 게임 방인지 확인하는 변수

        private GameObject _player3UI; // 플레이어 3 객체
        private GameObject _vsUI2; // 두 번째 vs 이미지 객체

        private Button _gameStartButton; // 게임 시작 버튼
        private Button _readyButton; // 게임 준비 버튼

        // 생성자에서 변수 초기화
        public PlayerInfoUISettingHandler(PlayerUI[] players, GameObject player3UI, GameObject vsUI2, Button startButton, Button readyButton)
        {
            _players = players;
            _isTwoPlayer = false;
            _player3UI = player3UI;
            _vsUI2 = vsUI2;
            _gameStartButton = startButton;
            _readyButton = readyButton;
        }

        public void Init()
        {
            MainThreadDispatcher.Enqueue(() =>
            {
                if (_roomInfo.maxPlayer < 3) // 최대 입장 가능 플레이어 수가 3인 이하라면
                {
                    _isTwoPlayer = true; // 2인용 플레이어로 할당
                }
            });

            MainThreadDispatcher.Enqueue(() =>
            {
                if (_isTwoPlayer)
                {
                    _player3UI.SetActive(false); // 세 번째 플레이어 정보 UI 비활성화
                    _vsUI2.SetActive(false); // 두 번쨰 vsUI 비활성화 
                }
            });

            MainThreadDispatcher.Enqueue(() =>
            {
                // 여기서 이제 준비 여부 및 게임 시작 여부 결정도 해야 함
                for(int i = 0; i < _roomInfo.players.Length; i++)
                {
                    _players[i].playerNameText.text = _roomInfo.players[i].nickName; // 각 클라이언트 이름 띄우기
                    if (_roomInfo.players[i].isRoomManager) // n번째 인덱스 플레이어가 방장이라면
                    {
                        _players[i].isRoomManagerToggle.isOn = true;// n번째 플레이어 방장 토글 체크
                        _players[i].readyText.text = "준비 완료"; // 방장은 바로 준비 완료 상태
                        _players[i].readyImage.color = Color.green; // 준비 완료 상태 색으로 변경

                        if (_roomInfo.players[i].id == NetworkManager.Instance.CurrentPlayerID) // 현재 클라이언트와 현재 플레이어의 클라 ID가 같다면
                        {
                            _readyButton.gameObject.SetActive(false); // 준비 완료 버튼 비활성화
                            _gameStartButton.gameObject.SetActive(true); // 게임 시작 버튼 활성화

                            for (int j = 0; j < _roomInfo.players.Length; j++) // 다시 플레이어들을 전부 순회하며
                            {
                                if (!_roomInfo.players[j].isRoomManager) // 방장이 아닌 플레이어라면 - 방장 입장에서 추방 버튼이 보여야 함
                                {
                                    _players[j].exileButton.gameObject.SetActive(true); // 추방 버튼 활성화
                                }
                            }
                        }
                    }
                }
            });
        }
    }
}
// 마지막 작성 일자: 2025.08.07