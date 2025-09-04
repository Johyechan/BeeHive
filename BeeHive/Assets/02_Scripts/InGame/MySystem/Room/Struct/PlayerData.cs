using System;

namespace InGame.MySystem
{
    // 작성자: 조혜찬
    // 플레이어 정보 구조체
    [Serializable] // 직렬화하여 JsonUtility에서 파싱 가능하도록 변경
    public struct PlayerData
    {
        public string id; // 플레이어 ID
        public string nickName; // 플레이어 이름
        public int index; // 슬롯 인덱스
        public bool isRoomManager; // 방장 여부
        public bool isReady; // 준비 완료 여부
        public int team; // 팀
        public int goldCoin; // 금화 개수
        public int goldBar; // 금괴 개수
        public int card; // 카드 개수
    }
}
// 마지막 작성 일자: 2025.08.28