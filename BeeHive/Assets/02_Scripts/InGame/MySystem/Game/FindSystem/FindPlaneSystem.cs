using InGame.MyEnum;
using InGame.MyObject;
using InGame.MyObject.Piece;
using InGame.MySystem.Game.FindSystem.Handler;
using InGame.MySystem.Game.FindSystem.Handler.PieceAttack;
using InGame.MySystem.Game.FindSystem.Handler.PieceCreate;
using InGame.MySystem.Game.FindSystem.Handler.PieceMove;
using InGame.MySystem.Game.FindSystem.Handler.RoadCreate;

namespace InGame.MySystem.Game.FindSystem
{
    // 작성자: 조혜찬
    // 배치 가능한 판들을 찾는 시스템 클래스
    public class FindPlaneSystem
    {
        private FindCanPieceCreatePlaneHandler _findCanPieceCreatePlaneHandler; // 기물 생성 가능한 칸을 탐색하는 핸들러

        private FindCanPieceMovePlaneHandler _findCanPieceMovePlaneHandler; // 기물 이동 가능한 칸을 탐색하는 핸들러

        private FindCanRoadCreatePlaneHandler _findCanRoadCreatePlaneHandler; // 도로 생성 가능한 칸을 탐색하는 핸들러

        private FindCanAttackPiecesHandler _findCanAttackPiecesHandler; // 공격 가능한 기물을 탐색하는 핸들러

        private FindCanRangedAttackPiecesHandler _findCanRangedAttackPiecesHandler; // 원거리 공격 가능한 기물 탐색 핸들러

        private ResetPlanesHandler _resetPlanesHandler; // 찾은 값들을 초기화 하는 핸들러

        public FindPlaneSystem(FindCanPieceCreatePlaneHandler findCanPieceCreatePlaneHandler, FindCanPieceMovePlaneHandler findCanPieceMovePlaneHandler, FindCanRoadCreatePlaneHandler findCanRoadCreatePlaneHandler, FindCanAttackPiecesHandler findCanAttackPiecesHandler, FindCanRangedAttackPiecesHandler findCanRangedAttackPiecesHandler, ResetPlanesHandler resetPlanesHandler)
        {
            _findCanPieceCreatePlaneHandler = findCanPieceCreatePlaneHandler;
            _findCanPieceMovePlaneHandler = findCanPieceMovePlaneHandler;
            _findCanRoadCreatePlaneHandler = findCanRoadCreatePlaneHandler;
            _findCanAttackPiecesHandler = findCanAttackPiecesHandler;
            _findCanRangedAttackPiecesHandler = findCanRangedAttackPiecesHandler;
            _resetPlanesHandler = resetPlanesHandler;
        }

        // 기물 생성 가능한 기물 칸을 찾는 함수 - 기물 생성 버튼을 누르면 실행
        public void FindCanPieceCreatePlane(TeamType type)
        {
            _findCanPieceCreatePlaneHandler.FindCanPieceCreatePlane(type);
        }

        // 움직일 수 있는 칸을 찾는 함수 - 기물을 누르면 실행
        public void FindCanPieceMovePlane(PiecePlacePlaneObject piece, TeamType teamType, ObjectType currentPieceType)
        {
            _findCanPieceMovePlaneHandler.FindCanPieceMovePlane(piece, teamType, currentPieceType);
        }

        // 도로 생성 가능한 칸들을 찾는 함수 - 도로 생성 버튼을 누르면 실행
        public void FindCanRoadCreatePlane(TeamType type)
        {
            _findCanRoadCreatePlaneHandler.FindCanRoadCreatePlane(type);
        }

        // 찾은 값들을 초기화하는 함수
        public void ResetFindPlanes()
        {
            _resetPlanesHandler.ResetFindPlanes();
        }

        // 공격 가능한 기물들을 탐색하는 함수 - 기물을 눌렀을 때 실행
        public void FindCanAttackPieces(PieceBase pieceBase)
        {
            _findCanAttackPiecesHandler.FindCanAttackPieces(pieceBase);
        }

        // 화력을 사용해 원거리 공격 가능한 기물 탐색 함수 - 전차를 눌렀을 때 실행
        public void FindCanFirePowerAttackPiece(TeamType teamType, PiecePlacePlaneObject piece)
        {
            _findCanRangedAttackPiecesHandler.FindCanRangedAttackPieces(teamType, piece);
        }
    }
}
// 마지막 작성 일자: 2026.05.05