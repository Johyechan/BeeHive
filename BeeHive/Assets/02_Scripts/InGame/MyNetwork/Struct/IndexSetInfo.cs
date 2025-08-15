using UnityEngine;

namespace InGame.MyNetwork
{
    // 작성자: 조혜찬
    // 플레이어가 사용할 UI 인덱스를 저장해줄 때 필요한 값을 가지는 구조체
    public struct IndexSetInfo
    {
        public string roomID; // 현재 방 ID
        public string targetID; // 인덱스를 지정할 플레이어 클라이언트 ID
        public int index; // 인덱스 값
    }
}
// 마지막 작성 일자: 2025.08.15