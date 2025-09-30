using UnityEngine;

namespace InGame.MySystem
{
    // 작성자: 조혜찬
    // 도로 변경을 적용할 때 필요한 값을 가지는 구조체
    public struct PieceChangedRoadInfo
    {
        public int teamType; // 바뀔 도로의 팀 타입
        public int placePlaneID; // 바뀔 도로들의 공통 기물 배치 칸 ID
        public int pieceID; // 주위 도로를 변경 시킬 기물의 ID
    }
}
// 마지막 작성 일자: 2025.09.30