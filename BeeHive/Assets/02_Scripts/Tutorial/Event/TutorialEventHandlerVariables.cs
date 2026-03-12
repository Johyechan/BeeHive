using Tutorial.Handler;
using UnityEngine;

namespace Tutorial.Event
{
    // 작성자: 조혜찬
    // 이벤트에 구독할 기능을 가지는 핸들러 변수 모음 클래스
    public class TutorialEventHandlerVariables
    {
        private CanvasGroup _tutorialOverlay; // 튜토리얼 UI

        private float _animationDuration; // 애니메이션 지속 시간

        private TutorialIntroEndHandler _introEndHandler; // 인트로 종료 핸들러

        public TutorialEventHandlerVariables(CanvasGroup tutorialOverlay, float animationDuration)
        {
            _tutorialOverlay = tutorialOverlay;
            _animationDuration = animationDuration;
        }

        // 초기화 함수
        public void Init()
        {
            _introEndHandler = new TutorialIntroEndHandler(_tutorialOverlay, _animationDuration);
        }

        // 활성화 시 실행될 함수
        public void Enable()
        {
            _introEndHandler?.Enable();
        }

        // 비활성화 시 실행될 함수
        public void Disable()
        {
            _introEndHandler?.Disable();
        }
    }
}
// 마지막 작성 일자: 2026.03.12