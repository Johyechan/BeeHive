using InGame.MyObject;
using MyUtil;
using UnityEngine;

namespace InGame.MyManager
{
    // 작성자: 조혜찬
    // 덱에 대한 정보를 가지는 싱글톤 클래스
    public class DeckManager : MonoSingleton<DeckManager>
    {
        [SerializeField] private DeckCardInfo _deckCardInfo; // 덱에 필요한 카드들의 정보를 가지는 구조체 변수

        // 덱 제작 함수(현재 방 ID)
        public void MakeDeck(string currentRoomID)
        {
            DeckShuffleInfo deckShuffleInfo = new DeckShuffleInfo()
            {
                roomID = currentRoomID, // 현재 방 ID
                castleUpgradeCardCount = _deckCardInfo.castleUpgradeCardCount, // 성벽 강화 카드 수
                droughtCardCount = _deckCardInfo.droughtCardCount, // 가뭄 카드 수
                goodHarvestCardCount = _deckCardInfo.goodHarvestCardCount, // 풍년 카드 수
                roadChangeCardCount = _deckCardInfo.roadChangeCardCount, // 도로 변형 카드 수
                firePowerCardCount = _deckCardInfo.firePowerCardCount, // 화력 카드 수
            };
            string json = JsonUtility.ToJson(deckShuffleInfo); // Json 형태로 변환

            if (TeamManager.Instance.CurrentTeamType == MyEnum.TeamType.Team1) // 팀 1이 시작 덱을 만듦 (중복 제작을 통한 30장의 덱이 아닌 60장, 90장의 덱이 만들어지는 것을 막기위함
            {
                NetworkManager.Instance.Socket.Emit("shuffle", json); // 서버로 전송
            }
        }
    }
}
// 마지막 작성 일자: 2025.10.20