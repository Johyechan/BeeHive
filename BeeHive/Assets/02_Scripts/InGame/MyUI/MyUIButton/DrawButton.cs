using InGame.MyInput;
using InGame.MyInput.Struct;
using InGame.MyObject;
using InGame.MyUI.MyUIInterface;
using MyUtil.GameMode;
using Tutorial;
using Tutorial.MyEnum;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Settings;

namespace InGame.MyUI.MyUIButton
{
    // 작성자: 조혜찬
    // 드로우 버튼
    public class DrawButton : MonoBehaviour, IUIClick
    {
        [SerializeField] private Deck _deck; // 드로우에 필요한 객체를 가지는 클래스

        [SerializeField] private int _drawMillisecondDelay; // 드로우 딜레이 시간

        private InputDrawHandlerData _inputDrawHandlerData; // 드로우 핸들러에 필요한 핸들러들을 가지는 구조체

        private InputDrawHandler _drawHandler; // 드로우를 위한 핸들러

        private void Awake()
        {
            _inputDrawHandlerData = new InputDrawHandlerData()
            {
                returnHandler = new InputDrawReturnHandler(_deck),
                socketEventHandler = new InputDrawSocketEventHandler(),
                functionHandler = new InputDrawFunctionHandler(),
            };

            _drawHandler = new InputDrawHandler(_deck, _drawMillisecondDelay, _inputDrawHandlerData);
        }

        public void OnUIClick()
        {
            _drawHandler.DrawAction();

            if(GameModeManager.Instance.CurrentGameMode.IsTutorial()) // 튜토리얼 일 경우
            {
                switch(TutorialManager.Instance.CurrentTutorialState) // 튜토리얼 상태가
                {
                    case TutorialState.Turn6_Player: // 여섯 번째 턴(플레이어 턴) 일때
                        string checkCard = LocalizationSettings.StringDatabase.GetLocalizedString(
                            "Tutorial",
                            "Tutorial_CheckCard"
                        );
                        string rightClick = LocalizationSettings.StringDatabase.GetLocalizedString(
                            "Tutorial",
                            "Tutorial_RightClick"
                        );
                        TutorialManager.Instance.SetTutorialPanel(true, checkCard, rightClick, 0.1f, 0.008f, new Vector4(0.5f, 0.15f), new Vector4(1.2f, 1.2f));
                        break;
                    default:
                        string checkDrawCard = LocalizationSettings.StringDatabase.GetLocalizedString(
                            "Tutorial",
                            "Tutorial_CheckDrawCard"
                        );
                        TutorialManager.Instance.SetTutorialPanel(true, checkDrawCard, TutorialManager.Instance.ButtonClick, 0.1f, 0.008f, new Vector4(0.128f, 0.094f), new Vector4(0.7f, 0.7f));
                        break;
                }
            }
            EventSystem.current.SetSelectedGameObject(null); // 선택한 객체 초기화
        }
    }
}
// 마지막 작성 일자: 2026.04.16