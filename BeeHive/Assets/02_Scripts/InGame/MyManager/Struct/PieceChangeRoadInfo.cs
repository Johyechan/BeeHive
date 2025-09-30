namespace InGame.MyManager
{
    // 작성자: 조혜찬
    // 기물이 도로 변경에 필요한 값을 가지는 구조체
    public struct PieceChangeRoadInfo
    {
        public string roomID; // 현재 방 ID
        public int teamType; // 현재 팀 타입
        public int placePlaneID; // 배치 칸 ID
        public int pieceID; // 주위 도로를 변경 시킬 기물 ID
    }
}
// 마지막 작성 일자: 2025.09.30