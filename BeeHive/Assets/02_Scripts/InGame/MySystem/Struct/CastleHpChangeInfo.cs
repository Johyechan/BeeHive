using System;
using UnityEngine;

namespace InGame.MySystem
{
    // 작성자: 조혜찬
    // 성 체력이 바뀌었을 때 필요한 값을 가지는 구조체
    [Serializable] // 직렬화
    public struct CastleHpChangeInfo
    {
        public int changeTeamType; // 체력이 올라간 팀 타입
        public int changedHp; // 바뀐 체력
    }
}
// 마지막 작성 일자: 2025.10.20