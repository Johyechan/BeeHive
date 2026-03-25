using InGame.MyEnum;
using InGame.MyManager.Local;
using MyUtil.Interface;
using Tutorial.MyEnum;
using UnityEngine;

namespace Tutorial.FSM.State.Eighth
{
    // 작성자: 조혜찬
    // 여덟 번째 턴(AI 턴) 상태
    public class TutorialEighthTurnPlayerState : IState
    {
        public void Enter()
        {
            
        }

        public void Exit()
        {
            
        }

        public void Update()
        {
            if (TutorialManager.Instance.TurnEnd) // 현재 턴이 끝났다면
            {
                TutorialManager.Instance.TurnEnd = false; // 턴 종료 초기화

                switch (InGameContext.Current.Data.TurnManager.CurrentTurnType) // 현재 턴이
                {
                    case TurnType.ChangeTeam: // 팀 변경 턴이라면
                        _ = InGameContext.Current.Data.TurnManager.NextTurn(TurnType.MakeTurn); // 생산 턴으로 턴 변경
                        break;
                    case TurnType.MakeTurn: // 생산 턴이라면
                        _ = InGameContext.Current.Data.TurnManager.NextTurn(TurnType.DrawTurn); // 드로우 턴으로 턴 변경
                        break;
                    case TurnType.DrawTurn: // 드로우 턴이라면
                        TutorialManager.Instance.SetTutorialPanel(true, "다음 턴을 눌러 메인 턴을 진행합시다.", "버튼 클릭", 0.18f, 0.008f, new Vector4(0.92f, 0.095f), new Vector4(0.66f, 0.4f));
                        break;
                    case TurnType.MainTurn: // 메인 턴이라면
                        TutorialManager.Instance.SetTutorialPanel(true, "전차를 생성합시다.", "버튼 클릭", 0.1f, 0.008f, new Vector4(0.196f, 0.095f), new Vector4(0.7f, 0.7f));
                        break;
                }
            }
        }
    }
}
// 마지막 작성 일자: 2026.03.25


