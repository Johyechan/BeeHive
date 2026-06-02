using InGame.MyEnum;
using InGame.MyManager.Local;
using MyUtil.Interface;
using Tutorial.Event;
using Tutorial.MyEnum;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Tutorial.FSM.State
{
    // 작성자: 조혜찬
    // 튜토리얼 시작 상태 클래스
    public class TutorialIntroState : IState
    {
        private int _count; // 다음 설명을 보여주기 위한 카운팅에 사용할 변수

        public void Enter()
        {
            TutorialManager.Instance.InputOn = true;
            TutorialManager.Instance.IsInputDelayOver = false;
            _count = 0;

            string howToQuit = LocalizationSettings.StringDatabase.GetLocalizedString(
                        "Tutorial",
                        "Tutorial_HowToQuit"
                    );
            TutorialManager.Instance.SetTutorialPanel(true, howToQuit, TutorialManager.Instance.EnterClick);
        }

        public void Update()
        {
            if(TutorialManager.Instance.IsInputDelayOver) // 인풋 딜레이가 지나고 인풋이 들어왔다면
            {
                _count++; // 카운팅
                TutorialManager.Instance.IsInputDelayOver = false;
            }

            switch(_count) // 카운팅 된 수가
            {
                case 1:
                    string tutorialStart = LocalizationSettings.StringDatabase.GetLocalizedString(
                        "Tutorial",
                        "Tutorial_Start"
                    );

                    TutorialManager.Instance.SetTutorialPanel(true, tutorialStart, TutorialManager.Instance.EnterClick, 0.08f, 0.008f, new Vector4(0.5f, 0.305f), new Vector4(1.2f, 1.2f), new Vector2(0, 110f));
                    break;
                case 2:
                    string yourHP = LocalizationSettings.StringDatabase.GetLocalizedString(
                        "Tutorial",
                        "Tutorial_YourHPAndGold"
                    );
                    TutorialManager.Instance.SetTutorialPanel(true, yourHP, TutorialManager.Instance.EnterClick, 0.07f, 0.008f, new Vector4(0.42f, 0.958f), new Vector4(2f, 0.6f));
                    break;
                case 3:
                    string opponentCastle = LocalizationSettings.StringDatabase.GetLocalizedString(
                        "Tutorial",
                        "Tutorial_OpponentCastle"
                    );
                    TutorialManager.Instance.SetTutorialPanel(true, opponentCastle, TutorialManager.Instance.EnterClick, 0.07f, 0.008f, new Vector4(0.5f, 0.78f), new Vector4(1f, 1f));
                    break;
                case 4:
                    string opponentHP = LocalizationSettings.StringDatabase.GetLocalizedString(
                        "Tutorial",
                        "Tutorial_OpponentHPAndGold"
                    );
                    TutorialManager.Instance.SetTutorialPanel(true, opponentHP, TutorialManager.Instance.EnterClick, 0.07f, 0.008f, new Vector4(0.58f, 0.958f), new Vector4(2f, 0.6f));
                    break;
                case 5:
                    string viewingCastleIsYourTeam = LocalizationSettings.StringDatabase.GetLocalizedString(
                        "Tutorial",
                        "Tutorial_viewingCastleIsYourTeam"
                    );
                    TutorialManager.Instance.SetTutorialPanel(true, viewingCastleIsYourTeam, TutorialManager.Instance.EnterClick);
                    break;
                case 6:
                    string defeatOpponet = LocalizationSettings.StringDatabase.GetLocalizedString(
                        "Tutorial",
                        "Tutorial_DefeatOpponent"
                    );
                    TutorialManager.Instance.SetTutorialPanel(true, defeatOpponet, TutorialManager.Instance.EnterClick);
                    break;
                case 7:
                    TutorialEvents.OnIntroEnd?.Invoke(); // 인트로 종료 이벤트 호출
                    break;
            }
        }

        public void Exit()
        {
            TutorialManager.Instance.InputOn = false;
            _ = InGameContext.Current.Data.TurnManager.TurnChange(TurnType.ChangeTeam); // 턴 시작
        }
    }
}
// 마지막 작성 일자: 2026.06.02