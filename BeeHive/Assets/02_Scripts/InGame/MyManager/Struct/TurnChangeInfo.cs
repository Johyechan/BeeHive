using UnityEngine;

namespace InGame.MyManager
{
    // 작성자: 조혜찬
    // 턴 변경 이벤트를 서버에 보낼 때 필요한 값을 가지는 구조체
    public struct TurnChangeInfo
    {
        public string roomID; // 현재 방 ID
        public int team; // 현재 클라이언트의 팀
    }
}
// 마지막 작성 일자: 2025.08.26