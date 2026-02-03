using DG.Tweening;
using InGame.MyManager;
using InGame.MyManager.Local;
using MyUtil.MyObjectPool;
using System.Threading.Tasks;
using UnityEngine;

namespace InGame.MyObject.Handler
{
    // 작성자: 조혜찬
    // 사용한 카드들 셔플 기능 핸들러
    public class UsedDeckShuffleHandler
    {
        private UsedDeckData _usedDeckData;

        private UsedCardInfo _usedCardInfo;

        private UsedDeckUIHandler _usedDeckUIHandler;

        private Transform _usedDeckTrans;

        public UsedDeckShuffleHandler(UsedDeckData usedDeckData, UsedCardInfo usedCardInfo, UsedDeckUIHandler usedDeckUIHandler, Transform usedDeckTrans)
        {
            _usedDeckData = usedDeckData;
            _usedCardInfo = usedCardInfo;
            _usedDeckUIHandler = usedDeckUIHandler;
            _usedDeckTrans = usedDeckTrans;
        }

        public async Task UsedDeckShuffle()
        {
            _usedDeckUIHandler.Init(); // 사용한 카드 UI 초기화
            _usedCardInfo.castleCardCount = 0;
            _usedCardInfo.droughtCardCount = 0;
            _usedCardInfo.goodHarvestCardCount = 0;
            _usedCardInfo.roadChangeCardCount = 0;
            _usedCardInfo.firePowerCardCount = 0;

            int childCount = _usedDeckTrans.childCount;
            for (int i = childCount - 1; i >= 0; i--)
            {
                GameObject cardObj = _usedDeckTrans.GetChild(i).gameObject;
                CardObject card = cardObj.GetComponent<CardObject>();
                ObjectPoolManager.Instance.ReturnObject(card.CardPoolType, cardObj);
            }

            for (int i = 0; i < _usedDeckData.shuffleCount; i++)
            {
                int index = Random.Range(4, 8); // 4 ~ 7 인덱스 카드 랜덤 선택
                RectTransform randomUICardRectTrans = _usedDeckData.uiCardDeck.GetChild(index).GetComponent<RectTransform>(); // 랜덤 선택 카드
                RectTransform frontUICardRectTrans = _usedDeckData.uiCardDeck.GetChild(_usedDeckData.uiCardDeck.childCount - 1).GetComponent<RectTransform>(); // 맨 위 카드

                float randomCardTargetY = frontUICardRectTrans.anchoredPosition.y; // 랜덤하게 선택된 카드의 y 목표 값을 맨 앞 카드의 y값으로 할당
                float frontCardTargetY = randomUICardRectTrans.anchoredPosition.y; // 맨 앞 카드의 y 목표 값을 랜덤하게 선택된 카드의 y값으로 할당

                await randomUICardRectTrans.DOAnchorPosY(_usedDeckData.shuffleMinYPos, _usedDeckData.cardShuffleDuration)
                    .OnComplete(() =>
                    {
                        randomUICardRectTrans.SetAsLastSibling();
                        frontUICardRectTrans.SetSiblingIndex(index);
                        frontUICardRectTrans.anchoredPosition = new Vector3(0, frontCardTargetY, 0);
                    }).AsyncWaitForCompletion();

                await randomUICardRectTrans.DOAnchorPosY(randomCardTargetY, _usedDeckData.cardShuffleDuration).AsyncWaitForCompletion();
            }

            await _usedDeckData.deckShuffleAnimationUI.DOFade(0, _usedDeckData.uiFadeDuration).AsyncWaitForCompletion(); // ui 객체 페이드 아웃
            _usedDeckData.deckShuffleAnimationUI.gameObject.SetActive(false);

            await Task.Yield(); // 한 프레임 대기를 통한 연출의 자연스러움 추가

            InGameContext.Current.Data.DeckManager.CompleteTcs(); // 덱 생성 완료
        }
    }
}
// 마지막 작성 일자: 2026.02.03