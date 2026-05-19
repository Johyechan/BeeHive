using InGame.MyEnum;
using InGame.MyEvent;
using InGame.MyManager;
using InGame.MyManager.Global;
using InGame.MyManager.Local;
using InGame.MySystem.Game;
using MyUtil.GameMode;
using System;
using TMPro;
using UnityEngine;

namespace InGame.MySystem
{
    // 작성자: 조혜찬
    // 지갑 클래스
    public class Wallet : MonoBehaviour
    {
        [SerializeField] private TMP_Text _team1GoldCoinText; // 팀 1 금화 개수 텍스트
        [SerializeField] private TMP_Text _team1GoldBarText; // 팀 1 금괴 개수 텍스트
        [SerializeField] private TMP_Text _team2GoldCoinText; // 팀 2 금화 개수 텍스트
        [SerializeField] private TMP_Text _team2GoldBarText; // 팀 2 금괴 개수 텍스트

        [SerializeField] private float _team1GoldCoinInterval; // 팀 1 금화 간격
        [SerializeField] private float _team1GoldBarInterval; // 팀 1 금괴 간격
        [SerializeField] private float _team2GoldCoinInterval; // 팀 2 금화 간격
        [SerializeField] private float _team2GoldBarInterval; // 팀 2 금괴 간격
        [SerializeField] private float _zInterval; // z축 간격

        [SerializeField] private int _zValueChangeCount; // z축 값이 변경되는 개수 
        [SerializeField] private int _goldBarMaxCount; // 금괴 최대 개수
        [SerializeField] private int _makeDelayMillisecond; // 생성 대기 시간

        [SerializeField] private Color _team1OriginalColor; // 팀 1 텍스트 기본 색상
        [SerializeField] private Color _team2OriginalColor; // 팀 2 텍스트 기본 색상

        private int _goldCoinCount = 0; // 금화 개수
        private int _goldBarCount = 0; // 금괴 개수
        private int _tutorialGoldCoinCount = 0; // 튜토리얼 금화 개수
        private int _tutorialGoldBarCount = 0; // 튜토리얼 금괴 개수
        private int _nextTurnGoldCoin = 0; // 다음 턴 금화 수
        public int NextTurnGoldCoin { get => _nextTurnGoldCoin; }
        private int _nextTurnGoldBar = 0; // 다음 턴 금괴 수
        public int NextTurnGoldBar { get => _nextTurnGoldBar; }

        private GoldSetHandle _goldSetHandle;

        private WalletUIHandle _walletUIHandle; // 지갑 UI 핸들러

        private WalletObjectHandle _walletObjectHandle; // 지갑 관련 객체 핸들러
        public WalletObjectHandle WalletObjectHandle { get => _walletObjectHandle; } // 위 변수 프로퍼티

        private async void Awake()
        {
            await GameReady.Gate.WaitAsync();

            if(GameModeManager.Instance.CurrentGameMode.IsTutorial())
            {
                _goldBarCount = 2;
                _tutorialGoldBarCount = 1;
            }

            _goldSetHandle = new GoldSetHandle(this);

            _walletUIHandle = new WalletUIHandle(_team1GoldCoinText, _team1GoldBarText, _team2GoldCoinText, _team2GoldBarText, _goldBarMaxCount, _team1OriginalColor, _team2OriginalColor);

            _walletObjectHandle = new WalletObjectHandle(_team1GoldCoinInterval, _team1GoldBarInterval, _team2GoldCoinInterval, _team2GoldBarInterval, _zInterval, _zValueChangeCount, _goldBarMaxCount, _team1OriginalColor, _team2OriginalColor, _team1GoldCoinText, _team1GoldBarText, _team2GoldCoinText, _team2GoldBarText);

            _walletUIHandle.SetUI(_goldCoinCount, _goldBarCount); // 금화, 금괴 UI 초기화
        }

        private void OnEnable()
        {
            WalletEvent.OnGetGoldCoin += GetGoldCoin; // 금화 획득 이벤트에 금화 획득 함수 구독
            WalletEvent.OnGetGoldBar += GetGoldBar; // 금괴 획득 이벤트에 금괴 획득 함수 구독
            WalletEvent.OnUseGoldBar += UseGoldBar; // 금괴 사용 이벤트에 금괴 사용 함수 구독
            WalletEvent.OnCanUseGoldBar += CanUseGoldBar; // 금괴 사용 여부 확인 이벤트에 금괴 사용 여부 확인 함수 구독
            WalletEvent.OnSetGold += SetGold; // 금괴 및 금화 세팅 이벤트에 함수 구독
            
        }

        private void OnDisable()
        {
            WalletEvent.OnGetGoldCoin -= GetGoldCoin; // 금화 획득 이벤트에 금화 획득 함수 구독 해제
            WalletEvent.OnGetGoldBar -= GetGoldBar; // 금괴 획득 이벤트에 금괴 획득 함수 구독 해제
            WalletEvent.OnUseGoldBar -= UseGoldBar; // 금괴 사용 이벤트에 금괴 사용 함수 구독 해제
            WalletEvent.OnCanUseGoldBar -= CanUseGoldBar; // 금괴 사용 여부 확인 이벤트에 금괴 사용 여부 확인 함수 구독 해제
            WalletEvent.OnSetGold -= SetGold; // 금괴 및 금화 세팅 이벤트에 함수 구독 해제
        }

