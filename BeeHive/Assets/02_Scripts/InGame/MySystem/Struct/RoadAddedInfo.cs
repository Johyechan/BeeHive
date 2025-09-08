using System;

namespace InGame.MySystem
{
    [Serializable]
    public struct RoadAddedInfo
    {
        public int roadCount; // 추가된 도로 개수
        public int teamType; // 추가된 도로의 팀 타입
        public string roadParentName; // 도로 객체의 부모 명
    }
}
// 마지막 작성 일자: 2025.09.08