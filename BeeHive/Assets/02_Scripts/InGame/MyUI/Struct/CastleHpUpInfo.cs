using System;
using UnityEngine;

namespace InGame.MyUI
{
    // 작성자: 조혜찬
    // 성 체력을 올릴 때 필요한 값을 가지는 구조체
    [Serializable] // 직렬화
    public struct CastleHpUpInfo
    {
        public string roomID; // 현재 방 ID
        public int changeTeamType; // 체력이 올라간 팀 타입
        public int changedHp; // 바뀐 체력
    }
}
// 마지막 작성 일자: 2025.10.20