        private void SetGold()
        {
            GoldSetEventEmit();

            _walletUIHandle.SetUI(_goldCoinCount, _goldBarCount); // UI 변경

            _goldSetHandle.GoldCoinSetting((int)TeamManager.Instance.CurrentTeamType, _goldCoinCount); // 객체 변경
            _goldSetHandle.GoldBarSetting((int)TeamManager.Instance.CurrentTeamType, _goldBarCount); // 객체 변경
        }

        // 금화를 얻는 함수(얻는 값)
        private void GetGoldCoin(int value)
        {
            if (GameModeManager.Instance.CurrentGameMode.IsTutorial())
            {
                switch(InGameContext.Current.Data.TurnManager.CurrentTeamType)
                {
                    case TeamType.Team1:
                        _goldCoinCount += value; // 금화 증가

                        ChangeGoldCoinToGoldBar(); // 함수를 통해 금화를 금괴로 치환

                        _walletUIHandle.SetUI(_goldCoinCount, _goldBarCount); // UI 변경
                        break;
                    case TeamType.Team2:
                        _tutorialGoldCoinCount += value; // 금화 증가

                        ChangeGoldCoinToGoldBar(); // 함수를 통해 금화를 금괴로 치환
                        break;
                }

                switch (InGameContext.Current.Data.TurnManager.CurrentTeamType)
                {
                    case TeamType.Team1:
                        _goldSetHandle.GoldCoinSetting((int)TeamType.Team1, _goldCoinCount); // 객체 변경
                        break;
                    case TeamType.Team2:
                        _goldSetHandle.GoldCoinSetting((int)TeamType.Team2, _tutorialGoldCoinCount); // 객체 변경
                        break;
                }
            }
            else
            {
                _goldCoinCount += value; // 금화 증가

                ChangeGoldCoinToGoldBar(); // 함수를 통해 금화를 금괴로 치환
            }
        }

        // 금괴를 얻는 함수(얻는 값)
        private void GetGoldBar(int value, bool directChange)
        {
            if(GameModeManager.Instance.CurrentGameMode.IsTutorial()) // 튜토리얼일 경우
            {
                switch(InGameContext.Current.Data.TurnManager.CurrentTeamType)
                {
                    case TeamType.Team1:
                        if (_goldBarCount >= _goldBarMaxCount) // 금괴 수가 최대 금괴 수 이상이라면
                        {
                            return; // 반환
                        }

                        _goldBarCount += value; // 금괴 증가

                        _walletUIHandle.SetUI(_goldCoinCount, _goldBarCount); // UI 변경
                        break;
                    case TeamType.Team2:
                        if (_tutorialGoldBarCount >= _goldBarMaxCount) // 금괴 수가 최대 금괴 수 이상이라면
                        {
                            return; // 반환
                        }

                        _tutorialGoldBarCount += value; // 금괴 증가
                        break;
                }


                switch (InGameContext.Current.Data.TurnManager.CurrentTeamType)
                {
                    case TeamType.Team1:
                        _goldSetHandle.GoldBarSetting((int)TeamType.Team1, _goldBarCount); // 객체 변경
                        break;
                    case TeamType.Team2:
                        _goldSetHandle.GoldBarSetting((int)TeamType.Team2, _tutorialGoldBarCount); // 객체 변경
                        break;
                }
            }
            else // 튜토리얼이 아닐 경우
            {
                if (_goldBarCount >= _goldBarMaxCount) // 금괴 수가 최대 금괴 수 이상이라면
                {
                    return; // 반환
                }

                _goldBarCount += value; // 금괴 증가

                BlockGoldBarMaxOver(); // 금괴가 최대 금괴 수 초과로 벌리는 것을 막는 함수

                if(directChange) // 즉시 변경일 경우
                {
                    SetGold(); // 골드 세팅
                }
            }
        }

        // 금괴가 최대 금괴 수를 넘어가는 것을 방지하는 함수
        private void BlockGoldBarMaxOver()
        {
            if (_goldBarCount >= _goldBarMaxCount) // 금괴 개수가 최대라면
            {
                _goldBarCount = _goldBarMaxCount; // 금괴 개수를 최대로 고정
            }
        }

