using System;
using UnityEngine;

namespace InGame.MyObject
{
    // 작성자: 조혜찬
    // 성이 공격 받았을 때 필요한 값을 가지는 구조체
    [Serializable]
    public struct CastleHitInfo
    {
        public int attackedCaslteType; // 공격 받은 성 타입
        public int damage; // 데미지
        public int objectID; // 객체 ID
    }
}
// 마지막 작성 일자: 2025.10.28