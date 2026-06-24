using DG.Tweening;
using InGame.MyEnum;
using InGame.MyManager.Global;
using InGame.MyManager.Local;
using InGame.MyObject.Handler;
using InGame.MyObject.MyObjectInterface;
using InGame.MyUI.Card;
using MyUtil.GameMode;
using MyUtil.MyObjectPool;
using System.Threading.Tasks;
using UnityEngine;

namespace InGame.MyObject
{
    // 작성자: 조혜찬
    // 사용한 카드들을 모아두는 덱
    public class UsedDeck : MonoBehaviour, IClickObject
    {
        [SerializeField] private UsedDeckData _usedDeckData = new UsedDeckData();

        [SerializeField] private UsedDeckUIData _usedDeckUIData = new UsedDeckUIData();

        private UsedDeckHandlers _usedDeckHandlers = new UsedDeckHandlers(); // UsedDeck에 필요한 핸들러들을 모아둔 클래스

        private UsedCardInfo _usedCardInfo = new UsedCardInfo();
        public UsedCardInfo UsedCardInfo { get => _usedCardInfo; }

        private void Awake()
        {
            _usedDeckHandlers.usedDeckUIHandler = new UsedDeckUIHandler(_usedDeckUIData);
            _usedDeckHandlers.cardSettingHandler = new CardSettingHandler(_usedDeckData.animationDuration, _usedDeckData.yInterval);
            _usedDeckHandlers.usedDeckShuffleHandler = new UsedDeckShuffleHandler(_usedDeckData, _usedCardInfo, _usedDeckHandlers.usedDeckUIHandler, transform);

            _usedDeckHandlers.usedDeckUIHandler.Init();
        }

        // 사용한 카드들을 덱에 추가하는 함수
        public void AddCardInToUsedDeck(Transform addCardTrans, TeamType cardUsedTeam)
        {
            addCardTrans.SetParent(transform); // 추가한 카드의 부모를 자기 자신으로 할당
            int usedCardCount = transform.childCount; // 사용한 카드들을 모아두는 덱에 있는 카드 수

            CardObject cardObject = addCardTrans.GetComponent<CardObject>();

            switch(cardObject.CardUIPoolType)
            {
                case ObjectPoolType.CastleUpgradeUICard:
                    ++_usedCardInfo.castleCardCount;
                    _usedDeckUIData.castleUpgradeCardCount.text = $"x {_usedCardInfo.castleCardCount}";
                    break;
                case ObjectPoolType.DroughtUICard:
                    ++_usedCardInfo.droughtCardCount;
                    _usedDeckUIData.droughtCardCount.text = $"x {_usedCardInfo.droughtCardCount}";
                    break;
                case ObjectPoolType.GoodHarvestUICard:
                    ++_usedCardInfo.goodHarvestCardCount;
                    _usedDeckUIData.goodHarvestCardCount.text = $"x {_usedCardInfo.goodHarvestCardCount}";
                    break;
                case ObjectPoolType.RoadChangeUICard:
                    ++_usedCardInfo.roadChangeCardCount;
                    _usedDeckUIData.roadChangeCardCount.text = $"x {_usedCardInfo.roadChangeCardCount}";
                    break;
                case ObjectPoolType.FirePowerUICard:
                    ++_usedCardInfo.firePowerCardCount;
                    _usedDeckUIData.firePowerCardCount.text = $"x {_usedCardInfo.firePowerCardCount}";
                    break;
            }

            StartCoroutine(_usedDeckHandlers.cardSettingHandler.CardSettingCo(addCardTrans, cardObject.CardPoolType, usedCardCount, cardUsedTeam));
        }

        public async Task DeckShuffleAnimationFadeIn()
        {
            _usedDeckData.deckShuffleAnimationUI.gameObject.SetActive(true); // ui 객체 활성화
            await _usedDeckData.deckShuffleAnimationUI.DOFade(1, _usedDeckData.uiFadeDuration).AsyncWaitForCompletion(); // ui 객체 페이드 인
        }

        public async Task UsedDeckShuffle()
        {
            await _usedDeckHandlers.usedDeckShuffleHandler.UsedDeckShuffle();
        }

        public void ObjectClicked()
        {
            if (GameModeManager.Instance.CurrentGameMode.IsTutorial()) // 튜토리얼 일 경우 반환
                return;

            _usedDeckUIData.usedDeckCanvasGroup.gameObject.SetActive(true); // ui 활성화
            _usedDeckUIData.usedDeckCanvasGroup.DOFade(1, _usedDeckData.uiFadeDuration); // 페이드 인
        }
    }
}
// 마지막 작성 일자: 2026.06.24