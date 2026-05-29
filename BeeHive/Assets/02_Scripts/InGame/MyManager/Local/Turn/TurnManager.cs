using InGame.MyEnum;
using InGame.MyEvent;
using InGame.MyManager.Global;
using InGame.MyManager.Turn.Handler;
using InGame.MySystem.Game;
using InGame.MyUI.Turn;
using MyUtil;
using MyUtil.GameMode;
using System;
using System.Threading.Tasks;
using Tutorial.Event;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.MyManager.Local.Turn
{
    // 작성자: 조혜찬
    // 턴을 관리하는 매니저 클래스
    public class TurnManager : MonoBehaviour
    {
        [SerializeField] private float _teamChangeDelay; // 다른 팀의 턴으로 변경하면서 기다리는 시간 변수

        [SerializeField] private int _turnDurationTime; // 턴 지속 시간(초)
        [SerializeField] private int _makeTurnDelayMillisecond; // 생산 턴 대기 시간

        [SerializeField] private Slider _turnTimerSlider; // 턴 타이머 슬라이더

        private TeamType _currentTeamType; // 현재 턴의 팀
        // 위 변수 프로퍼티
        public TeamType CurrentTeamType { get => _currentTeamType; }

        private TurnType _currentTurnType; // 현재 턴
        // 위 변수 프로퍼티
        public TurnType CurrentTurnType { get => _currentTurnType; }

        // UI 애니메이션을 실행 시키는 클래스
        private TurnUIAnimation _turnUIAnimation;

        private MakeTurnAddSystem _makeTurnAddSystem; // 생산 턴에 객체들을 추가하는 기능을 가지는 클래스

        private TurnTimerHandler _turnTimerHandler; // 턴 타이머 핸들러 클래스
        private TurnTimerUIHandler _turnTimerUIHandler; // 턴 타이머 UI 핸들러 클래스

        private TaskCompletionSource<bool> _roadCreateCompletionTcs; // 도로 생성 완료 tcs
        public TaskCompletionSource<bool> RoadCreateCompletionTcs { get => _roadCreateCompletionTcs; } // 도로 생성 완료 tcs 프로퍼티

        private bool _canChangeTurn; // 턴 변경 가능 여부
        public bool CanChangeTurn { get => _canChangeTurn; set => _canChangeTurn = value; } // 위 변수 프로퍼티

        public Action OnTurnTimerStop; // 턴 타이머 종료 이벤트

        // 변수 초기화
        private void Awake()
        {
            var socket = NetworkManager.Instance.Socket; // 서버와 통신하기 위한 객체 받아오기
            if(socket != null) // 서버와 통신하기 위한 객체가 존재할 때
            {
                socket.On("turnChanged", value =>
                {
                    if (NetworkManager.Instance.IsClientOver) // 클라이언트가 종료 되었다면
                        return; // 반환

                    int turn = value.GetValue<int>(); // int 자료형으로 읽어오기
                    TurnType turnType = (TurnType)turn; // TurnType형태로 turn변수 변경

                    MainThreadDispatcher.Enqueue(() => // NextTurn 함수가 메인스레드에서 실행되어야 함(Unity 기능들을 사용중인 함수)
                    {
                        _ = NextTurn(turnType); // turnType 턴 변경
                    });
                });

                socket.On("nowGameStart", response =>
                {
                    MainThreadDispatcher.Enqueue(() =>
                    {
                        _ = TurnChange(TurnType.ChangeTeam, true);
                    });
                });
            }

            _turnUIAnimation = GetComponent<TurnUIAnimation>();
            _makeTurnAddSystem = new MakeTurnAddSystem();
            _turnTimerHandler = new TurnTimerHandler();
            _turnTimerUIHandler = new TurnTimerUIHandler(_turnTimerSlider, _turnDurationTime);

            _currentTeamType = TeamType.Team1; // 처음 시작은 Team1부터
            _makeTurnAddSystem.Init(); // 초기화
            _turnTimerUIHandler.Init(); // 초기화
        }

        public void OnEnable()
        {
            OnTurnTimerStop += _turnTimerHandler.TurnTimerStopImmediately;
            OnTurnTimerStop += _turnTimerUIHandler.SliderTimerStop;
        }

        public void OnDisable()
        {
            NetworkManager.Instance.Socket.Off("turnChanged");
            NetworkManager.Instance.Socket.Off("nowGameStart");
            _turnTimerUIHandler.Disable();
            _makeTurnAddSystem.Disable();
            OnTurnTimerStop -= _turnTimerHandler.TurnTimerStopImmediately;
            OnTurnTimerStop -= _turnTimerUIHandler.SliderTimerStop;
        }

        // 턴을 넘기는 함수
        public async Task NextTurn(TurnType turn)
        {
            if (turn == TurnType.ChangeTeam) // 팀을 변경하는 턴 일경우
            {
                _currentTeamType = InGameContext.Current.Data.GameManager.NextTeam(_currentTeamType); // 현재 팀을 다음 팀으로 지정
            }

            await TurnChange(turn); // 턴 변경 및 변경된 턴 기능 실행 함수 호출
        }

        // 언어가 바뀌었을 때 실행될 함수
        public void OnLanguageChange(bool isOn)
        {
            if(isOn) // 언어 토글이 선택이 되었을 때
            {
                _turnUIAnimation.SetCurrentTurnUI(_currentTurnType); // 현재 턴을 알려주는 UI 변경
            }
        }

        // 턴 변경 시 현재 턴을 다음 턴으로 변경 및 다음 턴의 애니메이션까지 실행 시키는 함수(다음 턴)
        public async Task TurnChange(TurnType nextTurn, bool isStart = false)
        {
            if(isStart)
            {
                if(TeamManager.Instance.CurrentTeamType == TeamType.Team1) // 시작 시 제작하는 덱은 팀 1이 전담해서 제작
                {
                    InGameContext.Current.Data.DeckManager.MakeDeck(SceneMgr.Instance.CurrentRoomID);
                    await InGameContext.Current.Data.DeckManager.DeckMakeEnd();
                }
                else // 팀 1이 아닐 경우
                {
                    InGameContext.Current.Data.DeckManager.CreateTcs(); // 덱 제작 대기 tcs 생성
                    await InGameContext.Current.Data.DeckManager.DeckMakeEnd(); // 덱 제작 대기
                }
            }

            if(nextTurn == TurnType.TurnEnd) // 다음 턴이 턴 종료 턴이라면
            {
                foreach (var piece in InGameContext.Current.Data.PlacePlaneManager.Variable.placePlaneMap.PiecePlacePlanes) // 전체 기물 판 순회
                {
                    piece.HighLightOff(); // 하이라이트 끄기
                }

                foreach (var road in InGameContext.Current.Data.PlacePlaneManager.Variable.placePlaneMap.RoadPlacePlanes) // 전체 도로 판 순회
                {
                    road.HighLightOff(); // 하이라이트 끄기
                }
            }

            await _turnUIAnimation.UIAnimationPlay(nextTurn); // 배치 관련 배열 초기화 및 현재 턴을 알려주는 애니메이션

            _currentTurnType = nextTurn; // 현재 턴을 다음 턴으로 변경
            _turnTimerUIHandler.SliderTimerStop(); // 턴 타이머 슬라이더 초기화

            if (_currentTeamType == TeamManager.Instance.CurrentTeamType) // 현재 클라이언트의 팀의 턴이라면
            {
                if (_currentTurnType == TurnType.MakeTurn) // 현재 턴이 생산 턴이라면
                {
                    TaskCompletionSource<bool> completionTcs = new TaskCompletionSource<bool>();
                    _roadCreateCompletionTcs = new TaskCompletionSource<bool>();

                    await TurnEvents.OnMakeTurn.ActionlistPlay(); // 생산 턴의 작업 실행
                    WalletEvent.OnSetGold?.Invoke(completionTcs); // 금화 및 금괴의 객체와 UI 세팅
                    await completionTcs.Task;
                    await Task.Delay(_makeTurnDelayMillisecond); // 생산 턴 대기 시간 만큼 대기

                    InGameContext.Current.Data.DrawManager.CanDraw = true; // 드로우 가능 상태
                }
                else if(_currentTurnType == TurnType.DrawTurn || _currentTurnType == TurnType.MainTurn) // 드로우턴 또는 메인턴일 때
                {
                    if (InGameContext.Current.Data.PieceManager.IsDrought) // 가뭄 상태라면
                    {
                        InGameContext.Current.Data.PieceManager.IsDrought = false; // 가뭄 종료

                        InGameContext.Current.Data.CardManager.DroughtState(false, _currentTeamType); // 가뭄 종료
                    }

                    if (!GameModeManager.Instance.CurrentGameMode.IsTutorial()) // 튜토리얼이 아닐 때
                    {
                        _turnTimerHandler.TurnTimerStart(_turnTimerSlider, _turnDurationTime); // 턴 타이머 시작
                    }
                }

                if (NetworkManager.Instance.IsClientOver) // 클라이언트가 종료 되었다면
                    return; // 반환

                NetworkManager.Instance.Socket.Emit("debug", "이벤트 부르기 바로 전"); 
                HighLightEvents.OnPieceMovementHighLight?.Invoke(false, false); // 하이라이트 끄기, 이동 가능 배치 칸 대상
                HighLightEvents.OnRoadPlacementHighLight?.Invoke(false); // 도로 배치 칸 하이라이트 끄기
                HighLightEvents.OnPiecePlacementHighLight?.Invoke(false, true); // 기물 배치 칸 하이라이트 끄기, 배치 가능 배치 판 대상
                PieceEvents.OnHideCanAttackPieces?.Invoke(true); // 공격 가능한 기물들 하이라이트 끄기
            }
            else // 상대 팀 턴일 때
            {
                if(_currentTurnType == TurnType.DrawTurn) // 드로우 턴일 때 (생산 이후 턴)
                {
                    if(_currentTeamType == TeamType.Team2)
                    {
                        if(TeamManager.Instance.Team2FirstTurn) // 팀2의 첫 번째 턴이라면
                        {
                            TeamManager.Instance.Team2FirstTurn = false; // 첫 번째 턴 종료
                        }
                    }

                    if(InGameContext.Current.Data.PieceManager.OpponentDrought) // 상대가 가뭄 상태라면
                    {
                        InGameContext.Current.Data.PieceManager.OpponentDrought = false; // 가뭄 종료

                        InGameContext.Current.Data.CardManager.DroughtState(false, _currentTeamType); // 가뭄 종료
                    }
                }
            }

            if (GameModeManager.Instance.CurrentGameMode.UseServer()) // 현재 게임 모드가 서버를 사용하는 멀티 플레이라면
            {
                NetworkManager.Instance.Socket.Emit("debug", "이벤트 종료");
                AutoTurnCompleted(); // 턴 완료
            }
            else // 현제 게임 모드가 서버를 사용하지 않는다면
            {
                TutorialEvents.OnTurnEnd?.Invoke(); // 튜토리얼 턴 종료 이벤트 호출
            }
        }

        // 서버에 턴 완료 신호를 보내는 함수
        private void AutoTurnCompleted()
        {
            switch (_currentTurnType)
            {
                case TurnType.ChangeTeam:
                case TurnType.MakeTurn:
                case TurnType.TurnEnd:
                    TurnCompletedInfo turnCompletedInfo = new TurnCompletedInfo()
                    {
                        roomID = SceneMgr.Instance.CurrentRoomID, // 현재 방 ID
                        completedTurn = (int)_currentTurnType // 현재 완료한 턴
                    };
                    string json = JsonUtility.ToJson(turnCompletedInfo); // Json으로 변환
                    if(GameModeManager.Instance.CurrentGameMode.UseServer())
                        NetworkManager.Instance.Socket.Emit("turnCompleted", json); // 서버에 턴 변경 신호를 보냄
                    break;
                case TurnType.DrawTurn:
                case TurnType.MainTurn:
                    break;
                default:
                    break;
            }
        }
    }
}
// 마지막 작성 일자: 2026.05.26