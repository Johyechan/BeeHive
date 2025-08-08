using UnityEngine;

namespace InGame.MyUI
{
    // 작성자: 조혜찬
    // 준비할 때 필요한 값들을 가지는 구조체
    public struct ReadyInfo
    {
        public string roomID; // 현재 방 ID
        public string targetID; // 준비 여부 변경 클라이언트 ID
        public bool isReady; // 준비 여부
    }
}
// 마지막 작성 일자: 2025.08.08