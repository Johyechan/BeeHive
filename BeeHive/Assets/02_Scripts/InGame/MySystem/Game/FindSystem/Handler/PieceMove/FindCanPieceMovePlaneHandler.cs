using InGame.MyEnum;
using InGame.MyManager.Local;
using InGame.MyObject;
using InGame.MyObject.Piece;
using System.Collections.Generic;

namespace InGame.MySystem.Game.FindSystem.Handler.PieceMove
{
    // 작성자: 조혜찬
    // 기물이 이동 가능한 칸을 탐색하는 핸들러
    public class FindCanPieceMovePlaneHandler
    {
        private FindPlanesUtil _findPlanesUtil; // 값을 탐색할 때 필요한 기능들을 가지는 클래스

        public FindCanPieceMovePlaneHandler(FindPlanesUtil findPlanesUtil)
        {
            _findPlanesUtil = findPlanesUtil;
        }

        public void FindCanPieceMovePlane(PiecePlacePlaneObject piece, TeamType teamType, ObjectType currentPieceType)
        {
            PieceBase pieceBase = piece.PlacedPiece;

            // 현재 이동 가능한 위치를 찾으려는 기물을 이전에 저장한 적이 없다면
            if (!InGameContext.Current.Data.PlacePlaneManager.Variable.highLightHandler.CanPieceMovePlanes.ContainsKey(pieceBase))
            {
                InGameContext.Current.Data.PlacePlaneManager.Variable.highLightHandler.CanPieceMovePlanes.Add(pieceBase, new HashSet<PlacePlaneObjectBase>()); // 맵에 새로 추가
            }

            foreach (var nearRoad in piece.nearRoadPlaceTransformList) // 현재 기물의 근접한 도로 순회
            {
                switch (currentPieceType)
                {
                    case ObjectType.Miner:
                    case ObjectType.Tank:
                        if (_findPlanesUtil.CheckNearRoad(teamType, nearRoad)) // 주위 도로가 자신의 팀이거나 비어 있을 경우
                        {
                            foreach (var nearPiece in nearRoad.nearPiecePlaceTransformList)
                            {
                                InGameContext.Current.Data.PlacePlaneManager.Variable.highLightHandler.CanPieceMovePlanes[pieceBase].Add(nearPiece); // 이동 가능한 기물 배치 칸 추가
                            }
                        }
                        break;
                    case ObjectType.Soldier:
                        foreach (var nearPiece in nearRoad.nearPiecePlaceTransformList)
                        {
                            InGameContext.Current.Data.PlacePlaneManager.Variable.highLightHandler.CanPieceMovePlanes[pieceBase].Add(nearPiece); // 이동 가능한 기물 배치 칸 추가
                        }
                        break;
                }

                if(nearRoad.TeamType == teamType) // 주위 도로가 내 도로라면
                {
                    HashSet<RoadPlacePlaneObject> roadVisited = new HashSet<RoadPlacePlaneObject>(); // 도로 배치 칸에서 방문 여부 확인 컨테이너
                    HashSet<PiecePlacePlaneObject> pieceVisited = new HashSet<PiecePlacePlaneObject>(); // 기물 배치 칸에서 방문 여부 확인 컨테이너
                    CheckRoad(pieceBase, nearRoad, teamType, roadVisited, pieceVisited);
                }
            }
        }

        // 도로 확인 함수
        private void CheckRoad(PieceBase pieceBase, RoadPlacePlaneObject road, TeamType teamType, HashSet<RoadPlacePlaneObject> roadVisited, HashSet<PiecePlacePlaneObject> pieceVisited)
        {
            if (roadVisited.Contains(road)) // 방문 했었다면
                return; // 반환

            roadVisited.Add(road); // 방문 추가

            foreach(var nearPiece in road.nearPiecePlaceTransformList) // 도로의 주위 기물 탐색
            {
                if(nearPiece.PlacedObjectType == ObjectType.None) // 기물 위가 비어 있다면
                {
                    CheckPiece(pieceBase, nearPiece, teamType, roadVisited, pieceVisited);
                }
            }
        }

        // 기물 확인 함수
        private void CheckPiece(PieceBase pieceBase, PiecePlacePlaneObject piece, TeamType teamType, HashSet<RoadPlacePlaneObject> roadVisited, HashSet<PiecePlacePlaneObject> pieceVisited)
        {
            if (pieceVisited.Contains(piece)) // 방문 했었다면
                return; // 반환

            pieceVisited.Add(piece);

            InGameContext.Current.Data.PlacePlaneManager.Variable.highLightHandler.CanPieceMovePlanes[pieceBase].Add(piece); // 이동 가능한 기물 배치 칸 추가

            if(pieceBase.CurrentObjectType == ObjectType.Miner) // 이동 가능한 위치를 찾는 기물이 광부일 때
            {
                // 만약 현재 기물의 생산 가능 여부를 확인 한 적이 없다면
                if(!InGameContext.Current.Data.PlacePlaneManager.Variable.highLightHandler.CanDigCheckPlacePlanes.ContainsKey(pieceBase))
                {
                    InGameContext.Current.Data.PlacePlaneManager.Variable.highLightHandler.CanDigCheckPlacePlanes.Add(pieceBase, new HashSet<PlacePlaneObjectBase>()); // 맵에 새로 추가
                }
                InGameContext.Current.Data.PlacePlaneManager.Variable.highLightHandler.CanDigCheckPlacePlanes[pieceBase].Add(piece); // 생산 가능 여부 확인 배치칸으로 추가
            }

            foreach (var nearRoad in piece.nearRoadPlaceTransformList)
            {
                if(nearRoad.TeamType == teamType) // 도로가 같은 팀 도로일 때
                {
                    CheckRoad(pieceBase, nearRoad, teamType, roadVisited, pieceVisited);
                }
            }
        }
    }
}
// 마지막 작성 일자: 2026.05.06