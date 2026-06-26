using DG.Tweening;
using InGame.MyEnum;
using InGame.MyEvent;
using InGame.MyManager.Global;
using InGame.MyManager.Local;
using InGame.MyObject;
using InGame.MyUI.Card.Handler;
using InGame.MyUI.Card.Variable;
using InGame.MyUI.MyUIInterface;
using MyUtil;
using MyUtil.GameMode;
using MyUtil.MyObjectPool;
using System.Threading.Tasks;
using Tutorial;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Settings;

namespace InGame.MyUI.Card
{
    // 작성자: 조혜찬
    // UI 카드 부모 클래스
    public class UICardBase : MonoBehaviour, IUIClick, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] protected UICardData _uiCardData = new UICardData();
        public UICardData UICardData { get => _uiCardData; } // 외부에서 접근하기 위한 인스펙터 할당 변수들을 가지는 구조체 프로퍼티

        protected UICardVariable _uiCardVariable = new UICardVariable(); // 필요한 변수들을 가지는 클래스
        public UICardVariable UICardVariable { get => _uiCardVariable; } // 위 변수 프로퍼티

        private Tween _upDownAnimationTween; // 위아래 이동 트윈

        private void Awake()
        {
            _uiCardVariable.rect = GetComponent<RectTransform>();
            _uiCardVariable.initializeHandler = new UICardInitializeHandler(_uiCardVariable);
            _uiCardVariable.showInformationHandler = new UICardShowInformationHandler(_uiCardVariable, _uiCardData.animationDuration);
            _uiCardVariable.clickedHandler = new UICardClickedHandler(this, _uiCardVariable, _uiCardData.animationDuration);
        }

        private void OnEnable()
        {
            UIEvents.OnShowUICardInformation += ShowInfomation;

            if(InGameContext.Current != null)
            {
                _uiCardVariable.usedCardDeck = InGameContext.Current.Data.DeckManager.UsedDeckProp;
            }
        }

        private void OnDisable()
        {
            UIEvents.OnShowUICardInformation -= ShowInfomation;
        }

        // 초기화 함수
        public void Init(CanvasGroup cardUsePanelCanvaseGroup, CanvasGroup cardInformationCanvasGroup)
        {
            _uiCardVariable.initializeHandler.Init(cardUsePanelCanvaseGroup, cardInformationCanvasGroup);
        }

        // 마우스 커서가 UI 위에 올라와 있을 때
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!_uiCardVariable.isAnimationEnd) // 애니메이션이 종료 되지 않았다면
                return; // 반환

            _uiCardVariable.originIndex = _uiCardVariable.rect.GetSiblingIndex(); // 현재 순서 저장

            _uiCardVariable.isMouseCursorOn = true;
            _upDownAnimationTween?.Kill(); // 이전 트윈이 돌고 있다면 삭제

            _uiCardVariable.rect.SetAsLastSibling(); // 가장 위에 UI 그리기
            _uiCardVariable.isAnimationEnd = false; // 애니메이션 실행
            _upDownAnimationTween = _uiCardVariable.rect.DOAnchorPosY(_uiCardData.animationYValue, _uiCardData.animationDuration)
                .OnComplete(() => _uiCardVariable.isAnimationEnd = true); // y축으로 이동 + 닷트윈 종료 시 애니메이션 종료
        }

        // 마우스 커서가 UI 위에 올라와 있지 않을 때
        public void OnPointerExit(PointerEventData eventData)
        {
            _uiCardVariable.rect.SetSiblingIndex(_uiCardVariable.originIndex); // 기존 인덱스로 변경
            _uiCardVariable.isMouseCursorOn = false;
            _upDownAnimationTween?.Kill(); // 이전 트윈이 돌고 있다면 삭제

            _uiCardVariable.isAnimationEnd = false; // 애니메이션 실행
            _upDownAnimationTween = _uiCardVariable.rect.DOAnchorPosY(_uiCardVariable.originYPos, _uiCardData.animationDuration)
                .OnComplete(() => _uiCardVariable.isAnimationEnd = true); // 기존 위치로 이동 + 닷트윈 종료 시 애니메이션 종료
        }

        // 카드 사용 함수
        public virtual Task<bool> UseCard()
        {
            // 카드 사용 후 사용된 카드들을 모아두는 덱으로 이동
            if(_uiCardVariable.usedCardDeck != null) // 사용한 카드들을 모아두는 덱을 찾았을 경우
            {
                MainThreadDispatcher.Enqueue(() =>
                {
                    // 카드를 사용한 팀을 현재 게임 모드가 튜토리얼 일 경우 팀 1을 할당하고 튜토리얼이 아닐 경우 현재 팀을 할당
                    TeamType cardUseTeam = GameModeManager.Instance.CurrentGameMode.IsTutorial() == true ? TeamType.Team1 : TeamManager.Instance.CurrentTeamType;
                    // 사용한 카드를 추가
                    _uiCardVariable.usedCardDeck.AddCardInToUsedDeck(_uiCardVariable.cardObj.transform, cardUseTeam); 
                    ObjectPoolManager.Instance.ReturnObject(_uiCardData.poolType, gameObject, true, false);
                });

                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        // 카드 정보를 보여주는 함수
        private void ShowInfomation()
        {
            if (!_uiCardVariable.isMouseCursorOn) // 마우스 포인터가 현재 UI에 올려져 있지 않다면
                return; // 반환

            if(GameModeManager.Instance.CurrentGameMode.IsTutorial()) // 튜토리얼 일 경우
            {
                string itsFirePowerCard = LocalizationSettings.StringDatabase.GetLocalizedString(
                    "Tutorial",
                    "Tutorial_ItsFirePowerCard"
                );
                TutorialManager.Instance.SetTutorialPanel(true, itsFirePowerCard, TutorialManager.Instance.ButtonClick, 0.1f, 0.008f, new Vector4(0.5f, 0.2795f), new Vector4(1.3f, 0.6f), new Vector2(0, 350f));
            }

            InGameContext.Current.Data.CardManager.CurrentUICard = this; // 현재 ui 카드 할당

            _uiCardVariable.showInformationHandler.ShowInfomation();
        }

        // UI 클릭 함수
        public void OnUIClick()
        {
            if(GameModeManager.Instance.CurrentGameMode.IsTutorial()) // 튜토리얼 일 경우
            {
                EventSystem.current.SetSelectedGameObject(null); // 선택한 객체 초기화
                return; // 반환
            }

            if(_uiCardData.poolType == ObjectPoolType.FirePowerUICard) // 화력 카드일 경우
            {
                string canNotUseFirePowerCard = LocalizationSettings.StringDatabase.GetLocalizedString(
                    "Game",
                    "Game_UI_CanNotUseFirePowerCard"
                );

                UIManager.Instance.WarningUIMake(canNotUseFirePowerCard); // 직접 사용 불가 패널 띄우기
            }
            else // 화력 카드가 아닐 경우
            {
                if(InGameContext.Current.Data.TurnManager.CurrentTeamType != TeamManager.Instance.CurrentTeamType) // 자신의 턴이 아닐 경우
                {
                    string notYourTurn = LocalizationSettings.StringDatabase.GetLocalizedString(
                        "Game",
                        "Game_UI_NotYourTurn"
                    );

                    UIManager.Instance.WarningUIMake(notYourTurn); // 직접 사용 불가 패널 띄우기
                    return; // 반환
                }
                _uiCardVariable.clickedHandler.ShowAskPanel();
            }

            EventSystem.current.SetSelectedGameObject(null); // 선택한 객체 초기화
        }
    }
}
// 마지막 작성 일자: 2026.06.26