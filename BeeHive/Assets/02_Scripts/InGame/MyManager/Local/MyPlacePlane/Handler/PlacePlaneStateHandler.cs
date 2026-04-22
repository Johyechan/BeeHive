using InGame.MyEnum;
using InGame.MyObject;
using InGame.MyObject.Piece;

namespace InGame.MyManager.MyPlacePlane.Handler
{
    // 작성자: 조혜찬
    // 배치칸의 상태를 관리하는 핸들러
    public class PlacePlaneStateHandler
    {
        // 배치 칸 상태 변경 함수(상태 변경될 배치칸, 배치할 기물, 이동 여부)
        public void ChangePlacePlaneState(PlacePlaneObjectBase currentPlacePlane, PieceBase placedPiece, bool isMove)
        {
            if (isMove) // 이동일 경우
            {
                // 배치된 기물의 이전 배치 칸 초기화
                placedPiece.PieceVariable.currentPlacePlane.PlacedObjectType = ObjectType.None;
                placedPiece.PieceVariable.currentPlacePlane.TeamType = TeamType.None; // 배치 칸에서 판단하는 위에 올려진 기물 팀 초기화
                placedPiece.PieceVariable.currentPlacePlane.PlacedPiece = null;
            }

            bool isRoad = currentPlacePlane is RoadPlacePlaneObject; // 도로 배치칸인지 확인하기 위한 변수

            currentPlacePlane.PlacedObjectType = placedPiece.CurrentObjectType; // 배치된 기물의 객체 타입 할당
            currentPlacePlane.PlacedPiece = placedPiece;
            currentPlacePlane.TeamType = placedPiece.CurrentTeamType; // 배치된 기물의 팀 타입 할당

            if (!isRoad) // 도로 배치칸을 변경하는 것이 아닌 기물 배치칸을 변경하는 것이라면
            {
                PiecePlacePlaneObject piecePlacePlane = (PiecePlacePlaneObject)currentPlacePlane;
                placedPiece.PieceVariable.currentPlacePlane = piecePlacePlane; // 기물 전용 배치칸 할당
            }
            else // 도로 배치칸을 변경하는 것이라면
            {
                RoadPlacePlaneObject roadPlacePlane = (RoadPlacePlaneObject)currentPlacePlane;
                placedPiece.PieceVariable.currentRoadPlacePlane = roadPlacePlane; // 도로 전용 배치칸 할당
            }
        }
    }
}
// 마지막 작성 일자: 2026.04.22