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
            TutorialManager.Instance.SetTutorialPanel(false);
            TutorialManager.Instance.ChangeTutorialState(TutorialState.Turn1_Player); // 첫 번째 턴(플레이어 턴) 상태로 전환
        }
    }
}
// 마지막 작성 일자: 2026.03.12