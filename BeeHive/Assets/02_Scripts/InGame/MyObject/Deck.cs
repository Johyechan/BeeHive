using InGame.MyManager;
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

        [SerializeField] private float _yInterval; // 카드 간의 y축 간격

        private List<ObjectPoolType> _deckList = new List<ObjectPoolType>(); // 덱 리스트

        // 변수 초기화
        private void Awake()
        {
            deckTransform = GetComponent<Transform>();

            NetworkManager.Instance.Socket.On("deckShuffled", (data) => // 서버로부터 덱 받기
            {
                string json = data.GetValue().ToString(); // 서버가 전송한 값 받기

                MainThreadDispatcher.Enqueue(() =>
                {
                    _deckList.Clear();
                    DeckInfo deckInfo = JsonUtility.FromJson<DeckInfo>(json); // DeckInfo로 변환

                    for (int i = 0; i < deckInfo.deck.Length; i++) // 덱에 있는 카드 수 만큼 반복
                    {
                        switch (deckInfo.deck[i])
                        {
                            case 1: // 성벽 강화 카드
                                _deckList.Add(ObjectPoolType.CastleUpgradeCard);
                                break;
                            case 2: // 가뭄 카드
                                _deckList.Add(ObjectPoolType.DroughtCard);
                                break;
                            case 3: // 풍년 카드
                                _deckList.Add(ObjectPoolType.GoodHarvestCard);
                                break;
                            case 4: // 도로 변형 카드
                                _deckList.Add(ObjectPoolType.RoadChangeCard);
                                break;
                            case 5: // 화력 카드
                                _deckList.Add(ObjectPoolType.FirePowerCard);
                                break;
                        }
                    }

                    _ = CreateDeck(); // 덱 생성
                });
            });
        }

        private async Task CreateDeck()
        {
            await DeckManager.Instance.UsedDeckProp.DeckShuffleAnimationFadeIn();

            _ = DeckManager.Instance.UsedDeckProp.UsedDeckShuffle();

            for (int i = 0; i <  _deckList.Count; i++) // 덱 리스트 순회
            {
                GameObject card = await ObjectPoolManager.Instance.GetObject(_deckList[i], deckTransform); // 카드 생성
                card.transform.localPosition = new Vector3(0, _yInterval * i, 0); // 카드를 생성할 수 록 y축 간격 만큼 위로 올리기
            }
        }
    }
}
// 마지막 작성 일자: 2026.01.14