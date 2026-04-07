using InGame.MyEnum;
using InGame.MyManager.Global;
using InGame.MyManager.Local;
using MyUtil.Interface;
using Tutorial.MyEnum;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Tutorial.FSM.State.Fourth
{
    // 작성자: 조혜찬
    // 네 번째 턴(플레이어 턴) 상태 클래스
    public class TutorialFourthTurnPlayerState : IState
    {
        private TurnType _currentTurnType; // 현재 턴

        public void Enter()
        {
            TutorialManager.Instance.IsInputDelayOver = false;
        }

        public void Exit()
        {
            TutorialManager.Instance.InputOn = false;
            _ = InGameContext.Current.Data.TurnManager.NextTurn(TurnType.ChangeTeam); // 팀 변경 턴(다음 팀 턴 - 튜토리얼에선 두 번째 플레이어 턴)으로 변경
        }

        public void Update()
        {
            if (TutorialManager.Instance.IsInputDelayOver) // 인풋이 딜레이 이후 들어오면
            {
                switch (_currentTurnType)
                {
                    case TurnType.MakeTurn:
                        _ = InGameContext.Current.Data.TurnManager.NextTurn(TurnType.DrawTurn); // 턴 종료 턴으로 턴 변경
                        TutorialManager.Instance.SetTutorialPanel(false);
                        _currentTurnType = TurnType.DrawTurn;
                        TutorialManager.Instance.InputOn = false;
                        break;
                }
                TutorialManager.Instance.IsInputDelayOver = false;
            }

            if (TutorialManager.Instance.TurnEnd) // 현재 턴이 종료되었을 때
            {
                TutorialManager.Instance.TurnEnd = false; // 초기화

                switch(InGameContext.Current.Data.TurnManager.CurrentTurnType) // 현재 턴이
                {
                    case TurnType.ChangeTeam:
                        _ = InGameContext.Current.Data.TurnManager.NextTurn(TurnType.MakeTurn);
                        break;
                    case TurnType.MakeTurn:
                        string minerSkillProblem = LocalizationSettings.StringDatabase.GetLocalizedString(
                            "Tutorial",
                            "Tutorial_MinerSkillProblem"
                        );
                        TutorialManager.Instance.SetTutorialPanel(true, minerSkillProblem, TutorialManager.Instance.EnterClick, 0.08f, 0.008f, new Vector4(0.37f, 0.515f), new Vector4(1.2f, 1.2f), new Vector2(0, 250f));
                        _currentTurnType = TurnType.MakeTurn;
                        TutorialManager.Instance.InputOn = true;
                        break;
                    case TurnType.DrawTurn:
                        string goToMainTurn = LocalizationSettings.StringDatabase.GetLocalizedString(
                            "Tutorial",
                            "Tutorial_GoToMainTurn"
                        );
                        TutorialManager.Instance.SetTutorialPanel(true, goToMainTurn, TutorialManager.Instance.ButtonClick, 0.18f, 0.008f, new Vector4(0.92f, 0.095f), new Vector4(0.66f, 0.4f));
                        break;
                    case TurnType.MainTurn:
                        string minerMoveInformation = LocalizationSettings.StringDatabase.GetLocalizedString(
                            "Tutorial",
                            "Tutorial_MinerMoveInformation"
                        );
                        TutorialManager.Instance.SetTutorialPanel(true, minerMoveInformation, TutorialManager.Instance.TargetClick, 0.08f, 0.008f, new Vector4(0.401f, 0.452f), new Vector4(0.3f, 0.3f), new Vector2(0, 110f));
                        break;
                    case TurnType.TurnEnd:
                        TutorialManager.Instance.ChangeTutorialState(TutorialState.Turn4_AI); // 네 번째 턴(AI 턴) 상태로 변경
                        break;
                }
            }
        }
    }
}
// 마지막 작성 일자: 2026.04.07