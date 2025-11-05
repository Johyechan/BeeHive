using System;
using UnityEngine;

namespace InGame.MyUI
{
    // 작성자: 조혜찬
    // 방으로 돌아갈 때 필요한 값을 가지는 구조체
    [Serializable]
    public struct GoToRoomInfo
    {
        public string roomID; // 현재 방 ID
        public string clientID; // 현재 클라이언트 ID
    }
}
// 마지막 작성 일자: 2025.11.05