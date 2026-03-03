using UnityEngine;

namespace MyUtil.MyObjectPool
{
    // 작성자: 조혜찬
    // 만들 풀 객체의 정보를 가지는 구조체
    public struct MakeObjectPoolData
    {
        public int poolType; // 풀 타입
        public int Id; // 네트워크 ID
        public int roadPlacePlaneId; // 도로 배치칸 ID
        public bool needAnimation; // 애니메이션 필요 여부
        public string parentName; // 부모 객체 명
        public float angle; // 각도
        public Vector3 pos; // 객체 위치
    }
}
// 마지막 작성 일자: 2026.03.03