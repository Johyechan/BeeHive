using System;

namespace InGame.MyObject
{
    // 작성자: 조혜찬
    // 서버에서 드로우 이벤트를 처리할 때 필요한 값을 가지는 구조체
    [Serializable]
    public struct DrawInfo
    {
        public string roomID; // 현재 방 ID
    }
}
// 마지막 작성 일자: 2026.01.09