using System;

namespace InGame.MySystem
{
    // 작성자: 조혜찬
    // 방 정보 구조체
    [Serializable] // 직렬화
    public struct RoomInfo
    {
        public string ID; // 방 ID 
        public string Name; // 방 이름
        public int maxPlayer; // 최대 입장 가능한 플레이어 수
        public string host; // 방장
        public PlayerData[] players; // 플레이어들 - 플레이어 정보 UI에 배치 값을 주기 위해 필요한 변수
    }
}
// 마지막 작성 일자: 2025.08.08