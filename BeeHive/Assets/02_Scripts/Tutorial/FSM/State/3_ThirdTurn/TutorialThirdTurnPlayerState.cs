using InGame.MyEnum;
using InGame.MyManager.Local;
using InGame.MyUI;
using MyUtil.Interface;
using UnityEngine;

namespace Tutorial.FSM.State.Third
{
    // 작성자: 조혜찬
    // 세 번째 턴(플레이어 턴) 상태 클래스
    public class TutorialThirdTurnPlayerState : IState
    {
        private ConfirmUI _confirmUI; // 확인 UI

        public TutorialThirdTurnPlayerState(ConfirmUI confirmUI)
        {
            _confirmUI = confirmUI;
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
                        TutorialManager.Instance.SetTutorialPanel(true, "다시 한 번 더 카드를 뽑아봅시다.", "버튼 클릭", 0.1f, 0.008f, new Vector4(0.055f, 0.094f), new Vector4(0.75f, 0.7f));
                        break;
                    case TurnType.MainTurn: // 메인 턴이라면
                        TutorialManager.Instance.SetTutorialPanel(true, "보병을 이동합시다.", "대상 클릭", 0.08f, 0.008f, new Vector4(0.479f, 0.635f), new Vector4(0.3f, 0.3f), new Vector2(0, 400f));
                        break;
                    case TurnType.TurnEnd: // 턴 종료 턴이라면
                        TutorialManager.Instance.ChangeTutorialState(MyEnum.TutorialState.Turn3_AI); // 세 번째 턴(AI 턴) 상태로 변경
                        break;
                }
            }
        }
    }
}
// 마지막 작성 일자: 2026.03.19