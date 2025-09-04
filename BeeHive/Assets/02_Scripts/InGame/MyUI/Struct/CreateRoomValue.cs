using System;

namespace InGame.MyUI
{
    // 작성자: 조혜찬
    // 방을 생성할 때 필요한 값을 가지는 구조체
    [Serializable]
    public struct CreateRoomValue
    {
        public string roomName;
        public int maxPlayer;
        public string socketName;
    }
}
// 마지막 작성 일자: 2025.08.08