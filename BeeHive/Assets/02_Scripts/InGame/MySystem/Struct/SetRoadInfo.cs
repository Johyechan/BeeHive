using UnityEngine;

namespace InGame.MySystem
{
    // 작성자: 조혜찬
    // 도로 세팅에 필요한 값을 가지는 구조체
    public struct SetRoadInfo
    {
        public int objectId; // 객체 id
        public string parentName; // 부모 객체 이름
        public Vector3 targetPos; // 이동해야할 위치
        public float angle; // 회전 각도
    }
}
// 마지막 작성 일자: 2025.09.02