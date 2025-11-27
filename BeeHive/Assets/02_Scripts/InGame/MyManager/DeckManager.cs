using InGame.MyObject;
using MyUtil;
using System.Threading.Tasks;
using UnityEngine;

namespace InGame.MyManager
{
    // 작성자: 조혜찬
    // 덱에 대한 정보를 가지는 싱글톤 클래스
    public class DeckManager : MonoSingleton<DeckManager>
    {
        [SerializeField] private DeckCardInfo _deckCardInfo; // 덱에 필요한 카드들의 정보를 가지는 구조체 변수

        [SerializeField] private Deck _deck; // 사용하지 않은 카드들을 모아두는 덱
        public Deck DeckProp { get => _deck; }

        [SerializeField] private UsedDeck _usedDeck; // 사용된 카드들을 모아두는 덱
        public UsedDeck UsedDeckProp { get => _usedDeck; }

        private bool _isEmpty;
        public bool IsEmpty { get => _isEmpty; set => _isEmpty = value; }

        private TaskCompletionSource<bool> _deckMakeCheckTcs;
        public TaskCompletionSource<bool> DeckMakeCheckTcs { get => _deckMakeCheckTcs; set => _deckMakeCheckTcs = value; }

        // 덱 제작 함수(현재 방 ID)
        public async Task MakeDeck(string currentRoomID, int castleUpdradeCardCount = 0, int droughtCardCount = 0, int goodHarvestCardCount = 0, int roadChangeCardCount = 0, int firePowerCardCount = 0)
        {
            _deckMakeCheckTcs = new TaskCompletionSource<bool>(); // 새로운 tcs 할당

            int castleCard = castleUpdradeCardCount == 0 ? _deckCardInfo.castleUpgradeCardCount : castleUpdradeCardCount; // 성벽 카드 수
            int droughtCard = droughtCardCount == 0 ? _deckCardInfo.droughtCardCount : droughtCardCount; // 가뭄 카드 수
            int goodHarvestCard = goodHarvestCardCount == 0 ? _deckCardInfo.goodHarvestCardCount : goodHarvestCardCount; // 풍년 카드 수
            int roadChangeCard = roadChangeCardCount == 0 ? _deckCardInfo.roadChangeCardCount : roadChangeCardCount; // 도로 변형 카드 수
            int firePowerCard = firePowerCardCount == 0 ? _deckCardInfo.firePowerCardCount : firePowerCardCount; // 화력 카드 수

            DeckShuffleInfo deckShuffleInfo = new DeckShuffleInfo()
            {
                roomID = currentRoomID, // 현재 방 ID
                castleUpgradeCardCount = castleCard, // 성벽 강화 카드 수
                droughtCardCount = droughtCard, // 가뭄 카드 수
                goodHarvestCardCount = goodHarvestCard, // 풍년 카드 수
                roadChangeCardCount = roadChangeCard, // 도로 변형 카드 수
                firePowerCardCount = firePowerCard, // 화력 카드 수
            };
            string json = JsonUtility.ToJson(deckShuffleInfo); // Json 형태로 변환

            if (TeamManager.Instance.CurrentTeamType == MyEnum.TeamType.Team1) // 팀 1이 시작 덱을 만듦 (중복 제작을 통한 30장의 덱이 아닌 60장, 90장의 덱이 만들어지는 것을 막기위함
            {
                NetworkManager.Instance.Socket.Emit("shuffle", json); // 서버로 전송
            }

            await Task.CompletedTask;
        }

        public async Task DeckMakeEnd()
        {
            await _deckMakeCheckTcs.Task;
        }

        public async void ReMakeDeck()
        {
            await MakeDeck(SceneMgr.Instance.CurrentRoomID, _usedDeck.UsedDeckData.castleCardCount, _usedDeck.UsedDeckData.droughtCardCount, _usedDeck.UsedDeckData.goodHarvestCardCount, _usedDeck.UsedDeckData.roadChangeCardCount, _usedDeck.UsedDeckData.firePowerCardCount);
        }
    }
}
// 마지막 작성 일자: 2025.11.27