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
                bool isCurrentRoomManager = false; // 현재 클라이언트가 방장 클라이언트인지 확인하는 변수

                string currentPlayerID = NetworkManager.Instance.CurrentPlayerID; // 현재 클라이언트 ID
                string roomManagerID = null; // 방장 클라이언트 ID 저장 변수

                for(int i = 0; i < _roomInfo.players.Length; i++)
                {
                    if (_roomInfo.players[i].isRoomManager)
                    {
                        roomManagerID = _roomInfo.players[i].id; // 방장 클라이언트 ID 저장
                        break; // 반복문 탈출
                    }
                }

                isCurrentRoomManager = currentPlayerID == roomManagerID; // 현재 클라이언트가 방장 클라이언트인지 확인


                for (int i = 0; i < _roomInfo.players.Length; i++)
                {
                    
                    if (_roomInfo.players[i].id == null) // 만약 현재 플레이어가 빈 칸이라면
                    {
                        _players[i].playerNameText.text = "비어 있음";
                        _players[i].readyText.text = "준비 중"; // 준비 중 상태
                        _players[i].readyImage.color = Color.white; // 준비 중 상태
                        _players[i].roomManagerImage.color = Color.white; // 방장 여부 방장 아님으로 초기화
                        _players[i].roomManagerButton.gameObject.SetActive(false); // 방장 버튼 비활성화
                        _players[i].exileButton.gameObject.SetActive(false); // 추방 버튼 비활성화
                        continue; // 이번 반복 회차 넘기기
                    }
                    _players[i].playerNameText.text = _roomInfo.players[i].nickName; // 각 클라이언트 이름 띄우기

                    if (_roomInfo.players[i].isReady) // n번째 인덱스 플레이어가 준비 완료 상태라면
                    {
                        ReadyUI(i, "준비 완료", Color.green);
                    }
                    else
                    {
                        ReadyUI(i, "준비 중", Color.white);
                    }

                    if (_roomInfo.players[i].id == currentPlayerID)
                    {
                        if (_roomInfo.players[i].isReady) // 준비 상태인 경우
                        {
                            _players[i].readyButtonText.text = "취소";
                        }
                        else // 준비 중인 경우
                        {
                            _players[i].readyButtonText.text = "준비";
                        }
                    }

                    if (_roomInfo.players[i].isRoomManager) // 방장 슬롯
                    {
                        _players[i].roomManagerImage.color = Color.red; // 방장 표시
                        _players[i].roomManagerButton.gameObject.SetActive(true); // 방장 버튼 활성화
                        _players[i].exileButton.gameObject.SetActive(false); // 방장 슬롯 추방 버튼 비활성화
                    }
                    else // 일반 슬롯
                    {
                        _players[i].roomManagerImage.color = Color.white; // 일반 표시
                        _players[i].roomManagerButton.gameObject.SetActive(isCurrentRoomManager); // 방장 클라이언트라면 활성화
                        _players[i].exileButton.gameObject.SetActive(isCurrentRoomManager); // 방장 클라이언트라면 활성화
                    }
                }

                _readyButton.gameObject.SetActive(!isCurrentRoomManager); // 방장 클라이언트라면 비활성화
                _gameStartButton.gameObject.SetActive(isCurrentRoomManager); // 방장 클라이언트라면 활성화
            });
        }

        // 준비 관련 UI 변경 함수
        private void ReadyUI(int index, string text, Color color)
        {
            _players[index].readyText.text = text;
            _players[index].readyImage.color = color;
        }
    }
}
// 마지막 작성 일자: 2025.08.12