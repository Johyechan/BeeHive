using System;
using UnityEngine;

namespace InGame.MySystem
{
    // 작성자: 조혜찬
    // 도로 세팅에 필요한 값을 가지는 구조체
    [Serializable]
    public struct SetRoadInfo
    {
        public int roadID; // 도로 객체 ID
        public int placePlaneId; // 배치 칸 객체 ID
        public int placedType; // 배치한 타입
        public int roadTeamType; // 배치한 도로의 팀 타입
        public string roadParentName; // 부모 객체 이름
        public string targetParentName; // 이동해야할 위치 부모 객체 이름
        public Vector3 targetPos; // 이동해야할 위치
        public float angle; // 회전 각도
    }
}
// 마지막 작성 일자: 2025.09.04