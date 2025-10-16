using UnityEngine;

namespace InGame.MyObject
{
    // 작성자: 조혜찬
    // 덱 셔플에 필요한 값을 가지는 구조체
    public struct DeckShuffleInfo
    {
        public string roomID; // 현재 방 ID
        public int castleUpgradeCardCount; // 성벽 강화 카드 수
        public int droughtCardCount; // 가뭄 카드 수
        public int goodHarvestCardCount; // 풍년 카드 수
        public int roadChangeCardCount; // 도로 변형 카드 수
        public int firePowerCardCount; // 화력 카드 수
    }
}
// 마지막 작성 일자: 2025.10.16