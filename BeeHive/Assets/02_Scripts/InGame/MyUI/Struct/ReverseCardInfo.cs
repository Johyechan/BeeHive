using UnityEngine;

namespace InGame.MyUI
{
    // 작성자: 조혜찬
    // 카드를 뒤집을 때 필요한 값을 가지는 구조체
    public struct ReverseCardInfo
    {
        public string roomID; // 현재 방 ID
        public int cardID; // 뒤집히는 카드의 ID
        public float animationDuration; // 애니메이션 지속 시간
        public int cardUseTeam; // 카드를 사용한 팀
    }
}
// 마지막 작성 일자: 2026.06.22