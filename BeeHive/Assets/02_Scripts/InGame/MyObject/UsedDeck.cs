using DG.Tweening;
using InGame.MyManager;
using InGame.MyManager.MyCard;
using MyUtil.MyEvent;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

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
        [SerializeField] private float _cardHight; // 카드 높이

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
            for(int i = transform.childCount - 1; i >= 0; i--)
            {
                Transform cardTrans = transform.GetChild(i); // 현재
                cardTrans.SetParent(_deckTransform); // 부모 변경
                StartCoroutine(CardMoveCo(cardTrans));
            }
        }

        private IEnumerator CardMoveCo(Transform cardTrans)
        {
            float currentTime = 0;
            float currentXPos = cardTrans.localPosition.x; // 현재 x축 값 저장

            while (currentTime < _cardMoveDuration)
            {
                currentTime += Time.deltaTime;
                float angle = (180 / _cardMoveDuration) * currentTime; // 현재 각도
                float hight = Mathf.Sin(Mathf.Deg2Rad * angle); // 현재 각도의 사인 값 구하기
                float yPos = hight * _cardHight; // y축 좌표
                float xPos = currentXPos - (currentXPos / _cardMoveDuration) * currentTime; // x축 좌표
                float zRot = 180 - (180 / _cardMoveDuration) * currentTime; // z축 회전값 (시작값이 180인 이유는 카드가 뒤집혀있는 상태이기 때문)
                cardTrans.localPosition = new Vector3(xPos, yPos, cardTrans.localPosition.z); // 위치 변경
                cardTrans.localRotation = Quaternion.Euler(0, 0, zRot); // 회전
                yield return null;
            }
        }
    }
}
// 마지막 작성 일자: 2025.11.14