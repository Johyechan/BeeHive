using InGame.MyEnum;
using InGame.MyManager.Global;
using InGame.MyManager.Local;
using InGame.MyObject;
using InGame.MyObject.Piece;
using System.Collections.Generic;

namespace InGame.MySystem.Game
{
    // 작성자: 조혜찬
    // 배치 가능한 판들을 찾는 시스템 클래스
    public class FindCanPlacePlaneSystem
    {
        private List<RoadPlacePlaneObject> _nearToCastleRoadPlacePlanes = new List<RoadPlacePlaneObject>();

        // 배치 가능한 기물 칸들을 찾는 함수
        public void FindCanPlacePiecePlane(TeamType type)
        {
            foreach (var piece in InGameContext.Current.Data.PlacePlaneManager.Variable.placePlaneMap.PiecePlacePlanes) // 전체 기물 판 순회
            {
                if (piece.isNearToCastle && piece.currentPlayerTeamType == type) // 성과 인접한 배치 판이면서 같은 팀일 경우
                {
                    if(piece.PlacedObjectType == ObjectType.None) // 해당 위치에 아무것도 올라와 있지 않을 때
                    {
                        if(!InGameContext.Current.Data.PlacePlaneManager.Variable.highLightHandler.CanPiecePlacePlanes.Contains(piece))
                            InGameContext.Current.Data.PlacePlaneManager.Variable.highLightHandler.CanPiecePlacePlanes.Add(piece); // 배치가 가능한 기물 배치 칸 저장
                    }
                }
            }
        }

        // 움직일 수 있는 칸을 찾는 함수
        public void FindCanMovePlacePlane(PiecePlacePlaneObject piece, TeamType teamType, ObjectType currentPieceType)
        {
            bool findTeamRoad = false;

            ResetPlacePlanes(false); // 전체 도로 및 기물 칸 접근 여부 false로 초기화 - 이동 가능한 칸을 찾기 위함
            InGameContext.Current.Data.PlacePlaneManager.Variable.highLightHandler.CanPieceMovePlanes.Clear(); // 기물 이동 가능한 판 저장 컨테이너 비우기 - 이전에 저장했던 이동 가능한 판들을 초기화
            InGameContext.Current.Data.PlacePlaneManager.Variable.highLightHandler.CanDigCheckPlacePlanes.Clear(); // 생산 가능 확인에 필요한 판 저장 컨테이너 비우기

            foreach (var nearRoad in piece.nearRoadPlaceTransformList) // 해당 기물 칸 주위 도로 칸 순회
            {
                if(nearRoad.TeamType == teamType && nearRoad.PlacedObjectType == ObjectType.Road) // 내 도로가 있다면
                {
                    findTeamRoad = true;
                    break; // 반복문 나가기
                }
            }

            foreach (var nearRoad in piece.nearRoadPlaceTransformList) // 현재 기물의 근접한 도로 순회
            {
                HashSet<RoadPlacePlaneObject> roadVisited = new HashSet<RoadPlacePlaneObject>();
                HashSet<PiecePlacePlaneObject> pieceVisited = new HashSet<PiecePlacePlaneObject>();
                FindNearPieces(teamType, nearRoad, roadVisited, pieceVisited, !findTeamRoad, currentPieceType); // 근접한 기물 찾기 - 팀 도로를 찾았다면 쭉 탐색, 팀 도로를 찾지 못했다면 한 번만 탐색
            }
        }

        // 배치 가능한 도로 칸들을 찾는 함수
        public void FindCanPlaceRoadPlane(TeamType type)
        {
            foreach (var road in InGameContext.Current.Data.PlacePlaneManager.Variable.placePlaneMap.RoadPlacePlanes) // 전체 도로 판 순회
            {
                if (road.TeamType == type && road.PlacedObjectType != ObjectType.None) // 도로 칸의 팀 타입이 현재 탐색 중인 팀 타입이며 빈 곳이 아니라면
                {
                    HashSet<RoadPlacePlaneObject> roadVisited = new HashSet<RoadPlacePlaneObject>();
                    HashSet<PiecePlacePlaneObject> pieceVisited = new HashSet<PiecePlacePlaneObject>();
                    FindNearPieces(type, road, roadVisited, pieceVisited); // 배치가 가능한 도로 배치 칸 저장 후 인접한 기물 탐색
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

                    if(road.PlacedObjectType == ObjectType.None) // 아무것도 올라와 있지 않은 상태 일때
                    {
                        if(road.currentPlayerTeamType == type) // 팀 타입이 도로 탐색을 하는 팀과 같을 경우
                        {
                            if (!InGameContext.Current.Data.PlacePlaneManager.Variable.highLightHandler.CanRoadPlacePlanes.Contains(road))
                                InGameContext.Current.Data.PlacePlaneManager.Variable.highLightHandler.CanRoadPlacePlanes.Add(road); // 배치가 가능한 도로 배치 칸 저장
                        }
                    }
                }
            }

            FindCanPlaceRoadPlane(); // 배치 가능한 도로칸 탐색 
        }

