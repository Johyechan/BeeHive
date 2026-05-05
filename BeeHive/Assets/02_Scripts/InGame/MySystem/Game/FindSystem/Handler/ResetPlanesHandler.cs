using InGame.MyManager.Local;

namespace InGame.MySystem.Game.FindSystem.Handler
{
    // 작성자: 조혜찬
    // 탐색해서 값을 저장해둔 컨테이너들을 초기화 하는 핸들러
    public class ResetPlanesHandler
    {
        // 찾은 값들을 초기화 하는 함수
        public void ResetFindPlanes()
        {
            InGameContext.Current.Data.PlacePlaneManager.Variable.highLightHandler.CanPiecePlacePlanes.Clear(); // 기물 배치 가능한 판 저장 컨테이너 비우기
            InGameContext.Current.Data.PlacePlaneManager.Variable.highLightHandler.CanRoadPlacePlanes.Clear(); // 도로 배치 가능한 판 저장 컨테이너 비우기
            InGameContext.Current.Data.PlacePlaneManager.Variable.highLightHandler.CanPieceMovePlanes.Clear(); // 기물 이동 가능한 판 저장 컨테이너 비우기
            InGameContext.Current.Data.PlacePlaneManager.Variable.highLightHandler.CanDigCheckPlacePlanes.Clear(); // 생산 가능 확인에 필요한 판 저장 컨테이너 비우기

            InGameContext.Current.Data.PieceManager.CanChangeRoadList.Clear(); // 도로 변형 가능한 도로 비우기

            InGameContext.Current.Data.PieceManager.CanAttackPieceMap.Clear(); // 공격 가능 기물 저장 컨테이너 비우기

            InGameContext.Current.Data.PieceManager.CanFirePowerAttackPieceMap.Clear(); // 화력 공격 가능 기물 저장 컨테이너 비우기

            InGameContext.Current.Data.PieceManager.CanFirePowerAttackPiecePlaceMap.Clear(); // 화력 공격 가능 기물 배치 칸 저장 컨테이너 비우기
        }
    }
}
// 마지막 작성 일자: 2026.05.05