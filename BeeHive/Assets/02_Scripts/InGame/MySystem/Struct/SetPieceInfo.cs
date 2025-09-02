using UnityEngine;

namespace InGame.MyObject
{
    // 작성자: 조혜찬
    // 기물을 옮길 때 필요한 값을 가지는 구조체
    public struct SetPieceInfo
    {
        public int objectId; // 객체 id
        public string parentName; // 부모 객체 이름
        public Vector3 targetPos; // 이동해야할 위치
    }
}
// 마지막 작성 일자: 2025.09.02