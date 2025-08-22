using InGame.MyEnum;
using MyUtil;
using UnityEngine;
using DG.Tweening;
using InGame.MyUI.TurnUI;

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

        private bool _canChangeTurn; // 턴을 변경 가능 여부를 결정하는 변수

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

            _currentTeamType = TeamType.Team1; // 처음 시작은 Team1부터
        }

        private void Start()
        {
            MainThreadDispatcher.Enqueue(() =>
            {
                Sequence seq = DOTween.Sequence()
                .Append(TurnChange(TurnType.ChangeTeam)); // 팀 변경 턴으로 변경
            });
        }

        // 턴을 넘기는 함수
        public void NextTurn(TurnType turn)
        {
            if (turn == TurnType.ChangeTeam) // 팀을 변경하는 턴 일경우
            {
                _currentTeamType = GameManager.Instance.NextTeam(_currentTeamType); // 현재 팀을 다음 팀으로 지정
            }
            MainThreadDispatcher.Enqueue(() => TurnChange(turn)); // 턴 변경 및 변경된 턴 기능 실행 함수 호출 - DOTween 사용 및 UI를 건드리는 작업을 하기 때문에 MainThreadDispatcher로 감싸기
        }

        // 턴 변경 시 현재 턴을 다음 턴으로 변경 및 다음 턴의 애니메이션까지 실행 시키는 함수(다음 턴)
        private Sequence TurnChange(TurnType nextTurn)
        {
            return DOTween.Sequence()
                .AppendCallback(() => _currentTurnType = nextTurn) // 현재 턴 다음 턴으로 변경
                .Append(_turnUIAnimation.UIAnimationPlay(nextTurn)) // 다음 턴 애니메이션 실행
                .AppendCallback(() =>
                {
                    if(nextTurn != TurnType.DrawTurn && nextTurn != TurnType.MainTurn)
                        NetworkManager.Instance.Socket.Emit("changeTurn", SceneMgr.Instance.CurrentRoomID); // 서버에 턴 변경 신호를 보냄
                });
        }
    }
}
// 마지막 작성 일자: 2025.08.22