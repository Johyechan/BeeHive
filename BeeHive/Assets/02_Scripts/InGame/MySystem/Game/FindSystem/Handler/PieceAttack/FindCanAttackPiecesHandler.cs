using InGame.MyEnum;
using InGame.MyManager.Global;
using InGame.MyManager.Local;
using InGame.MyObject;
using InGame.MyObject.Piece;
using System.Collections.Generic;

namespace InGame.MySystem.Game.FindSystem.Handler.PieceAttack
{
    // 작성자: 조혜찬
    // 공격 가능한 기물 탐색 핸들러
    public class FindCanAttackPiecesHandler
    {
        // 공격 가능한 기물 탐색 함수
        public void FindCanAttackPieces(PieceBase pieceBase)
        {
            PiecePlacePlaneObject piecePlacePlane = pieceBase.PieceVariable.currentPlacePlane;

            if (!InGameContext.Current.Data.PieceManager.CanAttackPieceMap.ContainsKey(pieceBase)) // 현재 선택된 기물의 공격 대상을 저장하지 않았다면
            {
                InGameContext.Current.Data.PieceManager.CanAttackPieceMap.Add(pieceBase, new List<PieceBase>()); // 새로운 값 추가
            }

            foreach (var nearRoad in piecePlacePlane.nearRoadPlaceTransformList)
            {
                HashSet<RoadPlacePlaneObject> roadVisited = new HashSet<RoadPlacePlaneObject>();
                HashSet<PiecePlacePlaneObject> pieceVisited = new HashSet<PiecePlacePlaneObject>();
                FindPieces(pieceBase, nearRoad, roadVisited, pieceVisited);
            }
        }
        // 도로 주위 기물칸을 찾는 함수
        private void FindPieces(PieceBase selectPiece, RoadPlacePlaneObject road, HashSet<RoadPlacePlaneObject> roadVisited, HashSet<PiecePlacePlaneObject> pieceVisited)
        {
            if (roadVisited.Contains(road)) // 이미 방문했었던 도로 배치 칸이라면
                return; // 반환

            roadVisited.Add(road); // 방문한 도로 배치 칸으로 추가

            foreach (var nearPiece in road.nearPiecePlaceTransformList)
            {
                NetworkManager.Instance.Socket.Emit("debug", $"현재 기물: {nearPiece}, 현재 기물 팀: {nearPiece.TeamType}");
                if (road.TeamType == selectPiece.CurrentTeamType) // 도로가 내 팀일 때
                {
                    CheckCanAttackPiece(selectPiece, nearPiece, roadVisited, pieceVisited);
                }
                else // 도로가 내 팀이 아니거나 비어있을 때
                {
                    if (selectPiece.CurrentObjectType == ObjectType.Soldier) // 선택된 기물이 보병일 경우
                    {
                        CheckCanAttackPiece(selectPiece, nearPiece, roadVisited, pieceVisited, true);
                    }
                }
            }
        }

        // 공격 가능한 기물을 찾는 함수
        private void CheckCanAttackPiece(PieceBase selectPiece, PiecePlacePlaneObject piece, HashSet<RoadPlacePlaneObject> roadVisited, HashSet<PiecePlacePlaneObject> pieceVisited, bool notMyRoad = false)
        {
            if (pieceVisited.Contains(piece)) // 이미 방문했었던 도로 배치 칸이라면
                return; // 반환

            pieceVisited.Add(piece); // 방문한 도로 배치 칸으로 추가

            if (piece.PlacedPiece != null)
            {
                if (piece.PlacedPiece.CurrentTeamType != selectPiece.CurrentTeamType) // 현재 확인하는 기물과 현재 선택된 기물의 팀이 다르다면
                {
                    if (!InGameContext.Current.Data.PieceManager.CanAttackPieceMap[selectPiece].Contains(piece.PlacedPiece)) // 중복 확인
                    {
                        if (piece.PlacedPiece.CurrentObjectType != ObjectType.Tank) // 근접한 기물 타일에 배치되어있는 기물이 전차가 아닐 경우
                        {
                            InGameContext.Current.Data.PieceManager.CanAttackPieceMap[selectPiece].Add(piece.PlacedPiece); // 전차의 공격 대상으로 추가
                        }
                    }
                }
            }

            if (notMyRoad) // 내 도로가 아니라면 
                return; // 반환

            foreach (var nearRoad in piece.nearRoadPlaceTransformList)
            {
                if (nearRoad.PlacedObjectType != ObjectType.None) // 배치된 도로가 존재하고
                {
                    if (nearRoad.TeamType == selectPiece.CurrentTeamType) // 해당 도로가 내 도로 일 때
                    {
                        FindPieces(selectPiece, nearRoad, roadVisited, pieceVisited);
                    }
                }
            }
        }
    }
}
// 마지막 작성 일자: 2026.05.05