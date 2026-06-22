using System;
using UnityEngine;

namespace InGame.MySystem
{
    // 작성자: 조혜찬
    // 카드가 뒤집힐 때 필요한 값을 가지는 구조체
    [Serializable]
    public struct CardReverseInfo
    {
        public int cardID; // 뒤집힐 카드의 ID
        public float animationDuration; // 애니메이션 지속 시간
        public int cardUseTeam; // 카드를 사용한 팀
    }
}
// 마지막 작성 일자: 2026.06.22