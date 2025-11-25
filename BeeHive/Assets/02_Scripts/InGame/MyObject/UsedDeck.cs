using DG.Tweening;
using InGame.MyEnum;
using InGame.MyManager;
using InGame.MyManager.MyCard;
using InGame.MyUI.Card;
using MyUtil;
using MyUtil.MyEvent;
using MyUtil.MyObjectPool;
using System.Collections;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

namespace InGame.MyObject
{
    // 작성자: 조혜찬
    // 사용한 카드들을 모아두는 덱
    public class UsedDeck : MonoBehaviour
    {
        [SerializeField] private float _animationDuration; // 애니메이션 지속시간
        [SerializeField] private float _cardShuffleDuration; // 셔플 시간
        [SerializeField] private float _uiFadeDuration; // ui 활성화 시간
        [SerializeField] private float _yInterval; // y축 간격
        [SerializeField] private float _shuffleMinYPos; // 셔플 y 위치

        [SerializeField] private int _shuffleCount; // 셔플 횟수

        [SerializeField] private CanvasGroup _deckShuffleAnimationUI; // 덱 셔플 애니메이션 UI

        [SerializeField] private RectTransform _uiCardDeck; // ui 덱

        [SerializeField] private Deck _deck; // 덱

        private UsedDeckData _usedDeckData = new UsedDeckData();
        public UsedDeckData UsedDeckData { get => _usedDeckData; }

        // 사용한 카드들을 덱에 추가하는 함수
        public void AddCardInToUsedDeck(Transform addCardTrans)
        {
            addCardTrans.SetParent(transform); // 추가한 카드의 부모를 자기 자신으로 할당
            int usedCardCount = transform.childCount; // 사용한 카드들을 모아두는 덱에 있는 카드 수

            CardObject cardObject = addCardTrans.GetComponent<CardObject>();

            switch(cardObject.CardUIPoolType)
            {
                case ObjectPoolType.CastleUpgradeUICard:
                    _usedDeckData.castleCardCount++;
                    break;
                case ObjectPoolType.DroughtUICard:
                    _usedDeckData.droughtCardCount++;
                    break;
                case ObjectPoolType.GoodHarvestUICard:
                    _usedDeckData.goodHarvestCardCount++;
                    break;
                case ObjectPoolType.RoadChangeUICard:
                    _usedDeckData.roadChangeCardCount++;
                    break;
                case ObjectPoolType.FirePowerUICard:
                    _usedDeckData.firePowerCardCount++;
                    break;
            }

            StartCoroutine(CardSettingCo(addCardTrans, usedCardCount));
        }

        // 카드 세팅 코루틴
        private IEnumerator CardSettingCo(Transform addCardTrans, int usedCardCount)
        {
            bool tweenEnd = false; // 트윈 종료 여부를 판단하는 변수
            DOTween.Sequence()
                .AppendInterval(_animationDuration) // 대기
                .Append(addCardTrans.DOLocalMove(new Vector3(0, _yInterval * usedCardCount, 0), _animationDuration))
                .OnComplete(() => tweenEnd = true); // 사용한 카드 위치 이동

            yield return new WaitUntil(() => tweenEnd); // 트윈이 종료될 때까지 대기

            DrawEventSystem.OnCardUISet?.Invoke();// 카드 UI 재세팅
            switch (TurnManager.Instance.CurrentTeamType) // 현재 팀에 따라 카드 재세팅
            {
                case TeamType.Team1:
                    yield return new WaitUntil(() => DrawEventSystem.OnCardObjectSet.Invoke(DeckManager.Instance.DeckProp.player1CardsParent));
                    break;
                case TeamType.Team2:
                    yield return new WaitUntil(() => DrawEventSystem.OnCardObjectSet.Invoke(DeckManager.Instance.DeckProp.player2CardsParent));
                    break;
            }

            if (DeckManager.Instance.IsEmpty) // 덱이 비어 있다면
            {
                DeckManager.Instance.IsEmpty = false; // 덱이 비어 있지 않은 상태로 할당
                DeckManager.Instance.ReMakeDeck(); // 덱 다시 만들기
            }
        }

        public async Task DeckShuffleAnimationFadeIn()
        {
            _deckShuffleAnimationUI.gameObject.SetActive(true); // ui 객체 활성화
            await _deckShuffleAnimationUI.DOFade(1, _uiFadeDuration).AsyncWaitForCompletion(); // ui 객체 페이드 인
        }

        public async Task DeckShuffle()
        {
            _usedDeckData.castleCardCount = 0;
            _usedDeckData.droughtCardCount = 0;
            _usedDeckData.goodHarvestCardCount = 0;
            _usedDeckData.roadChangeCardCount = 0;
            _usedDeckData.firePowerCardCount = 0;
            
            int childCount = transform.childCount;
            for(int i = childCount - 1; i >= 0; i--)
            {
                GameObject cardObj = transform.GetChild(i).gameObject;
                CardObject card = cardObj.GetComponent<CardObject>();
                ObjectPoolManager.Instance.ReturnObject(card.CardPoolType, cardObj);
            }

            for (int i = 0; i < _shuffleCount; i++)
            {
                int index = Random.Range(4, 8); // 4 ~ 7 인덱스 카드 랜덤 선택
                RectTransform randomUICardRectTrans = _uiCardDeck.GetChild(index).GetComponent<RectTransform>(); // 랜덤 선택 카드
                RectTransform frontUICardRectTrans = _uiCardDeck.GetChild(_uiCardDeck.childCount - 1).GetComponent<RectTransform>(); // 맨 위 카드

                float randomCardTargetY = frontUICardRectTrans.anchoredPosition.y; // 랜덤하게 선택된 카드의 y 목표 값을 맨 앞 카드의 y값으로 할당
                float frontCardTargetY = randomUICardRectTrans.anchoredPosition.y; // 맨 앞 카드의 y 목표 값을 랜덤하게 선택된 카드의 y값으로 할당

                await randomUICardRectTrans.DOAnchorPosY(_shuffleMinYPos, _cardShuffleDuration)
                    .OnComplete(() =>
                    {
                        randomUICardRectTrans.SetAsLastSibling();
                        frontUICardRectTrans.SetSiblingIndex(index);
                        frontUICardRectTrans.anchoredPosition = new Vector3(0, frontCardTargetY, 0);
                    }).AsyncWaitForCompletion();

                await randomUICardRectTrans.DOAnchorPosY(randomCardTargetY, _cardShuffleDuration).AsyncWaitForCompletion();
            }

            await _deckShuffleAnimationUI.DOFade(0, _uiFadeDuration).AsyncWaitForCompletion(); // ui 객체 페이드 아웃
            _deckShuffleAnimationUI.gameObject.SetActive(false);

            await Task.Yield(); // 한 프레임 대기를 통한 연출의 자연스러움 추가
            DeckManager.Instance.DeckMakeCheckTcs?.TrySetResult(true); // 덱 생성 완료
        }
    }
}
// 마지막 작성 일자: 2025.11.25