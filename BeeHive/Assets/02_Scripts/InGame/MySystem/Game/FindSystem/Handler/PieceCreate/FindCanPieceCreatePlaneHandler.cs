using InGame.MyEnum;
using InGame.MyManager.Local;

namespace InGame.MySystem.Game.FindSystem.Handler.PieceCreate
{
    // 작성자: 조혜찬
    // 기물 생성이 가능한 칸을 탐색하는 핸들러
    public class FindCanPieceCreatePlaneHandler
    {
        // 기물 생성이 가능한 칸을 탐색하는 함수(기물을 생성하려는 팀)
        public void FindCanPieceCreatePlane(TeamType type)
        {
            foreach (var piece in InGameContext.Current.Data.PlacePlaneManager.Variable.placePlaneMap.PiecePlacePlanes) // 전체 기물 판 순회
            {
                if (piece.isNearToCastle && piece.currentPlayerTeamType == type) // 성과 인접한 배치 판이면서 같은 팀일 경우
                {
                    if (piece.PlacedObjectType == ObjectType.None) // 해당 위치에 아무것도 올라와 있지 않을 때
                    {
                        if (!InGameContext.Current.Data.PlacePlaneManager.Variable.highLightHandler.CanPiecePlacePlanes.Contains(piece))
                            InGameContext.Current.Data.PlacePlaneManager.Variable.highLightHandler.CanPiecePlacePlanes.Add(piece); // 배치가 가능한 기물 배치 칸 저장
                    }
                }
            }
        }
    }
}
// 마지막 작성 일자: 2026.05.05