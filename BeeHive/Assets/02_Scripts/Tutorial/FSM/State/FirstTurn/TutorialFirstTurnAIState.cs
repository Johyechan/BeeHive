using InGame.MyEnum;
using InGame.MyManager.Local;
using MyUtil.Interface;
using UnityEngine;

namespace Tutorial.FSM.State.First
{
    // 작성자: 조혜찬
    // 첫 번째 턴(AI 턴) 상태 클래스
    public class TutorialFirstTurnAIState : IState
    {
        public void Enter()
        {

        }

        public void Exit()
        {

        }

        public void Update()
        {
            if(TutorialManager.Instance.TurnEnd) // 현재 턴이 끝났을 때
            {
                TutorialManager.Instance.TurnEnd = false; // 초기화
                switch(InGameContext.Current.Data.TurnManager.CurrentTurnType) // 현재 턴이
                {
                    case TurnType.ChangeTeam: // 팀 변경 턴이라면
                        _ = InGameContext.Current.Data.TurnManager.NextTurn(TurnType.MakeTurn); // 생성 턴으로 턴 변경
                        break;
                    case TurnType.MakeTurn:
                        break;
                    case TurnType.DrawTurn:
                        break;
                    case TurnType.MainTurn:
                        break;
                    case TurnType.TurnEnd:
                        break;
                }
            }
        }
    }
}
// 마지막 작성 일자: 2026.03.17