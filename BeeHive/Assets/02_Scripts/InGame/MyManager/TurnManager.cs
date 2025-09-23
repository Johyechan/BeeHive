using DG.Tweening;
using InGame.MyEnum;
using InGame.MyEvent;
using InGame.MySystem.Game;
using InGame.MyUI.Turn;
using MyUtil;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace InGame.MyManager
{
    // 작성자: 조혜찬
    // 턴을 관리하는 싱글톤 매니저 클래스
    public class TurnManager : MonoSingleton<TurnManager>
    {
        [SerializeField] private float _teamChangeDelay; // 다른 팀의 턴으로 변경하면서 기다리는 시간 변수

        private TeamType _currentTeamType; // 현재 턴의 팀
        // 위 변수 프로퍼티
        public TeamType CurrentTeamType { get => _currentTeamType; }

        private TurnType _currentTurnType; // 현재 턴
        // 위 변수 프로퍼티
        public TurnType CurrentTurnType { get => _currentTurnType; }

        // UI 애니메이션을 실행 시키는 클래스
        private TurnUIAnimation _turnUIAnimation;

        private MakeTurnAddSystem _makeTurnAddSystem; // 생산 턴에 객체들을 추가하는 기능을 가지는 클래스

        private bool _canChangeTurn; // 턴 변경 가능 여부
        public bool CanChangeTurn { get => _canChangeTurn; set => _canChangeTurn = value; } // 위 변수 프로퍼티

        // 변수 초기화
        protected override void Awake()
        {
            base.Awake();

            var socket = NetworkManager.Instance.Socket; // 서버와 통신하기 위한 객체 받아오기
            if(socket != null) // 서버와 통신하기 위한 객체가 존재할 때
            {
                socket.On("turnChanged", value =>
                {
                    int turn = value.GetValue<int>(); // int 자료형으로 읽어오기
                    TurnType turnType = (TurnType)turn; // TurnType형태로 turn변수 변경
                    NextTurn(turnType); // turnType 턴 변경
                });
            }

            _turnUIAnimation = GetComponent<TurnUIAnimation>();
            _makeTurnAddSystem = new MakeTurnAddSystem();

            _currentTeamType = TeamType.Team1; // 처음 시작은 Team1부터
            _makeTurnAddSystem.Init(); // 초기화
        }

        private void Start()
        {
            _ = TurnChange(TurnType.ChangeTeam); // 처음 팀을 알려주기 위해서 현재 팀으로 체인지
        }

        // 턴을 넘기는 함수
        public void NextTurn(TurnType turn)
        {
            if (turn == TurnType.ChangeTeam) // 팀을 변경하는 턴 일경우
            {
                _currentTeamType = GameManager.Instance.NextTeam(_currentTeamType); // 현재 팀을 다음 팀으로 지정
            }

            _= TurnChange(turn); // 턴 변경 및 변경된 턴 기능 실행 함수 호출
        }

        // 턴 변경 시 현재 턴을 다음 턴으로 변경 및 다음 턴의 애니메이션까지 실행 시키는 함수(다음 턴)
        private async Task TurnChange(TurnType nextTurn)
        {
            _currentTurnType = nextTurn; // 현재 턴을 다음 턴으로 변경

            if (_currentTeamType == TeamManager.Instance.CurrentTeamType) // 현재 클라이언트의 팀의 턴이라면
            {
                if (_currentTurnType == TurnType.MakeTurn) // 현재 턴이 생산 턴이라면
                {
                    await TurnEvents.OnMakeTurn.ActionlistPlay(); // 생산 턴의 작업 실행
                }

                if (_currentTurnType == TurnType.TurnEnd) // 현재 턴이면서 턴 종료일 때
                {
                    HighLightEvents.OnPieceMovementHighLight?.Invoke(false, false); // 하이라이트 끄기, 이동 가능 배치 칸 대상
                    HighLightEvents.OnRoadPlacementHighLight?.Invoke(false); // 도로 배치 칸 하이라이트 끄기
                    HighLightEvents.OnPiecePlacementHighLight?.Invoke(false, true); // 기물 배치 칸 하이라이트 끄기, 배치 가능 배치 판 대상
                    await PieceEvents.OnHideCanAttackPieces?.Invoke(); // 공격 가능한 기물들 하이라이트 끄기
                }
            }

            await _turnUIAnimation.UIAnimationPlay(_currentTurnType); // 현재 턴의 작업 실행

            AutoTurnCompleted(); // 턴 완료
        }

        // 서버에 턴 완료 신호를 보내는 함수
        private void AutoTurnCompleted()
        {
            if (_currentTurnType != TurnType.DrawTurn && _currentTurnType != TurnType.MainTurn) // 드로우 턴이 아니면서 메인 턴도 아닐 경우
            {
                TurnCompletedInfo turnCompletedInfo = new TurnCompletedInfo()
                {
                    roomID = SceneMgr.Instance.CurrentRoomID, // 현재 방 ID
                    targetID = NetworkManager.Instance.CurrentPlayerID, // 현재 클라이언트 ID
                };
                string json = JsonUtility.ToJson(turnCompletedInfo); // Json으로 변환
                NetworkManager.Instance.Socket.Emit("turnCompleted", json); // 서버에 턴 변경 신호를 보냄
            }
        }
    }
}
// 마지막 작성 일자: 2025.09.23