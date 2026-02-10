using System;
using UnityEngine;

namespace InGame.MyUI
{
    // 작성자: 조혜찬
    // 방 묶음 구조체
    [Serializable]
    public struct RoomPacket
    {
        public RoomData[] roomArr; // 방 배열
    }
}
// 마지막 작성 일자: 2026.02.10