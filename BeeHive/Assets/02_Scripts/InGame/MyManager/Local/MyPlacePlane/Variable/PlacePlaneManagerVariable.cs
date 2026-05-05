using InGame.MyManager.MyPlacePlane.Handler;
using InGame.MySystem.Game;
using InGame.MySystem.Game.FindSystem;
using InGame.MySystem.Game.FindSystem.Handler;
using InGame.MySystem.Game.FindSystem.Handler.PieceAttack;
using InGame.MySystem.Game.FindSystem.Handler.PieceCreate;
using InGame.MySystem.Game.FindSystem.Handler.PieceMove;
using InGame.MySystem.Game.FindSystem.Handler.RoadCreate;

namespace InGame.MyManager.MyPlacePlane.Variable
{
    // 작성자: 조혜찬
    // PlacePlaneManager 변수 모음 클래스
    public class PlacePlaneManagerVariable
    {
        public PlacePlaneMap placePlaneMap; // 전체 기물 판을 가지는 클래스 변수

        public HighLightHandler highLightHandler; // 하이라이트를 키고 끄는 기능을 가지는 클래스 변수

        public FindPlaneSystem findCanPlacePlaneSystem; // 배치 가능한 배치 판들을 찾는 시스템 클래스 변수

        public PlacePlaneStateHandler placePlaneStateHandler; // 배치 칸의 상태를 관리하는 핸들러

        public SetNearRoadHandler setNearRoadHandler; // 주위 도로 세팅 핸들러

        public FindCanPieceCreatePlaneHandler findCanPieceCreatePlaneHandler; // 기물 생성 가능한 칸을 탐색하는 핸들러

        public FindCanPieceMovePlaneHandler findCanPieceMovePlaneHandler; // 기물 이동 가능한 칸을 탐색하는 핸들러

        public FindCanRoadCreatePlaneHandler findCanRoadCreatePlaneHandler; // 도로 생성 가능한 칸을 탐색하는 핸들러

        public FindCanAttackPiecesHandler findCanAttackPiecesHandler; // 공격 가능한 기물을 탐색하는 핸들러

        public FindCanRangedAttackPiecesHandler findCanRangedAttackPiecesHandler; // 원거리 공격 가능한 기물 탐색 핸들러

        public ResetPlanesHandler resetPlanesHandler; // 찾은 값들을 초기화 하는 핸들러

        private FindPlanesUtil _findPlanesUtil; // 탐색에 필요한 기능을 가지는 클래스

        public void Init()
        {
            _findPlanesUtil = new FindPlanesUtil();

            findCanPieceCreatePlaneHandler = new FindCanPieceCreatePlaneHandler();
            findCanPieceMovePlaneHandler = new FindCanPieceMovePlaneHandler(_findPlanesUtil);
            findCanRoadCreatePlaneHandler = new FindCanRoadCreatePlaneHandler();
            findCanAttackPiecesHandler = new FindCanAttackPiecesHandler();
            findCanRangedAttackPiecesHandler = new FindCanRangedAttackPiecesHandler();
            resetPlanesHandler = new ResetPlanesHandler();

            placePlaneMap = new PlacePlaneMap();
            highLightHandler = new HighLightHandler();
            findCanPlacePlaneSystem = new FindPlaneSystem(findCanPieceCreatePlaneHandler, findCanPieceMovePlaneHandler, findCanRoadCreatePlaneHandler, findCanAttackPiecesHandler, findCanRangedAttackPiecesHandler, resetPlanesHandler);
            placePlaneStateHandler = new PlacePlaneStateHandler();
            setNearRoadHandler = new SetNearRoadHandler();
        }
    }
}
// 마지막 작성 일자: 2026.05.05