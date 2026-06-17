using System;
using UnityEngine;

namespace InGame.MyObject
{
    // 작성자: 조혜찬
    // 게임 오버에 필요한 값을 가지는 구조체
    [Serializable]
    public struct GameOverInfo
    {
        public string roomID; // 현재 방 ID
        public int loseTeamType; // 패배한 팀 타입
        public int isSurrender; // 항복 여부
    }
}
// 마지막 작성 일자: 2026.06.17