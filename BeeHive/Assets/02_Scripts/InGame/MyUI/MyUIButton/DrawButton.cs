using InGame.MyInput;
using InGame.MyInput.Struct;
using InGame.MyObject;
using InGame.MyUI.MyUIInterface;
using MyUtil.GameMode;
using Tutorial;
using UnityEngine;
using UnityEngine.EventSystems;

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
                TutorialManager.Instance.SetTutorialPanel(true, "뽑은 카드를 확인 합시다.", "버튼 클릭", 0.1f, 0.008f, new Vector4(0.128f, 0.094f), new Vector4(0.65f, 0.65f));
            }
        }
    }
}
// 마지막 작성 일자: 2026.03.18