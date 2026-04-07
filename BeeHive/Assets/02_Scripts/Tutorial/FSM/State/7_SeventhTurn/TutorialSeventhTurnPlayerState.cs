using InGame.MyEnum;
using InGame.MyEvent;
using InGame.MyManager.Local;
using InGame.MyObject.Piece.ObjectPieces;
using MyUtil.Interface;
using Tutorial.MyEnum;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Tutorial.FSM.State.Seventh
{
    // 작성자: 조혜찬
    // 일곱 번째 턴(AI 턴) 상태
    public class TutorialSeventhTurnPlayerState : IState
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
                        string draw = LocalizationSettings.StringDatabase.GetLocalizedString(
                            "Tutorial",
                            "Tutorial_Draw"
                        );
                        TutorialManager.Instance.SetTutorialPanel(true, draw, TutorialManager.Instance.ButtonClick, 0.1f, 0.008f, new Vector4(0.055f, 0.094f), new Vector4(0.7f, 0.7f));
                        break;
                    case TurnType.MainTurn: // 메인 턴이라면
                        string tankAttackCastle = LocalizationSettings.StringDatabase.GetLocalizedString(
                            "Tutorial",
                            "Tutorial_TankAttackCastle"
                        );
                        TutorialManager.Instance.SetTutorialPanel(true, tankAttackCastle, TutorialManager.Instance.TargetClick, 0.08f, 0.008f, new Vector4(0.543f, 0.684f), new Vector4(0.3f, 0.3f), new Vector2(0, 450f));
                        break;
                    case TurnType.TurnEnd: // 턴 종료 턴이라면
                        TutorialManager.Instance.ChangeTutorialState(TutorialState.Turn7_AI); // 일곱 번째 턴(AI 턴) 상태로 변경
                        break;
                }
            }
        }
    }
}
// 마지막 작성 일자: 2026.04.07


