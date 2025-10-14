using MyUtil;
using MyUtil.MyObjectPool;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        [SerializeField] private int _castleUpgradeCardCount; // 성벽 강화 카드 수
        [SerializeField] private int _droughtCardCount; // 가뭄 카드 수
        [SerializeField] private int _goodHarvestCardCount; // 풍년 카드 수
        [SerializeField] private int _roadChangeCardCount; // 도로 변형 카드 수
        [SerializeField] private int _firePowerCardCount; // 화력 카드 수

        [SerializeField] private float _yInterval; // 카드 간의 y축 간격

        private List<ObjectPoolType> _deckList = new List<ObjectPoolType>(); // 덱 리스트

        // 변수 초기화
        private void Awake()
        {
            deckTransform = GetComponent<Transform>();

            _ = CreateDeck(); // 덱 생성
        }

        private async Task CreateDeck()
        {
            AddCardInToDeck(_castleUpgradeCardCount, ObjectPoolType.CastleUpgradeCard);
            AddCardInToDeck(_droughtCardCount, ObjectPoolType.DroughtCard);
            AddCardInToDeck(_firePowerCardCount, ObjectPoolType.FirePowerCard);
            AddCardInToDeck(_goodHarvestCardCount, ObjectPoolType.GoodHarvestCard);
            AddCardInToDeck(_roadChangeCardCount, ObjectPoolType.RoadChangeCard);

            ShuffleUtility.Shuffle(_deckList); // 덱 셔플

            for(int i = 0; i <  _deckList.Count; i++) // 덱 리스트 순회
            {
                GameObject card = await ObjectPoolManager.Instance.GetObject(_deckList[i], deckTransform); // 카드 생성
                card.transform.localPosition = new Vector3(0, _yInterval * i, 0); // 카드를 생성할 수 록 y축 간격 만큼 위로 올리기
            }
        }

        // 덱에 카드를 추가하는 함수
        private void AddCardInToDeck(int count, ObjectPoolType addType)
        {
            for(int i = 0; i < count; i++)
            {
                _deckList.Add(addType);
            }
        }
    }
}
// 마지막 작성 일자: 2025.10.02