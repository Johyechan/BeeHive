using InGame.MyEnum;
using InGame.MyManager.Local;
using MyUtil.Interface;
using UnityEngine;

namespace Tutorial.FSM.State.Fourth
{
    // 작성자: 조혜찬
    // 네 번째 턴(플레이어 턴) 상태 클래스
    public class TutorialFourthTurnPlayerState : IState
    {
        public void Enter()
        {
            
        }

        public void Exit()
        {
            
        }

        public void Update()
        {
            if(TutorialManager.Instance.TurnEnd) // 현재 턴이 종료되었을 때
            {
                TutorialManager.Instance.TurnEnd = false; // 초기화

                switch(InGameContext.Current.Data.TurnManager.CurrentTurnType) // 현재 턴이
                {
                    case TurnType.ChangeTeam:
                        _ = InGameContext.Current.Data.TurnManager.NextTurn(TurnType.MakeTurn);
                        break;
                    case TurnType.MakeTurn:
                        _ = InGameContext.Current.Data.TurnManager.NextTurn(TurnType.DrawTurn);
                        break;
                    case TurnType.DrawTurn:
                        TutorialManager.Instance.SetTutorialPanel(true, "다음 턴을 눌러 메인 턴을 진행합시다.", "버튼 클릭", 0.18f, 0.008f, new Vector4(0.92f, 0.095f), new Vector4(0.66f, 0.4f));
                        break;
                    case TurnType.MainTurn:
                        TutorialManager.Instance.SetTutorialPanel(true, "보병은 도로가 연결되어 있지 않아도 한 칸을 뛰어 이동이 가능합니다. \n 상대 성을 공격합시다.", "대상 클릭", 0.08f, 0.008f, new Vector4(0.48f, 0.733f), new Vector4(0.3f, 0.3f));
                        break;
                }
            }
        }
    }
}
// 마지막 작성 일자: 2026.03.19