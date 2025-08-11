using InGame.MyManager;
using MyUtil;
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
                for (int i = 0; i < _roomInfo.players.Length; i++)
                {
                    _players[i].playerNameText.text = _roomInfo.players[i].nickName; // 각 클라이언트 이름 띄우기

                    if (_roomInfo.players[i].isReady) // n번째 인덱스 플레이어가 준비 완료 상태라면
                    {
                        ReadyUI(i, "준비 완료", Color.green);
                    }
                    else
                    {
                        ReadyUI(i, "준비 중", Color.white);
                    }

                    if (_roomInfo.players[i].id == NetworkManager.Instance.CurrentPlayerID) // 현재 클라이언트 ID와 플레이어 id가 같다면
                    {
                        RoomManagerUI(i, _roomInfo.players[i].isRoomManager); // 방장 관련 UI 변경 함수 실행

                        if (_roomInfo.players[i].isRoomManager) // 현재 클라이언트가 방장인 경우
                            _players[i].exileButton.gameObject.SetActive(false); // 방장의 추방 버튼은 비활성화
                    }
                }
            });
        }

        // 준비 관련 UI 변경 함수
        private void ReadyUI(int index, string text, Color color)
        {
            _players[index].readyText.text = text;
            _players[index].readyImage.color = color;
        }

        // 방장 관련 UI 변경 함수
        private void RoomManagerUI(int index, bool isRoomManager)
        {
            _players[index].roomManagerButton.interactable = !isRoomManager; // 방장일 경우 클릭 금지, 방장이 아닐 경우 클릭 가능

            _readyButton.gameObject.SetActive(!isRoomManager); // 방장일 경우 준비 버튼 비활성화, 방장이 아닐 경우 활성화
            _gameStartButton.gameObject.SetActive(isRoomManager); // 방장일 경우 게임 시작 버튼 활성화, 방장이 아닐 경우 비활성화

            for (int j = 0; j < _roomInfo.players.Length; j++)
            {
                if (_roomInfo.players[j].isRoomManager) // 방장인 플레이어
                {
                    _players[j].roomManagerImage.color = Color.red;// 방장일 경우 빨간색
                    _players[j].roomManagerButton.gameObject.SetActive(isRoomManager); // 방장일 경우 방장 변경 버튼 활성화, 방장이 아닐 경우 비활성화
                }
                else // 방장이 아닌 플레이어
                {
                    _players[j].roomManagerImage.color = Color.white; // 방장이 아닐 경우 흰색
                    _players[j].roomManagerButton.gameObject.SetActive(isRoomManager); // 방장일 경우 방장 변경 버튼 활성화, 방장이 아닐 경우 비활성화
                    Debug.Log(j);
                }

                _players[j].exileButton.gameObject.SetActive(isRoomManager); // 방장일 경우 추방 버튼 활성화, 방장이 아닐 경우 추방 버튼 비활성화
            }
        }
    }
}
// 마지막 작성 일자: 2025.08.11