using UnityEngine;

namespace InGame.MySystem
{
    // 작성자: 조혜찬
    // 카드 세팅에 필요한 값들을 가지는 구조체
    public struct SetCardInfo
    {
        public PlayerData[] players; // 플레이어 배열
        public string targetID; // 드로우를 한 클라이언트 ID
        public int targetTeam; // 드로우를 한 클라이언트 팀
    }
}
// 마지막 작성 일자: 2025.08.29