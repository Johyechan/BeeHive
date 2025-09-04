using System;

namespace InGame.MyManager
{
    // 작성자: 조혜찬
    // 턴 완료 이벤트를 서버에 보낼 때 필요한 값을 가지는 구조체
    [Serializable]
    public struct TurnCompletedInfo
    {
        public string roomID; // 현재 방 ID
        public string targetID; // 현재 클라이언트 ID
    }
}
// 마지막 작성 일자: 2025.09.04