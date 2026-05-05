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
                if (road.TeamType == type && road.PlacedObjectType != ObjectType.None) // 도로 칸의 팀 타입이 현재 탐색 중인 팀 타입이며 빈 곳이 아니라면
                {
                    HashSet<RoadPlacePlaneObject> roadVisited = new HashSet<RoadPlacePlaneObject>();
                    HashSet<PiecePlacePlaneObject> pieceVisited = new HashSet<PiecePlacePlaneObject>();
                    CheckRoad(type, road, roadVisited, pieceVisited); // 배치가 가능한 도로 배치 칸 저장 후 인접한 기물 탐색
                }
                else if (road.isNearToCastle) // 성과 인접한 배치 판이라면
                {
                    if (road.currentPlayerTeamType == type) // 도로 배치 칸이 내 팀 주위 배치 칸일 때
                    {
                        if (!_nearToCastleRoadPlacePlanes.Contains(road))
                        {
                            _nearToCastleRoadPlacePlanes.Add(road);
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

        // 기물 확인 함수
        private void CheckPiece(TeamType teamType, PiecePlacePlaneObject piece, HashSet<RoadPlacePlaneObject> roadVisited, HashSet<PiecePlacePlaneObject> pieceVisited)
        {
            if (pieceVisited.Contains(piece)) // 이미 방문했었던 기물 배치 칸이라면
                return; // 반환

            pieceVisited.Add(piece); // 방문한 기물 배치 칸으로 추가

            bool isExist = false; // 기물 주위에 teamType의 도로가 존재하는지 여부

            foreach (var nearRoad in piece.nearRoadPlaceTransformList) // 인접한 도로 확인
            {
                if (nearRoad.TeamType == teamType)
                {
                    isExist = true;
                    break;
                }
            }

            if (isExist) // 인접한 도로에 teamType의 도로가 있다면
            {
                foreach (var nearRoad in piece.nearRoadPlaceTransformList) // 인접한 도로 확인
                {
                    if (nearRoad.TeamType != teamType && nearRoad.TeamType != TeamType.None) // 도로가 올려져 있고 다른 팀 도로가 올려져 있다면
                    {
                        if (!InGameContext.Current.Data.PieceManager.CanChangeRoadList.Contains(nearRoad.PlacedPiece)) // 이전에 저장했던 도로가 아닐 경우
                        {
                            InGameContext.Current.Data.PieceManager.CanChangeRoadList.Add(nearRoad.PlacedPiece); // 도로 추가
                        }
                    }
                }
            }

            foreach (var nearRoad in piece.nearRoadPlaceTransformList) // 인접한 도로 확인
            {
                if (nearRoad.PlacedObjectType != ObjectType.None) // 빈 칸이 아니라면
                {
                    if (nearRoad.TeamType == TeamManager.Instance.CurrentTeamType) // 도로가 내 도로일 경우
                    {
                        CheckRoad(teamType, nearRoad, roadVisited, pieceVisited); // 해당 도로 칸의 인접한 기물만 탐색
                    }
                }
            }
        }

        // 도로 확인 함수
        private void CheckRoad(TeamType teamType, RoadPlacePlaneObject road, HashSet<RoadPlacePlaneObject> roadVisited, HashSet<PiecePlacePlaneObject> pieceVisited)
        {
            if (roadVisited.Contains(road)) // 이미 방문했었던 도로 배치 칸이라면
                return; // 반환

            roadVisited.Add(road); // 방문한 도로 배치 칸으로 추가

            foreach (var nearPiece in road.nearPiecePlaceTransformList) // 인접한 기물 확인
            {
                CheckPiece(teamType, nearPiece, roadVisited, pieceVisited); // 해당 기물 칸의 인접한 도로만 탐색
            }
        }
    }
}
// 마지막 작성 일자: 2026.05.05