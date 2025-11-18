using DG.Tweening;
using InGame.MyManager;
using InGame.MyManager.MyCard;
using InGame.MyUI.Card;
using MyUtil;
using MyUtil.MyEvent;
using MyUtil.MyObjectPool;
using System.Collections;
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

        private void Awake()
        {
            NetworkManager.Instance.Socket.On("", (data) =>
            {

            });
        }

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
                ReMakeDeck();
            }
        }

        // 덱 재제작 함수
        public void ReMakeDeck()
        {
            StartCoroutine(CardMoveCo());
        }

        private IEnumerator CardMoveCo()
        {
            int childCount = transform.childCount; // 자식 수 저장
            for (int i = childCount - 1; i >= 0; i--) // 사용한 카드들을 수만큼 반복
            {
                yield return null;

                Transform cardTrans = transform.GetChild(i); // 맨 위 카드
                cardTrans.SetParent(_deckTransform); // 부모 변경
                MainThreadDispatcher.Enqueue(() => { cardTrans.GetComponent<SortingGroup>().sortingOrder = _deckTransform.childCount; }); // 랜더링 순서 할당(값이 낮을 수록 뒤에 그려짐)

                float currentTime = 0;
                float currentYPos = cardTrans.localPosition.y; // 현재 y축 값 저장
                float currentXPos = cardTrans.localPosition.x; // 현재 x축 값 저장

                while (currentTime <= _cardMoveDuration)
                {
                    float t = currentTime / _cardMoveDuration; // 현재 시간 비율

                    float angle = 90 + 90 * t; // 현재 각도 (90부터 시작하는 이유는 sin 값이 1 -> 0으로 가는 형태로 반환하기를 원하기 때문)

                    float hight = Mathf.Sin(Mathf.Deg2Rad * angle); // 현재 각도의 사인 값 구하기
                    float yPos = hight * currentYPos + _yInterval * _deckTransform.childCount; // y축 좌표 - 현재 sin 값 * 현재 카드 높이 + 각 카드의 y축 간격 * 덱에 있는 자식 수

                    float xPos = Mathf.Lerp(currentXPos, 0, t); // x축 이동

                    float zRot = Mathf.Lerp(180, 0, t); // z축 회전값 (시작값이 180인 이유는 카드가 뒤집혀있는 상태이기 때문)

                    cardTrans.localPosition = new Vector3(xPos, yPos, cardTrans.localPosition.z); // 위치 변경
                    cardTrans.localRotation = Quaternion.AngleAxis(-zRot, Vector3.forward); // z축 회전

                    currentTime += Time.deltaTime;
                    yield return null;
                }

                ObjectPoolType currentCardObjectPoolType = cardTrans.GetComponent<UICardBase>().UICardData.poolType; // 풀 타입 저장
                Vector3 currentCardPos = cardTrans.localPosition; // 위치 저장
                ObjectPoolManager.Instance.ReturnObject(currentCardObjectPoolType, cardTrans.gameObject);
            }
        }
    }
}
// 마지막 작성 일자: 2025.11.18