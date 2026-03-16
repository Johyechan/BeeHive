using MyUtil.Interface;
using Tutorial.Event;
using UnityEngine;

namespace Tutorial.Handler
{
    // 작성자: 조혜찬
    // 튜토리얼 턴 종료 확인 핸들러
    public class TutorialTurnEndHandler : IEventHandler
    {
        public void Disable()
        {
            TutorialEvents.OnTurnEnd -= Function; // 구독 해제
        }

        public void Enable()
        {
            TutorialEvents.OnTurnEnd += Function;
        }

        public void Function()
        {
            TutorialManager.Instance.TurnEnd = true; // 턴 종료 할당
        }
    }
}
// 마지막 작성 일자: 2026.03.16