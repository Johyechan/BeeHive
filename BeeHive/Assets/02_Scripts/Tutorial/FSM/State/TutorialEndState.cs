using MyUtil.Interface;
using UnityEngine;

namespace Tutorial.FSM.State
{
    // 작성자: 조혜찬
    // 튜토리얼 종료 상태 클래스
    public class TutorialEndState : IState
    {
        public void Enter()
        {
            TutorialManager.Instance.SetTutorialPanel(true, "수고하셨습니다.", "버튼 클릭", 0.2f, 0.008f, new Vector4(0.5f, 0.247f), new Vector4(1f, 0.3f), new Vector2(0, 400f));
        }

        public void Exit()
        {
            
        }

        public void Update()
        {
            
        }
    }
}
// 마지막 작성 일자: 2026.03.19