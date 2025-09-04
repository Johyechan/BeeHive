using DG.Tweening;
using InGame.MyEnum;
using InGame.MyEvent;
using InGame.MyManager;
using InGame.MyManager.MyPlacePlane;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace InGame.MyUI.Turn
{
    // 작성자: 조혜찬
    // 각 턴에 따라 실행될 작업을 가지는 클래스
    public class TurnUIAnimation : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup; // UI 애니메이션 전체 페이드인, 아웃을 하기 위한 canvasGroup

        [SerializeField] private TMP_Text _tmpText; // 현재 턴을 보여주는 텍스트

        [SerializeField] private float _animationDuration; // 애니메이션 시간

        private Dictionary<TurnType, TurnUIAnimationHandlerBase> _turnAnimations = new Dictionary<TurnType, TurnUIAnimationHandlerBase>();

        // 변수 초기화
        private void Awake()
        {
            _turnAnimations.Add(TurnType.MakeTurn, new MakeTurnUIAnimationHandler(_canvasGroup, _tmpText, _animationDuration));
            _turnAnimations.Add(TurnType.DrawTurn, new DrawTurnHandler(_canvasGroup, _tmpText, _animationDuration));
            _turnAnimations.Add(TurnType.MainTurn, new MainTurnHandler(_canvasGroup, _tmpText, _animationDuration));
            _turnAnimations.Add(TurnType.TurnEnd, new TurnEndUIAnimationHandler(_canvasGroup, _tmpText, _animationDuration));
            _turnAnimations.Add(TurnType.ChangeTeam, new ChangeTeamTurnUIAnimationHandler(_canvasGroup, _tmpText, _animationDuration));

            TurnEvents.OnMakeTurn.Add(GetGoldBar); // 생산 이벤트에 금괴 획득 함수 큐에 추가
        }

        // 금괴 획득 함수
        private async Task GetGoldBar()
        {
            if (TurnManager.Instance.CurrentTeamType != TeamManager.Instance.CurrentTeamType) // 현재 턴의 팀과 내 팀이 다르다면
                return; // 반환

            WalletEvent.OnGetGoldBar?.Invoke(2); // 금괴 2개 획득

            await Task.CompletedTask; // Task 완료 반환
        }

        // 매개 변수로 받은 턴의 UI 애니메이션을 실행
        public Sequence UIAnimationPlay(TurnType currentTurn)
        {
            Sequence seq = DOTween.Sequence()
                .Append(PlacePlaneManager.Instance.FindCanPlacePlane()) // 배치 가능한 칸 찾는 기능 실행
                .Append(_turnAnimations[currentTurn].UIAnimationPlay()); // UI 애니메이션 실행

            return seq;
        }
    }
}
// 마지막 작성 일자: 2025.09.02