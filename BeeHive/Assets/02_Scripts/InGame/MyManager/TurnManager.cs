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

            _turnUIAnimation = GetComponent<TurnUIAnimation>();
        }

        private void Start()
        {
            Sequence seq = DOTween.Sequence()
                .Append(TurnChange(TurnType.ChangeTeam)) // 팀 변경 턴으로 변경
                .Append(TurnChange(TurnType.MakeTurn)); // 생산 턴으로 변경
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                NextTurn();
            }
        }

        // 턴을 넘기는 함수
        public void NextTurn()
        {
            switch (_currentTurnType) // 현재 턴을 확인
            {
                case TurnType.MakeTurn: // 만약 생산 턴이라면
                    TurnChange(TurnType.DrawTurn); // 드로우 턴으로 변경
                    break;
                case TurnType.DrawTurn: // 만약 카드 뽑기 결정 턴이라면
                    TurnChange(TurnType.MainTurn); // 메인 턴으로 변경
                    break;
                case TurnType.MainTurn: // 생성 및 이동 턴이라면
                    Sequence seq = DOTween.Sequence()
                        .Append(TurnChange(TurnType.TurnEnd)) // 턴 종료로 변경
                        .AppendCallback(() => _currentTeamType = GameManager.Instance.NextTeam(_currentTeamType)) // 팀 변경
                        .Append(TurnChange(TurnType.ChangeTeam)) // 팀 변경 턴으로 변경
                        .Append(TurnChange(TurnType.MakeTurn)); // 생산 턴으로 변경
                    break;
            }
        }

        // 턴 변경 시 현재 턴을 다음 턴으로 변경 및 다음 턴의 애니메이션까지 실행 시키는 함수(다음 턴)
        private Sequence TurnChange(TurnType nextTurn)
        {
            return DOTween.Sequence()
                .AppendCallback(() => _currentTurnType = nextTurn) // 현재 턴 다음 턴으로 변경
                .Append(_turnUIAnimation.UIAnimationPlay(nextTurn)); // 다음 턴 애니메이션 실행
        }
    }
}
// 마지막 작성 일자: 2025.08.01