using System;

namespace InGame.MySystem
{
    // 작성자: 조혜찬
    // 금화 및 금괴 객체를 세팅할 때 필요한 구조체
    [Serializable]
    public struct SetGoldInfo
    {
        public int team; // 팀
        public int goldCoin; // 금화 개수
        public int goldBar; // 금괴 개수
    }
}
// 마지막 작성 일자: 2025.09.04