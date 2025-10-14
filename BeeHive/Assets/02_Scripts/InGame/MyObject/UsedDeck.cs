using DG.Tweening;
using InGame.MyManager;
using UnityEngine;

namespace InGame.MyObject
{
    // 작성자: 조혜찬
    // 사용한 카드들을 모아두는 덱
    public class UsedDeck : MonoBehaviour
    {
        [SerializeField] private float _animationDuration; // 애니메이션 지속시간
        [SerializeField] private float _yInterval; // y축 간격

        // 사용한 카드들을 덱에 추가하는 함수
        public void AddCardInToUsedDeck(Transform addCardTrans)
        {
            addCardTrans.SetParent(transform); // 추가한 카드의 부모를 자기 자신으로 할당
            int usedCardCount = transform.childCount; // 사용한 카드들을 모아두는 덱에 있는 카드 수

            addCardTrans.DOLocalMove(new Vector3(0, _yInterval * usedCardCount, 0), _animationDuration); // 사용한 카드 위치 이동
        }
    }
}
// 마지막 작성 일자: 2025.10.14