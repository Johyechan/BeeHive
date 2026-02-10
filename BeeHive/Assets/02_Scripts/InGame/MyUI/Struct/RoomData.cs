using System;
using UnityEngine;

namespace InGame.MyUI
{
    // 작성자: 조혜찬
    // 방 정보
    [Serializable]
    public struct RoomData
    {
        public string roomID; // 방 ID
        public string roomName; // 방 이름
        public int currentPlayer; // 현재 참가자 수
        public bool isFull; // 참가자가 꽉 찼는지 여부 
        public bool isPlaying; // 플레이 여부
    }
}
// 마지막 작성 일자: 2026.02.10