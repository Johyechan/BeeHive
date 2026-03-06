using InGame.MyObject.Piece;
using System;

namespace Tutorial.Struct
{
    // 작성자: 조혜찬
    // 튜토리얼을 위해 세팅되어 있는 객체들의 연결을 위한 기물 구조체
    [Serializable] // Inspector 창에서 값을 할당하기 위한 직렬화
    public struct TutorialPieceData
    {
        public PieceBase piece; // 기물
        public int connectNumber; // 연결 번호
    }
}
// 마지막 작성 일자: 2026.03.06