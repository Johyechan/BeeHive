using InGame.MyEnum;
using InGame.MyManager.Local;
using InGame.MyObject;
using InGame.MyObject.Piece;
using System.Collections.Generic;

namespace InGame.MySystem.Game.FindSystem.Handler.PieceAttack
{
    // 작성자: 조혜찬
    // 원거리 공격 가능한 기물 탐색 핸들러
    public class FindCanRangedAttackPiecesHandler
    {
        // 원거리 공격 가능한 기물 탐색 함수
        public void FindCanRangedAttackPieces(TeamType teamType, PiecePlacePlaneObject piece)
        {
            PieceBase pieceBase = piece.PlacedPiece;

            if (!pieceBase) // 기물이 존재하지 않는다면
                return;

            if (!InGameContext.Current.Data.PieceManager.CanAttackPieceMap.ContainsKey(pieceBase)) // 현재 공격 하는 기물의 공격 대상이 저장되지 않았다면
            {
                InGameContext.Current.Data.PieceManager.CanAttackPieceMap.Add(pieceBase, new List<PieceBase>()); // 맵에 새롭게 추가
            }

            if (!InGameContext.Current.Data.PieceManager.CanFirePowerAttackPieceMap.ContainsKey(pieceBase))// 현재 공격 하는 기물의 원거리 공격 대상이 저장되지 않았다면
            {
                InGameContext.Current.Data.PieceManager.CanFirePowerAttackPieceMap.Add(pieceBase, new List<PieceBase>());
            }

            foreach (var nearRoad in piece.nearRoadPlaceTransformList)
            {
                foreach (var nearPiece in nearRoad.nearPiecePlaceTransformList)
                {
                    if (nearPiece == piece) // 자기 자신이라면
                        continue; // 넘기기

                    if (!nearPiece.isNearToCastle) // 성 주위 배치칸이 아닐 때만
                    {
                        if (nearPiece.TeamType == teamType || nearPiece.TeamType == TeamType.None) // 공격하려는 전차 기물의 팀이거나 빈 칸이라면
                        {
                            continue; // 넘기기
                        }
                    }
                    else // 성 주위 배치칸이라면
                    {
                        if (nearPiece.currentPlayerTeamType != teamType) // 상대 팀의 성 주위 배치칸이라면
                        {
                            if (nearPiece.PlacedPiece == null) // 성 주위 배치칸이 비어있다면
                            {
                                if (!InGameContext.Current.Data.PlacePlaneManager.Variable.highLightHandler.CanPieceMovePlanes[pieceBase].Contains(nearPiece)) // 이동 가능한 위치가 아닐 때
                                {
                                    if (!InGameContext.Current.Data.PieceManager.CanFirePowerAttackPiecePlaceMap.ContainsKey(pieceBase)) // 현재 기물의 원거리 공격 대상을 저장하지 않았다면
                                    {
                                        InGameContext.Current.Data.PieceManager.CanFirePowerAttackPiecePlaceMap.Add(pieceBase, new List<PiecePlacePlaneObject>()); // 새 맵 추가
                                    }

                                    InGameContext.Current.Data.PieceManager.CanFirePowerAttackPiecePlaceMap[pieceBase].Add(nearPiece); // 화력 공격 가능한 기물 배치칸으로 저장
                                }
                            }
                        }
                    }

                    if (nearPiece.PlacedObjectType == ObjectType.None) // 배치된 기물이 없다면
                    {
                        continue; // 넘기기
                    }

                    // 근접 공격으로 공격 가능한 대상이라면
                    if (InGameContext.Current.Data.PieceManager.CanAttackPieceMap[pieceBase].Contains(nearPiece.PlacedPiece))
                    {
                        continue; // 넘기기
                    }

                    if (nearPiece.TeamType != pieceBase.CurrentTeamType) // 상대 팀이라면
                    {
                        if (nearPiece.PlacedPiece != null) // 기물이 존재한다면
                        {
                            // 공격 가능한 기물 중에 일치하는 기물이 없을 경우
                            if (!InGameContext.Current.Data.PieceManager.CanFirePowerAttackPieceMap[pieceBase].Contains(nearPiece.PlacedPiece))
                            {
                                InGameContext.Current.Data.PieceManager.CanFirePowerAttackPieceMap[pieceBase].Add(nearPiece.PlacedPiece);
                            }
                        }
                    }
                }
            }
        }
    }
}
// 마지막 작성 일자: 2026.05.05