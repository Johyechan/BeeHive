using DG.Tweening;
using InGame.MyEvent;
using InGame.MyManager;
using InGame.MyObject;
using InGame.MyUI.Card.Handler;
using InGame.MyUI.Card.Variable;
using InGame.MyUI.MyUIInterface;
using MyUtil.MyObjectPool;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace InGame.MyUI.Card
{
    // 작성자: 조혜찬
    // UI 카드 부모 클래스
    public class UICardBase : MonoBehaviour, IUIClick, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] protected UICardData _uiCardData;

        protected UICardVariable _uiCardVariable = new UICardVariable(); // 필요한 변수들을 가지는 클래스
        public UICardVariable UICardVariable { get => _uiCardVariable; } // 위 변수 프로퍼티

        private void Awake()
        {
            _uiCardVariable.rect = GetComponent<RectTransform>();
            _uiCardVariable.initializeHandler = new UICardInitializeHandler(_uiCardVariable);
            _uiCardVariable.showInformationHandler = new UICardShowInformationHandler(_uiCardVariable, _uiCardData.currentCardImage, _uiCardData.cardInformationText, _uiCardData.animationDuration);
            _uiCardVariable.clickedHandler = new UICardClickedHandler(this, _uiCardVariable, _uiCardData.animationDuration);
        }

        private void OnEnable()
        {
            UIEvents.OnShowUICardInformation += ShowInfomation;
        }

        private void Start()
        {
            _uiCardVariable.usedCardDeck = GameObject.Find("UsedDeck").GetComponent<UsedDeck>();
        }

        private void OnDisable()
        {
            UIEvents.OnShowUICardInformation -= ShowInfomation;
        }

        // 초기화 함수
        public async Task Init(CanvasGroup cardUsePanelCanvaseGroup, CanvasGroup cardInformationCanvasGroup)
        {
            await _uiCardVariable.initializeHandler.Init(cardUsePanelCanvaseGroup, cardInformationCanvasGroup);
        }

        // 마우스 커서가 UI 위에 올라와 있을 때
        public void OnPointerEnter(PointerEventData eventData)
        {
            _uiCardVariable.isMouseCursorOn = true;
            _uiCardVariable.originPos = _uiCardVariable.rect.anchoredPosition; // 현재 위치 저장
            _uiCardVariable.rect.DOMoveY(_uiCardData.animationValueY, _uiCardData.animationDuration); // y축으로 이동
        }

        // 마우스 커서가 UI 위에 올라와 있지 않을 때
        public void OnPointerExit(PointerEventData eventData)
        {
            _uiCardVariable.isMouseCursorOn = false;
            _uiCardVariable.rect.DOAnchorPos(_uiCardVariable.originPos, _uiCardData.animationDuration); // 기존 위치로 이동
        }

        // 카드 사용 함수
        public virtual bool UseCard()
        {
            // 카드 사용 후 사용된 카드들을 모아두는 덱으로 이동
            if(_uiCardVariable.usedCardDeck != null) // 사용한 카드들을 모아두는 덱을 찾았을 경우
            {
                _uiCardVariable.usedCardDeck.AddCardInToUsedDeck(_uiCardVariable.cardObj.transform); // 사용한 카드를 추가
                ObjectPoolManager.Instance.ReturnObject(_uiCardData.poolType, gameObject); // UI 풀에 반환

                return true;
            }

            return false;
        }

        // 카드 정보를 보여주는 함수
        private void ShowInfomation()
        {
            if (!_uiCardVariable.isMouseCursorOn) // 마우스 포인터가 현재 UI에 올려져 있지 않다면
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
// 마지막 작성 일자: 2025.10.20