using InGame.MyManager.Global;
using InGame.MyObject;
using MyUtil;
using System.Threading.Tasks;
using UnityEngine;

namespace InGame.MyManager.Local
{
    // 작성자: 조혜찬
    // 덱에 대한 정보를 가지는 클래스
    public class DeckManager : MonoBehaviour
    {
        [SerializeField] private DeckCardInfo _deckCardInfo; // 덱에 필요한 카드들의 정보를 가지는 구조체 변수

        [SerializeField] private Deck _deck; // 사용하지 않은 카드들을 모아두는 덱
        public Deck DeckProp { get => _deck; }

        [SerializeField] private UsedDeck _usedDeck; // 사용된 카드들을 모아두는 덱
        public UsedDeck UsedDeckProp { get => _usedDeck; }

        private bool _isEmpty;
        public bool IsEmpty { get => _isEmpty; set => _isEmpty = value; }

        private TaskCompletionSource<bool> _deckMakeCheckTcs;

        // 외부 접근용 덱 제작 대기 tcs 생성 함수
        public void CreateTcs()
        {
            if (_deckMakeCheckTcs == null) // 덱 제작 대기 tcs가 null일 때
                _deckMakeCheckTcs = new TaskCompletionSource<bool>(); // 새로운 tcs 제작 함수
        }

        // 외부 접근용 함수
        public void CompleteTcs()
        {
            TryDeckMakeTcsComplete();
        }

        // 덱 제작 대기 tcs가 완료 가능한지 체크하고 완료 시키는 함수
        private void TryDeckMakeTcsComplete()
        {
            if (_deckMakeCheckTcs == null) // 덱 제작 대기 tcs가 null이면
                return; // 반환

            if (_deckMakeCheckTcs.Task.IsCompleted) // 덱 제작 대기 tcs가 완료 되었다면
                return; // 반환

            _deckMakeCheckTcs?.TrySetResult(true);
        }

        // 덱 제작 함수(현재 방 ID)
        public void MakeDeck(string currentRoomID, int castleUpdradeCardCount = 0, int droughtCardCount = 0, int goodHarvestCardCount = 0, int roadChangeCardCount = 0, int firePowerCardCount = 0)
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

            NetworkManager.Instance.Socket.Emit("shuffle", json); // 서버로 전송
        }

        public async Task DeckMakeEnd()
        {
            await _deckMakeCheckTcs.Task;
        }

        public void ReMakeDeck()
        {
            MakeDeck(SceneMgr.Instance.CurrentRoomID, _usedDeck.UsedCardInfo.castleCardCount, _usedDeck.UsedCardInfo.droughtCardCount, _usedDeck.UsedCardInfo.goodHarvestCardCount, _usedDeck.UsedCardInfo.roadChangeCardCount, _usedDeck.UsedCardInfo.firePowerCardCount);
        }
    }
}
// 마지막 작성 일자: 2026.02.03