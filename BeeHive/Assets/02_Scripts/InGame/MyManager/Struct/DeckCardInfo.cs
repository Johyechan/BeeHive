using System;
using UnityEngine;

namespace InGame.MyManager
{
    // 작성자: 조혜찬
    // 덱에 사용되는 카드들의 정보를 가지는 구조체
    [Serializable] // 직렬화
    public struct DeckCardInfo
    {
        public int castleUpgradeCardCount; // 성벽 강화 카드 수
        public int droughtCardCount; // 가뭄 카드 수
        public int goodHarvestCardCount; // 풍년 카드 수
        public int roadChangeCardCount; // 도로 변형 카드 수
        public int firePowerCardCount; // 화력 카드 수
    }
}
// 마지막 작성 일자: 2025.10.20