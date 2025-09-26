using DG.Tweening;
using InGame.MyManager;
using InGame.MyUI.Card.Handler;
using InGame.MyUI.Card.Variable;
using InGame.MyUI.MyUIInterface;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace InGame.MyUI.Card
{
    // 작성자: 조혜찬
    // UI 카드 부모 클래스
    public abstract class UICardBase : MonoBehaviour, IUIClick, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] protected string _cardInformationText; // 카드 정보 텍스트

        [SerializeField] protected Image _currentCardImage; // 현재 카드 이미지

        protected bool _isMouseCursorOn;

        [SerializeField] private float _animationDuration; // 애니메이션 시간

        private UICardVariable _uiCardVariable = new UICardVariable(); // 필요한 변수들을 가지는 클래스

        private void Awake()
        {
            _uiCardVariable.initializeHandler = new UICardInitializeHandler(_uiCardVariable);
            _uiCardVariable.showInformationHandler = new UICardShowInformationHandler(_uiCardVariable, _currentCardImage, _cardInformationText, _animationDuration);
            _uiCardVariable.clickedHandler = new UICardClickedHandler(this, _uiCardVariable, _animationDuration);
        }

        private void OnEnable()
        {
            InputManager.Instance.rClickAction.performed += ShowInfomation;
        }

        private void OnDisable()
        {
            InputManager.Instance.rClickAction.performed -= ShowInfomation;
        }

        // 초기화 함수
        public async Task Init(CanvasGroup cardUsePanelCanvaseGroup, CanvasGroup cardInformationCanvasGroup)
        {
            await _uiCardVariable.initializeHandler.Init(cardUsePanelCanvaseGroup, cardInformationCanvasGroup);
        }

        // 마우스 커서가 UI 위에 올라와 있을 때
        public void OnPointerEnter(PointerEventData eventData)
        {
            _isMouseCursorOn = true;
        }

        // 마우스 커서가 UI 위에 올라와 있지 않을 때
        public void OnPointerExit(PointerEventData eventData)
        {
            _isMouseCursorOn = false;
        }

        // 카드 사용 함수
        public abstract void UseCard();

        // 카드 정보를 보여주는 함수
        private void ShowInfomation(InputAction.CallbackContext context)
        {
            if (!_isMouseCursorOn) // 마우스 포인터가 현재 UI에 올려져 있지 않다면
                return; // 반환

            _uiCardVariable.showInformationHandler.ShowInfomation();
        }

        // UI 클릭 함수
        public void OnUIClick()
        {
            _uiCardVariable.clickedHandler.Clicked();
        }
    }
}
// 마지막 작성 일자: 2025.09.26