using InGame.MySystem;
using UnityEngine;

namespace InGame.MyNetwork
{
    // 작성자: 조혜찬
    // 방에 입장할 때 서버에서 보내는 값을 받기 위한 구조체
    public struct JoinRoomInfo
    {
        public string roomID; // 현재 방 ID
        public PlayerData[] players; // 방에 접속해있는 플레이어 배열
        public int maxPlayer; // 현재 방의 최대 플레이어 수
    }
}
// 마지막 작성 일자: 2025.08.15