        // 금괴 사용 함수
        private bool UseGoldBar(int value)
        {
            if (!CanUseGoldBar(value)) // 사용하려는 값보다 금괴 수가 적다면
                return false; // false 반환

            if(GameModeManager.Instance.CurrentGameMode.IsTutorial()) // 튜토리얼 일 때
            {
                switch(InGameContext.Current.Data.TurnManager.CurrentTeamType)
                {
                    case TeamType.Team1:
                        _goldBarCount -= value; // 금괴 감소
                        _walletUIHandle.SetUI(_goldCoinCount, _goldBarCount); // UI 변경
                        _goldSetHandle.GoldBarSetting((int)TeamType.Team1, _goldBarCount); // 객체 변경
                        return true;
                    case TeamType.Team2:
                        _tutorialGoldBarCount -= value; // 금괴 감소
                        _goldSetHandle.GoldBarSetting((int)TeamType.Team2, _tutorialGoldBarCount); // 객체 변경
                        return true;
                    default: // 예외 발생 시
                        return false;
                }
            }
            else
            {
                _goldBarCount -= value; // 금괴 감소

                GoldSetEventEmit();

                _walletUIHandle.SetUI(_goldCoinCount, _goldBarCount); // UI 변경

                _goldSetHandle.GoldBarSetting((int)TeamManager.Instance.CurrentTeamType, _goldBarCount); // 객체 변경
                return true;
            }
        }

        // 금괴 사용 여부 판단 함수
        private bool CanUseGoldBar(int value)
        {
            if(GameModeManager.Instance.CurrentGameMode.IsTutorial()) // 튜토리얼 일 경우
            {
                switch(InGameContext.Current.Data.TurnManager.CurrentTeamType) // 현재 턴의 팀이
                {
                    case TeamType.Team1:
                        if (_goldBarCount < value) // 사용하려는 값보다 금괴 수가 적다면
                            return false; // false 반환
                        break;
                    case TeamType.Team2:
                        if (_tutorialGoldBarCount < value) // 사용하려는 값보다 금괴 수가 적다면
                            return false; // false 반환
                        break;
                }
            }
            else
            {
                if (_goldBarCount < value) // 사용하려는 값보다 금괴 수가 적다면
                    return false; // false 반환
            }

            return true; // true 반환
        }

        // 금화를 금괴로 바꾸는 함수
        private void ChangeGoldCoinToGoldBar()
        {
            if (GameModeManager.Instance.CurrentGameMode.IsTutorial()) // 튜토리얼 일 때
            {
                if(_tutorialGoldCoinCount >= 5) // 튜토리얼 금화가 5개 이상이면
                {
                    _tutorialGoldCoinCount -= 5; // 튜토리얼 금화 5개 감수
                    _tutorialGoldBarCount++; // 튜토리얼 금괴 1 증가
                }
            }

            while(_goldCoinCount >= 5) // 금화가 5개 이상일 동안
            {
                _goldCoinCount -= 5; // 금화 5개 감소
                _goldBarCount++; // 금괴 1 증가
            }

            BlockGoldBarMaxOver(); // 금괴가 최대 금괴 수 초과로 벌리는 것을 막는 함수
        }

        private void GoldSetEventEmit()
        {
            ChangeGoldInfo goldSetInfo = new ChangeGoldInfo()
            {
                roomID = SceneMgr.Instance.CurrentRoomID, // 현재 방 ID
                goldCoinCount = _goldCoinCount, // 금화 개수
                goldBarCount = _goldBarCount // 금괴 개수
            };

            string json = JsonUtility.ToJson(goldSetInfo);
            if (GameModeManager.Instance.CurrentGameMode.UseServer())
                NetworkManager.Instance.Socket.Emit("changeGold", json);
        }

        public void CheckNextTurnGoldCoinAndGoldBar(TeamType team)
        {
            if(team == TeamManager.Instance.CurrentTeamType) // 내 팀일 때
            {
                _nextTurnGoldCoin = _goldCoinCount;
                _nextTurnGoldBar = _goldBarCount + 2; // 매턴 2 벌기 때문에 + 2
            }
            else // 상대 팀일 때
            {
                Transform GoldCoinParent = null;
                Transform GoldBarParent = null;

                switch (team)
                {
                    case TeamType.Team1:
                        GoldCoinParent = GameObject.Find("Player1GoldCoins").transform;
                        GoldCoinParent = GameObject.Find("Player1GoldBars").transform;
                        break;
                    case TeamType.Team2:
                        GoldCoinParent = GameObject.Find("Player2GoldCoins").transform;
                        GoldCoinParent = GameObject.Find("Player2GoldBars").transform;
                        break;
                }
                

                _nextTurnGoldCoin = GoldCoinParent.childCount;
                _nextTurnGoldBar = GoldBarParent.childCount + 2;
            }

            CheckMinerDigValue(team);

            while (_nextTurnGoldCoin >= 5)
            {
                _nextTurnGoldCoin -= 5;
                _nextTurnGoldBar++;
            }
        }

        private void CheckMinerDigValue(TeamType team)
        {
            switch (team)
            {
                case TeamType.Team1:
                    foreach (Func<int> func in WalletEvent.OnTeam1MinerDigValue.GetInvocationList())
                    {
                        _nextTurnGoldCoin += func.Invoke();
                    }
                    break;
                case TeamType.Team2:
                    foreach (Func<int> func in WalletEvent.OnTeam2MinerDigValue.GetInvocationList())
                    {
                        _nextTurnGoldCoin += func.Invoke();
                    }
                    break;
            }
        }
    }
}
// 마지막 작성 일자: 2026.05.19