using Tutorial.Handler;
using UnityEngine;

namespace Tutorial.Event
{
    // 작성자: 조혜찬
    // 이벤트에 구독할 기능을 가지는 핸들러 변수 모음 클래스
    public class TutorialEventHandlerVariables
    {
        private TutorialIntroEndHandler _introEndHandler; // 인트로 종료 핸들러
        private TutorialTurnEndHandler _turnEndhandler; // 턴 종료 핸들러

        // 초기화 함수
        public void Init()
        {
            _introEndHandler = new TutorialIntroEndHandler();
            _turnEndhandler = new TutorialTurnEndHandler();
        }

        // 활성화 시 실행될 함수
        public void Enable()
        {
            _introEndHandler?.Enable();
            _turnEndhandler?.Enable();
        }

        // 비활성화 시 실행될 함수
        public void Disable()
        {
            _introEndHandler?.Disable();
            _turnEndhandler?.Disable();
        }
    }
}
// 마지막 작성 일자: 2026.03.19