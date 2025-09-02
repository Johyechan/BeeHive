using UnityEngine;

namespace InGame.MySystem
{
    // 작성자: 조혜찬
    // 금화 및 금괴를 변경할 때 필요한 값을 가지는 구조체
    public struct ChangeGoldInfo
    {
        public string roomID; // 방 ID
        public string targetID; // 현재 금화 및 금괴가 변경되는 클라이언트 ID
        public int goldCoinCount; // 금화 개수
        public int goldBarCount; // 금괴 개수
    }
}
// 마지막 작성 일자: 2025.09.02