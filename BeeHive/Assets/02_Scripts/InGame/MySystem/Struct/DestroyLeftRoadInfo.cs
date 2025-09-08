using System;

namespace InGame.MySystem
{
    // 작성자: 조헤찬
    // 도로를 제거할 때 필요한 값을 가지는 구조체
    [Serializable]
    public struct DestroyLeftRoadInfo
    {
        public string roomID; // 현재 방 ID
        public string roadParentName; // 파괴할 사용하지 않은 도로 객체들의 부모 객체 명
        public int teamType; // 파괴하는 도로의 타입
    }
}
// 마지막 작성 일자: 2025.09.08