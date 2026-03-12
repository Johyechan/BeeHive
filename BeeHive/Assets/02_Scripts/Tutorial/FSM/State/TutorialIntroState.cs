using MyUtil.Interface;
using Tutorial.Event;
using Tutorial.MyEnum;
using UnityEngine;

namespace Tutorial.FSM.State
{
    // 작성자: 조혜찬
    // 튜토리얼 시작 상태 클래스
    public class TutorialIntroState : IState
    {
        public void Enter()
        {
            TutorialManager.Instance.ChangeTutorialState(TutorialState.Intro); // 현재 튜토리얼 상태를 인트로 상태로 변경
        }

        public void Update()
        {
            if(Input.GetKeyDown(KeyCode.Return)) // 엔터 키 클릭 시
            {
                TutorialEvents.OnIntroEnd?.Invoke(); // 인트로 종료 이벤트 호출
            }
        }

        public void Exit()
        {
            
        }
    }
}
// 마지막 작성 일자: 2026.03.12