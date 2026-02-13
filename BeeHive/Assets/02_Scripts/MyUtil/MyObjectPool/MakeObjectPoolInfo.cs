using System;
using UnityEngine;

namespace MyUtil.MyObjectPool
{
    // 작성자: 조혜찬
    // 서버에 풀링 객체의 제작을 요청할 때 필요한 값을 가지는 구조체
    [Serializable]
    public struct MakeObjectPoolInfo
    {
        public string roomID; // 방 ID
        public string parentName; // 부모 객체 명
        public int poolType; // 풀 타입
        public int roadPlacePlaneId; // 도로 배치 칸 ID
        public float angle; // 각도
        public Vector3 pos; // 배치할 위치
    }
}
// 마지막 작성 일자: 2026.02.13