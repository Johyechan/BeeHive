using InGame.MyEnum;
using InGame.MyManager.Local;
using InGame.MyUI;
using MyUtil.Interface;
using Tutorial.MyEnum;
using UnityEngine;

namespace Tutorial.FSM.State.Third
{
    // 작성자: 조혜찬
    // 세 번째 턴(플레이어 턴) 상태 클래스
    public class TutorialThirdTurnPlayerState : IState
    {
        public TutorialThirdTurnPlayerState()
        {

        }

        public void Enter()
        {
            
        }

        public void Exit()
        {
            _ = InGameContext.Current.Data.TurnManager.NextTurn(TurnType.ChangeTeam); // 팀 변경 턴(다음 팀 턴 - 튜토리얼에선 두 번째 플레이어 턴)으로 변경
        }

        public void Update()
        {
            if(TutorialManager.Instance.TurnEnd) // 현재 턴이 끝났다면
            {
                TutorialManager.Instance.TurnEnd = false; // 턴 종료 초기화

                switch(InGameContext.Current.Data.TurnManager.CurrentTurnType) // 현재 턴이
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
                        TutorialManager.Instance.SetTutorialPanel(true, "도로를 생성합시다.", "버튼 클릭", 0.1f, 0.008f, new Vector4(0.356f, 0.123f), new Vector4(0.5f, 0.3f));
                        break;
                    case TurnType.TurnEnd: // 턴 종료 턴이라면
                        TutorialManager.Instance.ChangeTutorialState(TutorialState.Turn3_AI); // 세 번째 턴(AI 턴) 상태로 변경
                        break;
                }
            }
        }
    }
}
// 마지막 작성 일자: 2026.03.24