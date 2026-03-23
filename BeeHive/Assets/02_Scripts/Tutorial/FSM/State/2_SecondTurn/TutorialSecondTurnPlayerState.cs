using InGame.MyEnum;
using InGame.MyManager.Local;
using MyUtil.Interface;
using Tutorial.MyEnum;
using UnityEngine;

namespace Tutorial.FSM.State.Second
{
    // 작성자: 조혜찬
    // 두 번째 턴(플레이어 턴) 상태 클래스
    public class TutorialSecondTurnPlayerState : IState
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
            if(TutorialManager.Instance.TurnEnd) // 현재 턴이 종료 되었을 때
            {
                TutorialManager.Instance.TurnEnd = false; // 턴 종료 여부 초기화

                switch(InGameContext.Current.Data.TurnManager.CurrentTurnType) // 현재 턴이
                {
                    case TurnType.ChangeTeam: // 팀 변경 턴일 때
                        _ = InGameContext.Current.Data.TurnManager.NextTurn(TurnType.MakeTurn); // 생산 턴으로 턴 넘기기
                        break;
                    case TurnType.MakeTurn: // 생산 턴일 때
                        _ = InGameContext.Current.Data.TurnManager.NextTurn(TurnType.DrawTurn); // 드로우 턴으로 턴 넘기기
                        break;
                    case TurnType.DrawTurn: // 드로우 턴일 때
                        TutorialManager.Instance.SetTutorialPanel(true, "이번에는 카드를 뽑아봅시다.", "버튼 클릭", 0.1f, 0.008f, new Vector4(0.055f, 0.094f), new Vector4(0.75f, 0.7f));
                        break;
                    case TurnType.MainTurn: // 메인 턴일 때
                        TutorialManager.Instance.SetTutorialPanel(true, "이번에는 전차를 생성해봅시다.", "버튼 클릭", 0.1f, 0.008f, new Vector4(0.196f, 0.095f), new Vector4(0.7f, 0.7f));
                        break;
                    case TurnType.TurnEnd: // 턴 종료 턴일 때
                        TutorialManager.Instance.ChangeTutorialState(TutorialState.Turn2_AI); // 첫 번째 턴(AI 턴)으로 튜토리얼 상태 변경
                        break;
                }
            }
        }
    }
}
// 마지막 작성 일자: 2026.03.18