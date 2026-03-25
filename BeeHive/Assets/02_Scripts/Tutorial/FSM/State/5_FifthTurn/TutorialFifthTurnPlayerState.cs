using InGame.MyEnum;
using InGame.MyManager.Local;
using MyUtil.Interface;
using Tutorial.MyEnum;
using UnityEngine;

namespace Tutorial.FSM.State.Fifth
{
    // 작성자: 조혜찬
    // 다섯 번째 턴(플레이어 턴) 상태
    public class TutorialFifthTurnPlayerState : IState
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
            if(TutorialManager.Instance.TurnEnd) // 턴이 종료 되었을 때
            {
                TutorialManager.Instance.TurnEnd = false; // 초기화
                switch(InGameContext.Current.Data.TurnManager.CurrentTurnType)
                {
                    case TurnType.ChangeTeam: // 팀 변경 턴일 경우
                        _ = InGameContext.Current.Data.TurnManager.NextTurn(TurnType.MakeTurn);
                        break;
                    case TurnType.MakeTurn: // 생산 턴일 경우
                        _ = InGameContext.Current.Data.TurnManager.NextTurn(TurnType.DrawTurn);
                        break;
                    case TurnType.DrawTurn: // 드로우 턴일 경우
                        TutorialManager.Instance.SetTutorialPanel(true, "카드를 뽑아봅시다", "버튼 클릭", 0.1f, 0.008f, new Vector4(0.055f, 0.094f), new Vector4(0.7f, 0.7f));
                        break;
                    case TurnType.MainTurn: // 메인 턴일 경우
                        TutorialManager.Instance.SetTutorialPanel(true, "상대 전차를 파괴해봅시다.", "대상 클릭", 0.08f, 0.008f, new Vector4(0.401f, 0.452f), new Vector4(0.3f, 0.3f), new Vector2(0, 110f));
                        break;
                    case TurnType.TurnEnd: // 턴 종료일 경우
                        TutorialManager.Instance.ChangeTutorialState(TutorialState.Turn5_AI); // 다섯 번째 턴(AI 턴) 상태로 변경
                        break;
                }
            }
        }
    }
}
// 마지막 작성 일자: 2026.03.25

