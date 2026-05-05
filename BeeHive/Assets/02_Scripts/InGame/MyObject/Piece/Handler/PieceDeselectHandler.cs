using InGame.MyEvent;
using InGame.MyManager.Local;

namespace InGame.MyObject.Piece.Handler
{
    // 작성자: 조혜찬
    // 선택 해제 핸들러 클래스
    public class PieceDeselectHandler
    {
        public void PieceDeselect()
        {
            HighLightEvents.OnPieceMovementHighLight?.Invoke(false, false); // 하이라이트 끄기, 이동 가능 배치 칸 대상
            PieceEvents.OnHideCanAttackPieces?.Invoke(true); // 공격 가능한 기물들 하이라이트 끄기
        }
    }
}
// 마지막 작성 일자: 2026.05.05