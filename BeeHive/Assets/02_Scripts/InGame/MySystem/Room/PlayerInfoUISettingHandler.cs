using InGame.MyManager;
using InGame.MyManager.Global;
using InGame.MyUI.MyUIButton;
using MyUtil;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
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

        private Button _gameStartButton; // 게임 시작 버튼
        private Button _readyButton; // 게임 준비 버튼
        private Button _exileButton; // 추방 버튼
        private Button _roomManagerChangeButton; // 방장 변경 버튼

        // 생성자에서 변수 초기화
        public PlayerInfoUISettingHandler(PlayerUI[] players, Button startButton, Button readyButton, Button exileButton, Button roomManagerChangeButton)
        {
            _players = players;
            _gameStartButton = startButton;
            _readyButton = readyButton;
            _exileButton = exileButton;
            _roomManagerChangeButton = roomManagerChangeButton;
        }

        public void Init()
        {
            MainThreadDispatcher.Enqueue(() =>
            {
                bool isCurrentRoomManager = false; // 현재 클라이언트가 방장 클라이언트인지 확인하는 변수

                string currentPlayerID = NetworkManager.Instance.CurrentPlayerID; // 현재 클라이언트 ID
                string roomManagerID = null; // 방장 클라이언트 ID 저장 변수

                List<int> existIndexList = new List<int>(); // 플레이어의 UI 슬롯으로 사용되는 인덱스를 저장하는 리스트

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
                    int index = _roomInfo.players[i].index;

                    existIndexList.Add(index); // 현재 플레이어의 인덱스 저장
                    _players[index].playerNameText.text = _roomInfo.players[i].nickName; // 각 클라이언트 이름 띄우기

                    if (_roomInfo.players[i].nickName != NetworkManager.Instance.CurrentClientName) // 자신의 닉네임이 아닌 것은
                    {
                        SceneMgr.Instance.OtherNickName = _roomInfo.players[i].nickName; // 상대팀 닉네임으로 저장
                    }

                    if (_roomInfo.players[i].isReady) // n번째 인덱스 플레이어가 준비 완료 상태라면
                    {
                        string str = LocalizationSettings.StringDatabase.GetLocalizedString(
                            "Room",
                            "Room_UI_Ready"
                        );

                        ReadyUI(index, str, Color.green);
                    }
                    else
                    {
                        string str = LocalizationSettings.StringDatabase.GetLocalizedString(
                            "Room",
                            "Room_UI_NotReady"
                        );

                        ReadyUI(index, str, Color.white);
                    }

                    if (_roomInfo.players[i].id == currentPlayerID)
                    {
                        if (_roomInfo.players[i].isReady) // 준비 상태인 경우
                        {
                            string str = LocalizationSettings.StringDatabase.GetLocalizedString(
                                "Room",
                                "Room_UI_Button_Cancel"
                            );

                            _players[index].readyButtonText.text = str;
                        }
                        else // 준비 중인 경우
                        {
                            string str = LocalizationSettings.StringDatabase.GetLocalizedString(
                                "Room",
                                "Room_UI_Button_Ready"
                            );

                            _players[index].readyButtonText.text = str;
                        }
                    }

                    if (_roomInfo.players[i].isRoomManager) // 방장 슬롯
                    {
                        _players[index].roomManagerImage.gameObject.SetActive(true); // 방장 표시
                    }
                    else // 일반 슬롯
                    {
                        _players[index].roomManagerImage.gameObject.SetActive(false); // 일반 표시
                        ExileButton exileButton = _exileButton.GetComponent<ExileButton>(); // 추방 버튼
                        exileButton.TargetID = _roomInfo.players[i].id; // 타겟 ID 초기화
                        RoomManagerButton roomManagerButton = _roomManagerChangeButton.GetComponent<RoomManagerButton>(); // 방장 변경 버튼
                        roomManagerButton.TargetIndex = index; // 방장 변경 대상 인덱스 할당
                    }
                }

                if (_roomInfo.players.Length < _roomInfo.maxPlayer)// 현재 플레이어 수가 최대 플레이어 수보다 작다면
                {
                    for (int i = 0; i < _roomInfo.maxPlayer; i++) // 최대 플레이어 수만큼 반복
                    {
                        if (!existIndexList.Contains(i)) // 만약 사용하는 인덱스 리스트에 존재하지 않는다면
                        {
                            _players[i].playerNameText.text = "...";
                            string str = LocalizationSettings.StringDatabase.GetLocalizedString(
                                "Room",
                                "Room_UI_NotReady"
                            );
                            _players[i].readyText.text = str; // 준비 중 상태
                            _players[i].roomManagerImage.gameObject.SetActive(false); // 방장 여부 방장 아님으로 초기화
                        }
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
        }
    }
}
// 마지막 작성 일자: 2026.04.06