        // 배치 판 확인 여부 초기화 함수(완전 초기화 할지 여부)
        public void ResetPlacePlanes(bool isClear = true)
        {
            foreach (var piece in InGameContext.Current.Data.PlacePlaneManager.Variable.placePlaneMap.PiecePlacePlanes) // 전체 기물 판 순회
            {
                piece.IsRangeAttackTarget = false;
            }

            if (isClear)
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

        // 공격 가능한 기물들을 탐색하는 함수
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

            foreach(var nearPiece in road.nearPiecePlaceTransformList)
            {
                if(road.TeamType == selectPiece.CurrentTeamType) // 도로가 내 팀일 때
                {
                    CheckCanAttackPiece(selectPiece, nearPiece, roadVisited, pieceVisited);
                }
                else // 도로가 내 팀이 아니거나 비어있을 때
                {
                    if(selectPiece.CurrentObjectType == ObjectType.Soldier) // 선택된 기물이 보병일 경우
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

            if(piece.PlacedPiece != null)
            {
                if(piece.TeamType != selectPiece.CurrentTeamType) // 기물 배치 칸이 다른 팀이 점령 중이라면
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

            foreach(var nearRoad in piece.nearRoadPlaceTransformList)
            {
                if(nearRoad.PlacedObjectType != ObjectType.None) // 배치된 도로가 존재하고
                {
                    if (nearRoad.TeamType == selectPiece.CurrentTeamType) // 해당 도로가 내 도로 일 때
                    {
                        FindPieces(selectPiece, nearRoad, roadVisited, pieceVisited);
                    }
                }
            }
        }

        // 배치 가능한 도로 칸을 추가하고 그 도로에 인접한 기물들을 찾는 함수(팀 타입, 도로 칸, 한 번만 검사할지)
        private void FindNearPieces(TeamType teamType, RoadPlacePlaneObject road, HashSet<RoadPlacePlaneObject> roadVisited, HashSet<PiecePlacePlaneObject> pieceVisited, bool once = false, ObjectType currentObjType = ObjectType.None)
        {
            if (roadVisited.Contains(road)) // 이미 방문했었던 도로 배치 칸이라면
                return; // 반환

            roadVisited.Add(road); // 방문한 도로 배치 칸으로 추가

            foreach (var nearPiece in road.nearPiecePlaceTransformList) // 인접한 기물 확인
            {
                if (nearPiece.PlacedObjectType == ObjectType.None) // 빈 칸이라면
                {
                    if (currentObjType == ObjectType.Miner) // 현재 기물이 광부 객체일 때
                    {
                        if(road.TeamType == TeamManager.Instance.CurrentTeamType) // 도로가 내 팀 도로라면
                        {
                            InGameContext.Current.Data.PlacePlaneManager.Variable.highLightHandler.CanDigCheckPlacePlanes.Add(nearPiece); // 생산 가능 여부 확인 배치칸으로 추가
                        }

                        if(CheckNearRoad(teamType, road)) // 자기 팀의 도로가 있거나 비어 있다면
                        {
                            InGameContext.Current.Data.PlacePlaneManager.Variable.highLightHandler.CanPieceMovePlanes.Add(nearPiece); // 이동 가능한 기물 배치 칸 추가
                        }
                    }
                    else if(currentObjType == ObjectType.Soldier) // 보병일 경우
                    {
                        InGameContext.Current.Data.PlacePlaneManager.Variable.highLightHandler.CanPieceMovePlanes.Add(nearPiece); // 이동 가능한 기물 배치 칸 추가
                    }
                    else if (currentObjType == ObjectType.Tank) // 전차일 경우
                    {
                        if (CheckNearRoad(teamType, road)) // 자기 팀의 도로가 있거나 비어 있다면
                        {
                            InGameContext.Current.Data.PlacePlaneManager.Variable.highLightHandler.CanPieceMovePlanes.Add(nearPiece); // 이동 가능한 기물 배치 칸 추가
                        }
                    }
                    else // None 상태일 경우(기본 이동 가능 위치)
                    {
                        if(road.TeamType == TeamManager.Instance.CurrentTeamType) // 도로가 내 팀의 도로라면
                        {
                            InGameContext.Current.Data.PlacePlaneManager.Variable.highLightHandler.CanPieceMovePlanes.Add(nearPiece); // 이동 가능한 기물 배치 칸 추가

                            InGameContext.Current.Data.PlacePlaneManager.Variable.highLightHandler.CanDigCheckPlacePlanes.Add(nearPiece); // 생산 가능 여부 확인 배치칸으로 추가
                        }
                    }
                }
                else // 빈 칸이 아니라면 - 즉 내 팀에 속한 기물이 올려져 있다면
                {
                    if(road.TeamType == TeamManager.Instance.CurrentTeamType) // 도로의 팀 타입이 내 팀이라면
                        InGameContext.Current.Data.PlacePlaneManager.Variable.highLightHandler.CanDigCheckPlacePlanes.Add(nearPiece); // 생산 가능 여부 확인 배치칸으로 추가
                }

                if (!once) // 한 번만 확인하는 게 아닐 경우
                    FindNearRoads(teamType, nearPiece, roadVisited, pieceVisited); // 해당 기물 칸의 인접한 도로만 탐색
            }
        }

        public void FindCanFirePowerAttackPiece(TeamType teamType, PiecePlacePlaneObject piece)
        {
            PieceBase pieceBase = piece.PlacedPiece;

            if (!pieceBase) // 기물이 존재하지 않는다면
                return;

            if(!InGameContext.Current.Data.PieceManager.CanAttackPieceMap.ContainsKey(pieceBase)) // 현재 공격 하는 기물의 공격 대상이 저장되지 않았다면
            {
                InGameContext.Current.Data.PieceManager.CanAttackPieceMap.Add(pieceBase, new List<PieceBase>()); // 맵에 새롭게 추가
            }

            if(!InGameContext.Current.Data.PieceManager.CanFirePowerAttackPieceMap.ContainsKey(pieceBase))// 현재 공격 하는 기물의 원거리 공격 대상이 저장되지 않았다면
            {
                InGameContext.Current.Data.PieceManager.CanFirePowerAttackPieceMap.Add(pieceBase, new List<PieceBase>());
            }

            foreach (var nearRoad in piece.nearRoadPlaceTransformList)
            {
                foreach(var nearPiece in nearRoad.nearPiecePlaceTransformList)
                {
                    if (nearPiece == piece) // 자기 자신이라면
                        continue; // 넘기기

                    if(!nearPiece.isNearToCastle) // 성 주위 배치칸이 아닐 때만
                    {
                        if (nearPiece.TeamType == teamType || nearPiece.TeamType == TeamType.None) // 공격하려는 전차 기물의 팀이거나 빈 칸이라면
                        {
                            continue; // 넘기기
                        }
                    }
                    else // 성 주위 배치칸이라면
                    {
                        if(nearPiece.currentPlayerTeamType != teamType) // 상대 팀의 성 주위 배치칸이라면
                        {
                            if(nearPiece.PlacedPiece == null) // 성 주위 배치칸이 비어있다면
                            {
                                if(!InGameContext.Current.Data.PlacePlaneManager.Variable.highLightHandler.CanPieceMovePlanes.Contains(nearPiece)) // 이동 가능한 위치가 아닐 때
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

                    if(nearPiece.TeamType != pieceBase.CurrentTeamType) // 상대 팀이라면
                    {
                        if(nearPiece.PlacedPiece != null) // 기물이 존재한다면
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

        private bool CheckNearRoad(TeamType teamType, RoadPlacePlaneObject road)
        {
            if (road.TeamType == teamType || road.TeamType == TeamType.None) // 현재 기물의 위치와 팀 타입이 같거나 비어있다면
            {
                return true;
            }
            else // 다른 팀일 경우
            {
                return false;
            }
        }

        // 배치 가능한 기물 칸을 추가하고 그 기물에 인접한 도로들을 찾는 함수(팀 타입, 기물 칸, 한 번만 검사할지)
        private void FindNearRoads(TeamType teamType, PiecePlacePlaneObject piece, HashSet<RoadPlacePlaneObject> roadVisited, HashSet<PiecePlacePlaneObject> pieceVisited, bool once = false)
        {
            if (pieceVisited.Contains(piece)) // 이미 방문했었던 기물 배치 칸이라면
                return; // 반환

            pieceVisited.Add(piece); // 방문한 기물 배치 칸으로 추가
         
            bool isExist = false; // 기물 주위에 teamType의 도로가 존재하는지 여부

            foreach(var nearRoad in piece.nearRoadPlaceTransformList) // 인접한 도로 확인
            {
                if(nearRoad.TeamType == teamType)
                {
                    isExist = true;
                    break;
                }
            }

            if(isExist) // 인접한 도로에 teamType의 도로가 있다면
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
                if (nearRoad.PlacedObjectType != ObjectType.None) // 빈 칸이 아니라면 - 즉 내 도로 기물이 올라가 있다면
                {
                    if (!once) // 한 번만 확인하는 게 아닐 경우
                        FindNearPieces(teamType, nearRoad, roadVisited, pieceVisited); // 해당 도로 칸의 인접한 기물만 탐색
                }
            }
        }

        // 배치 가능한 도로 칸 탐색 함수
        private void FindCanPlaceRoadPlane()
        {
            foreach(var nearRoad in _nearToCastleRoadPlacePlanes) // 성에 근접한 도로 배치칸 탐색
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

            foreach(var nearRoad in piecePlacePlane.nearRoadPlaceTransformList)
            {
                ChangeRoadPlacePlaneConnection(nearRoad, roadVisited, pieceVisited);
            }
        }
    }
}
// 마지막 작성 일자: 2026.05.04