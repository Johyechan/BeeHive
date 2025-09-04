using DG.Tweening;
using InGame.MyEnum;
using InGame.MyEvent;
using UnityEngine;

namespace InGame.MyObject
{
    // 작성자: 조혜찬
    // 덱 클래스 - 클릭되었을 때 카드를 추가 시키는 기능을 가지는 클래스
    public class Deck : MonoBehaviour
    {
        public Transform player1CardsParent; // 플레이어1 카드들의 부모 Transform 변수
        public Transform player2CardsParent; // 플레이어2 카드들의 부모 Transform 변수
        public Transform player3CardsParent; // 플레이어3 카드들의 부모 Transform 변수

        public RectTransform _playerUICardsParent; // 플레이어 UI 카드들의 부모 RectTransform 변수

        public Transform deckTransform; // 덱 Transform 변수 - 현재 덱에 있는 카드의 수를 알기 위한 변수

        // 변수 초기화
        private void Awake()
        {
            deckTransform = GetComponent<Transform>();
        }
    }
}
// 마지막 작성 일자: 2025.08.29