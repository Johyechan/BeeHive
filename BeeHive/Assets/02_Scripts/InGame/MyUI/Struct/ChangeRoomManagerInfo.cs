using System;

namespace InGame.MyUI
{
    // 작성자: 조혜찬
    // 방장 변경에 필요한 값을 가지는 구조체
    [Serializable]
    public struct ChangeRoomManagerInfo
    {
        public string roomID; // 현재 방 ID
        public int targetIndex; // 방장으로 선택된 플레이어의 순서
    }
}
// 마지막 작성 일자: 2025.09.04