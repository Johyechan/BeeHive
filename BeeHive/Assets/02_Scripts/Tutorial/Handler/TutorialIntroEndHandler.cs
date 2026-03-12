using DG.Tweening;
using MyUtil.Interface;
using TMPro;
using Tutorial.Event;
using Tutorial.MyEnum;
using UnityEngine;

namespace Tutorial.Handler
{
    // 작성자: 조혜찬
    // 튜토리얼 인트로 종료 핸들러 클래스
    public class TutorialIntroEndHandler : IEventHandler
    {
        private CanvasGroup _tutorialOverlay; // 튜토리얼 UI

        private float _animationDuration; // 애니메이션 지속 시간

        public TutorialIntroEndHandler(CanvasGroup tutorialOverlay, float animationDuration)
        {
            _tutorialOverlay = tutorialOverlay;
            _animationDuration = animationDuration;
        }

        public void Enable()
        {
            TutorialEvents.OnIntroEnd += Function; // 구독
        }

        public void Disable()
        {
            TutorialEvents.OnIntroEnd -= Function; // 구독 해제
        }

        public void Function()
        {
            _tutorialOverlay.DOFade(0, _animationDuration) // 튜토리얼 UI 페이드 아웃
                .OnComplete(() => // 튜토리얼 UI 페이드 아웃 종료 후
                {
                    TutorialManager.Instance.ChangeTutorialState(TutorialState.Turn1_Player); // 첫 번째 턴(플레이어 턴)으로 상태 변경
                }); 
        }
    }
}
// 마지막 작성 일자: 2026.03.12