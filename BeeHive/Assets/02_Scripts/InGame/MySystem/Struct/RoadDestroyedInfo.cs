using InGame.MyEnum;
using UnityEngine;

namespace InGame.MySystem
{
    // 작성자: 조혜찬
    // 사용하지 않은 도로를 파괴할 때 필요한 값을 가지는 구조체
    public struct RoadDestroyedInfo
    {
        public string roadParentName; // 파괴될 도로 객체들의 부모 객체 명
        public int teamType; // 파괴될 도로 객체들의 팀 타입
    }
}
// 마지막 작성 일자: 2025.09.08