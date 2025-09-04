using System;

namespace InGame.MyUI
{
    // 작성자: 조혜찬
    // 방에 입장할 때 필요한 값을 가지는 구조체
    [Serializable]
    public struct JoinRoomValue
    {
        public string roomID; // 참가할 방의 ID
        public string socketName; // 현재 클라이언트의 닉네임
    }
}
// 마지막 작성 일자: 2025.08.08