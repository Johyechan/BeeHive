using InGame.MyManager.Global;
using InGame.MyObject;
using InGame.MySystem.Game;
using InGame.MyUI.Card;
using MyUtil.MyObjectPool;
using System;
using UnityEngine;

namespace InGame.MyManager.Local.MyCard
{
    // 작성자: 조혜찬
    // 드로우를 관리하는 클래스
    public class DrawManager : MonoBehaviour
    {
        public bool CanDraw { get; set; } // 드로우 가능 여부 프로퍼티

        private CardSetHandle _cardSetHandle; // 카드 세팅 핸들
        public CardSetHandle CardSetHandle { get => _cardSetHandle; } // 위 변수 프로퍼티

        [SerializeField] private Deck _deck; // 덱 클래스

        [SerializeField] private Transform _usedDeckTrans; // 사용한 카드 덱 Transform

        [SerializeField] private CanvasGroup _cardUsePanelCanvasGroup; // 카드 사용 캔버스 그룹
        [SerializeField] private CanvasGroup _cardInformationPanelCanvasGroup; // 카드 정보 캔버스 그룹

        private void Awake()
        {
            CardParents cardParents = new CardParents() // 각 플레이어 카드 객체 부모 구조체
            {
                player1Parent = _deck.player1CardsParent, // 플레이어1 카드 객체 부모
                player2Parent = _deck.player2CardsParent, // 플레이어2 카드 객체 부모
                player3Parent = _deck.player3CardsParent, // 플레이어3 카드 객체 부모
            };
            _cardSetHandle = new CardSetHandle(_deck.deckTransform, cardParents); // 카드 세팅 클래스 생성
        }

        public void DrawCard(Transform deckParent, Transform playerCardsParent, RectTransform playerUICardsParent, bool includeUI = true)
        {
            Transform currentDrawCardTrans = deckParent.GetChild(0); // 맨 위에 있는 카드 할당
            CardObject currentDrawCard = currentDrawCardTrans.GetComponent<CardObject>();

            if (currentDrawCard.CardUIPoolType == ObjectPoolType.FirePowerUICard) // 드로우한 카드가 화력 카드일 경우
            {
                InGameContext.Current.Data.CardManager.HaveFirePowerCard = true; // 화력 카드를 가지고 있다고 할당
            }

            currentDrawCardTrans.SetParent(playerCardsParent);// 덱에 있는 카드를 플레이어의 카드로 변경

            if (includeUI) // UI도 생성해야 할 경우
            {
                GameObject uiCard = ObjectPoolManager.Instance.GetObject(currentDrawCard.CardUIPoolType); // UI 카드를 추가하여 플레이어 UI 카드에 추가
                uiCard.GetComponent<RectTransform>().SetParent(playerUICardsParent);
                UICardBase uiCardBase = uiCard.GetComponent<UICardBase>();
                uiCardBase.UICardVariable.cardObj = currentDrawCard.gameObject; // UI 카드에 현재 카드 객체 할당

                uiCardBase.Init(_cardUsePanelCanvasGroup, _cardInformationPanelCanvasGroup);
            }

            if(_deck.transform.childCount <= 0 && _usedDeckTrans.childCount > 0) // 덱이 비어있고 사용한 카드 덱에 카드가 있을 경우
            {
                if(InGameContext.Current.Data.TurnManager.CurrentTeamType == TeamManager.Instance.CurrentTeamType) // 현재 턴의 팀과 클라이언트 팀이 같을 경우
                {
                    InGameContext.Current.Data.DeckManager.IsEmpty = false;
                    InGameContext.Current.Data.DeckManager.ReMakeDeck();
                }
            }
        }
    }
}
// 마지막 작성 일자: 2026.03.03