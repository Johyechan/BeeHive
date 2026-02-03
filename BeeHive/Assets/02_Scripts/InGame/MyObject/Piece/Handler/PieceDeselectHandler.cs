using InGame.MyEvent;
using InGame.MyManager;
using InGame.MyManager.Local;
using System.Threading.Tasks;

namespace InGame.MyObject.Piece.Handler
{
    // 작성자: 조혜찬
    // 선택 해제 핸들러 클래스
    public class PieceDeselectHandler
    {
        private PieceBase _pieceBase; // 기물 클래스

        // 생성자(기물 클래스)
        public PieceDeselectHandler(PieceBase pieceBase)
        {
            _pieceBase = pieceBase;
        }

        public void PieceDeselect()
        {
            HighLightEvents.OnPieceMovementHighLight?.Invoke(false, false); // 하이라이트 끄기, 이동 가능 배치 칸 대상
            PieceEvents.OnHideCanAttackPieces?.Invoke(true); // 공격 가능한 기물들 하이라이트 끄기
        }

        public void HighLightOff(bool isOn, bool isMove = true) // 켜졌는지 여부, 이동 상태를 위해 켜졌는지 여부 = 어떤 값이 와도 상관 없음
        {
            if (isOn == false) // 끄는 상태일 때
            {
                InGameContext.Current.Data.GameManager.CurrentMovePiece = null; // 현재 이동하려는 기물을 null로 할당
                _pieceBase.PieceVariable.isSelected = false; // 선택 해제 된 상태로 할당
            }
        }
    }
}
// 마지막 작성 일자: 2026.02.03