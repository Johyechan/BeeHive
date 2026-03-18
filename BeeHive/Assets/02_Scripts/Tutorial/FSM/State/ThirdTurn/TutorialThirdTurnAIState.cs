using InGame.MyEnum;
using InGame.MyManager.Local;
using MyUtil.Interface;
using UnityEngine;

namespace Tutorial.FSM.State.Third
{
    // 작성자: 조혜찬
    // 세 번째 턴(AI 턴) 상태 클래스
    public class TutorialThirdTurnAIState : IState
    {
        public void Enter()
        {
            
        }

        public void Exit()
        {
            _ = InGameContext.Current.Data.TurnManager.NextTurn(TurnType.ChangeTeam); // 팀 변경 턴(다음 팀 턴 - 튜토리얼에선 두 번째 플레이어 턴)으로 변경
        }

        public void Update()
        {
            
        }
    }
}
// 마지막 작성 일자: 2026.03.12