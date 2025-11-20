using DG.Tweening;
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
        [SerializeField] private Transform _deckTransform; // 덱 트랜스폼

        [SerializeField] private float _cardMoveDuration; // 카드 이동 지속시간
        [SerializeField] private float _animationDuration; // 애니메이션 지속시간
        [SerializeField] private float _yInterval; // y축 간격
        [SerializeField] private float _suffleMinYPos; // 셔플 y 위치
        [SerializeField] private float _suffleMaxYPos; // 셔플 y 위치

        [SerializeField] private int _suffleCount; // 셔플 횟수

        [SerializeField] private RectTransform _uiCardDeck; // ui 덱

        // 사용한 카드들을 덱에 추가하는 함수
        public void AddCardInToUsedDeck(Transform addCardTrans)
        {
            addCardTrans.SetParent(transform); // 추가한 카드의 부모를 자기 자신으로 할당
            int usedCardCount = transform.childCount; // 사용한 카드들을 모아두는 덱에 있는 카드 수

            DOTween.Sequence()
                .AppendInterval(_animationDuration) // 대기
                .Append(addCardTrans.DOLocalMove(new Vector3(0, _yInterval * usedCardCount, 0), _animationDuration)) // 사용한 카드 위치 이동
                .AppendCallback(() => DrawEventSystem.OnCardUISet?.Invoke()); // 카드 UI 재세팅
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(DeckSuffle());
            }
        }

        private IEnumerator DeckSuffle()
        {
            for (int i = 0; i < _suffleCount; i++)
            {
                int index = Random.Range(4, 8);
                RectTransform randomUICardRectTrans = _uiCardDeck.GetChild(index).GetComponent<RectTransform>(); // 랜덤 선택 카드
                RectTransform frontUICardRectTrans = _uiCardDeck.GetChild(_uiCardDeck.childCount - 1).GetComponent<RectTransform>(); // 맨 위 카드

                float currentTime = 0;
                Vector3 randomCardStartPos = randomUICardRectTrans.anchoredPosition;
                Vector3 randomCardEndPos = new Vector3(0, _suffleMinYPos, 0);

                Vector3 frontCardStartPos = frontUICardRectTrans.anchoredPosition;
                Vector3 frontCardEndPos = new Vector3(0, _suffleMaxYPos, 0);

                while(currentTime <= _animationDuration)
                {
                    float t = currentTime / _animationDuration;
                    randomUICardRectTrans.anchoredPosition = Vector3.Lerp(randomCardStartPos, randomCardEndPos, t);
                    frontUICardRectTrans.anchoredPosition = Vector3.Lerp(frontCardStartPos, frontCardEndPos, t);
                    currentTime += Time.deltaTime;
                    yield return null;
                }

                currentTime = 0;
                randomUICardRectTrans.SetAsLastSibling();
                frontUICardRectTrans.SetSiblingIndex(index);

                randomCardEndPos = frontCardStartPos; // 맨 앞이었던 카드의 처음 위치를 랜덤하게 선택된 카드의 마지막 위치로 할당
                frontCardEndPos = randomCardStartPos; // 랜덤하게 선택된 카드의 처음 위치를 맨 앞이었던 카드의 마지막 위치로 할당

                frontCardStartPos = frontUICardRectTrans.anchoredPosition; // 맨 앞이었던 카드의 현재 위치 저장
                randomCardStartPos = randomUICardRectTrans.anchoredPosition; // 랜덤하게 선택된 카드의 현재 위치 저장

                while (currentTime <= _animationDuration)
                {
                    float t = currentTime / _animationDuration;
                    randomUICardRectTrans.anchoredPosition = Vector3.Lerp(randomCardStartPos, randomCardEndPos, t);
                    frontUICardRectTrans.anchoredPosition = Vector3.Lerp(frontCardStartPos, frontCardEndPos, t);
                    currentTime += Time.deltaTime;
                    yield return null;
                }
            }
        }
    }
}
// 마지막 작성 일자: 2025.11.20