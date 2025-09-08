using System;

namespace InGame.MySystem
{
    // 작성자: 조혜찬
    // 도로를 추가할 때 필요한 값을 가지는 구조체
    [Serializable]
    public struct AddRoadInfo
    {
        public string roomID; // 방 ID
        public int roadCount; // 추가된 도로 개수
        public int teamType; // 도로의 팀 타입
        public string roadParentName; // 도로의 부모 명
    }
}
// 마지막 작성 일자: 2025.09.08