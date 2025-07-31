using InGame.MyEnum;
using MyUtil;
using Unity.Android.Gradle;
using UnityEngine;
using DG.Tweening;
using InGame.MyUI.TurnUI;

namespace InGame.MyManager
{
    // 작성자: 조혜찬
    // 턴을 관리하는 싱글톤 매니저 클래스
    public class TurnManager : MonoSingleton<TurnManager>
    {
        private TeamType _currentTeamType; // 현재 턴의 팀
        // 위 변수 프로퍼티
        public TeamType CurrentTeamType { get => _currentTeamType; }

        private TurnType _currentTurnType; // 현재 턴
        // 위 변수 프로퍼티
        public TurnType CurrentTurnType { get => _currentTurnType; }

        // UI 애니메이션을 실행 시키는 클래스
        private TurnUIAnimation _turnUIAnimation;

        // 변수 초기화
        protected override void Awake()
        {
            base.Awake();

            _turnUIAnimation = GetComponent<TurnUIAnimation>();
        }

        // 턴을 넘기는 함수
        public void NextTurn()
        {
            switch(_currentTurnType) // 현재 턴을 확인
            {
                case TurnType.MakeTurn: // 만약 생산 턴이라면
                    _currentTurnType = TurnType.DrawTurn; // 카드 뽑기 결정 턴으로 턴 이동
                    break;
                case TurnType.DrawTurn: // 만약 카드 뽑기 결정 턴이라면
                    _currentTurnType = TurnType.MainTurn; // 생성 및 이동 턴으로 턴 이동
                    break;
                case TurnType.MainTurn: // 생성 및 이동 턴이라면
                    _currentTurnType = TurnType.TurnEnd; // 턴 종료로 턴 이동
                    break;
                case TurnType.TurnEnd: // 턴 종료라면
                    NextTeamTurn(); // 다음 팀의 턴으로 옮기기
                    break;
            }
        }

        // 다음 팀의 턴으로 옮기는 함수
        private void NextTeamTurn()
        {
            Sequence sequence = DOTween.Sequence() // 특정 기능이 완수되면 다음 기능이 실행되도록 하기 위해 DOTween의 시퀀스 사용
                .AppendCallback(() => _turnUIAnimation.UIAnimationPlay(_currentTurnType)) // 현재 턴의 애니메이션(턴 종료) 실행
                .AppendCallback(() => _currentTurnType = TurnType.ChangeTeam) // 팀 변경 턴으로 턴 이동
                .AppendCallback(() => _currentTeamType = GameManager.Instance.NextTeam(_currentTeamType)) // 팀 변경
                .AppendCallback(() => _turnUIAnimation.UIAnimationPlay(_currentTurnType)) // 현재 턴의 애니메이션(팀 변경) 실행
                .AppendCallback(() => _currentTurnType = TurnType.MakeTurn); // 생산 턴으로 턴 이동
        }
    }
}
// 마지막 작성 일자: 2025.07.31