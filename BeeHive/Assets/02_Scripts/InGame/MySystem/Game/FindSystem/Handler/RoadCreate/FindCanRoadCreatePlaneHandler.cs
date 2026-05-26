using InGame.MyEnum;
using InGame.MyManager.Global;
using InGame.MyManager.Local;
using InGame.MyObject;
using System.Collections.Generic;

namespace InGame.MySystem.Game.FindSystem.Handler.RoadCreate
{
    // 작성자: 조혜찬
    // 도로 생성 가능한 칸을 탐색하는 핸들러
    public class FindCanRoadCreatePlaneHandler
    {
        private List<RoadPlacePlaneObject> _nearToCastleRoadPlacePlanes = new List<RoadPlacePlaneObject>(); // 성 주위 도로 생성 칸 저장 리스트

        // 도로 생성 가능한 칸을 탐색하는 함수
        public void FindCanRoadCreatePlane(TeamType type)
        {
            foreach (var road in InGameContext.Current.Data.PlacePlaneManager.Variable.placePlaneMap.RoadPlacePlanes) // 전체 도로 판 순회
            {
                if (road.isNearToCastle) // 성과 인접한 배치 판이라면
                {
                    if (road.currentPlayerTeamType == type) // 도로 배치 칸이 내 팀 주위 배치 칸일 때
                    {
                        if (!_nearToCastleRoadPlacePlanes.Contains(road))
                        {
                            _nearToCastleRoadPlacePlanes.Add(road);
                        }

                        if(road.PlacedObjectType != ObjectType.None) // 도로 배치 칸이 비어있지 않고
                        {
                            if(road.PlacedPiece.CurrentTeamType != type) // 상대 팀 도로가 올라와 있다면
                            {
                                if (!InGameContext.Current.Data.PieceManager.CanChangeRoadList.Contains(road.PlacedPiece)) // 이전에 저장했던 도로가 아닐 경우
                                {
                                    InGameContext.Current.Data.PieceManager.CanChangeRoadList.Add(road.PlacedPiece); // 도로 추가
                                }
                            }
                        }
                    }

                    if (road.PlacedObjectType == ObjectType.None) // 아무것도 올라와 있지 않은 상태 일때
                    {
                        if (road.currentPlayerTeamType == type) // 팀 타입이 도로 탐색을 하는 팀과 같을 경우
                        {
                            if (!InGameContext.Current.Data.PlacePlaneManager.Variable.highLightHandler.CanRoadPlacePlanes.Contains(road))
                                InGameContext.Current.Data.PlacePlaneManager.Variable.highLightHandler.CanRoadPlacePlanes.Add(road); // 배치가 가능한 도로 배치 칸 저장
                        }
                    }
                }
            }

            FindCanPlaceRoadPlane();
        }

        // 배치 가능한 도로 칸 탐색 함수
        private void FindCanPlaceRoadPlane()
        {
            foreach (var nearRoad in _nearToCastleRoadPlacePlanes) // 성에 근접한 도로 배치칸 탐색
            {
                HashSet<RoadPlacePlaneObject> roadVisited = new HashSet<RoadPlacePlaneObject>();
                HashSet<PiecePlacePlaneObject> pieceVisited = new HashSet<PiecePlacePlaneObject>();
                ChangeRoadPlacePlaneConnection(nearRoad, roadVisited, pieceVisited);
            }
        }

        private void ChangeRoadPlacePlaneConnection(RoadPlacePlaneObject roadPlacePlane, HashSet<RoadPlacePlaneObject> roadVisited, HashSet<PiecePlacePlaneObject> pieceVisited)
        {
            if (roadVisited.Contains(roadPlacePlane)) // 이미 방문했었던 도로 배치 칸이라면
                return; // 반환 

            roadVisited.Add(roadPlacePlane); // 방문한 도로 배치 칸으로 추가

            if (roadPlacePlane.PlacedObjectType != ObjectType.None) // 배치된 도로가 있을 때
            {
                if (TeamManager.Instance.CurrentTeamType == roadPlacePlane.TeamType) // 플레이어의 도로일 때
                {
                    foreach (var nearPiece in roadPlacePlane.nearPiecePlaceTransformList)
                    {
                        ChangePiecePlacePlaneConnection(nearPiece, roadVisited, pieceVisited);
                    }
                }
                else // 상대 팀의 도로일 때
                {
                    if (!InGameContext.Current.Data.PieceManager.CanChangeRoadList.Contains(roadPlacePlane.PlacedPiece)) // 이전에 저장했던 도로가 아닐 경우
                    {
                        InGameContext.Current.Data.PieceManager.CanChangeRoadList.Add(roadPlacePlane.PlacedPiece); // 도로 추가
                    }
                }
            }
            else // 배치된 도로가 없을 때
            {
                if (!InGameContext.Current.Data.PlacePlaneManager.Variable.highLightHandler.CanRoadPlacePlanes.Contains(roadPlacePlane))
                    InGameContext.Current.Data.PlacePlaneManager.Variable.highLightHandler.CanRoadPlacePlanes.Add(roadPlacePlane); // 배치가 가능한 도로 배치 칸 저장
            }
        }

        private void ChangePiecePlacePlaneConnection(PiecePlacePlaneObject piecePlacePlane, HashSet<RoadPlacePlaneObject> roadVisited, HashSet<PiecePlacePlaneObject> pieceVisited)
        {
            if (pieceVisited.Contains(piecePlacePlane)) // 이미 방문했었던 기물 배치 칸이라면
                return; // 반환 

            pieceVisited.Add(piecePlacePlane); // 방문한 기물 배치 칸으로 추가

            foreach (var nearRoad in piecePlacePlane.nearRoadPlaceTransformList)
            {
                ChangeRoadPlacePlaneConnection(nearRoad, roadVisited, pieceVisited);
            }
        }
    }
}
// 마지막 작성 일자: 2026.05.26