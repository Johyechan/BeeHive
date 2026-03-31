using InGame.MyEnum;
using InGame.MyManager;
using InGame.MyManager.Global;
using InGame.MyManager.Local;
using InGame.MyManager.MyPiece;
using InGame.MyManager.MyPlacePlane;
using InGame.MyObject;
using InGame.MyObject.Piece.ObjectPieces;
using Tutorial;

namespace InGame.MySystem.Game
{
    // 작성자: 조혜찬
    // 배치 가능한 판들을 찾는 시스템 클래스
    public class FindCanPlacePlaneSystem
    {
        // 배치 가능한 기물 칸들을 찾는 함수
        public void FindCanPlacePiecePlane(TeamType type)
        {
            foreach (var piece in InGameContext.Current.Data.PlacePlaneManager.Variable.placePlaneMap.PiecePlacePlanes) // 전체 기물 판 순회
            {
                if (piece.isNearToCastle && piece.currentPlayerTeamType == type) // 성과 인접한 배치 판이면서 같은 팀일 경우
                {
                    piece.IsChecked = true; // 체크 한 것으로 취급
                    if(piece.PlacedObjectType == ObjectType.None) // 해당 위치에 아무것도 올라와 있지 않을 때
                    {
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
                FindNearPieces(teamType, nearRoad, !findTeamRoad, currentPieceType); // 근접한 기물 찾기 - 팀 도로를 찾았다면 쭉 탐색, 팀 도로를 찾지 못했다면 한 번만 탐색
            }
        }

        // 배치 가능한 도로 칸들을 찾는 함수
        public void FindCanPlaceRoadPlane(TeamType type)
        {
            foreach (var road in InGameContext.Current.Data.PlacePlaneManager.Variable.placePlaneMap.RoadPlacePlanes) // 전체 도로 판 순회
            {
                if (road.TeamType == type && road.PlacedObjectType != ObjectType.None) // 도로 칸의 팀 타입이 현재 탐색 중인 팀 타입이며 빈 곳이 아니라면
                {
                    FindNearPieces(type, road); // 배치가 가능한 도로 배치 칸 저장 후 인접한 기물 탐색
                }
                else if (road.isNearToCastle) // 성과 인접한 배치 판이라면
                {
                    road.IsChecked = true; // 체크 한 것으로 취급
                    if(road.PlacedObjectType == ObjectType.None) // 아무것도 올라와 있지 않은 상태 일때
                    {
                        if(road.currentPlayerTeamType == type) // 팀 타입이 도로 탐색을 하는 팀과 같을 경우
                        {
                            InGameContext.Current.Data.PlacePlaneManager.Variable.highLightHandler.CanRoadPlacePlanes.Add(road); // 배치가 가능한 도로 배치 칸 저장
                        }
                    }
                }
            }
        }

        // 배치 판 확인 여부 초기화 함수(완전 초기화 할지 여부)
        public void ResetPlacePlanes(bool isClear = true)
        {
            foreach (var piece in InGameContext.Current.Data.PlacePlaneManager.Variable.placePlaneMap.PiecePlacePlanes) // 전체 기물 판 순회
            {
                piece.IsChecked = false; // 확인하지 않은 상태로 초기화
            }

            foreach (var road in InGameContext.Current.Data.PlacePlaneManager.Variable.placePlaneMap.RoadPlacePlanes) // 전체 도로 판 순회
            {
                road.IsChecked = false; // 확인하지 않은 상태로 초기화
            }

            if(isClear)
            {
                InGameContext.Current.Data.PlacePlaneManager.Variable.highLightHandler.CanPiecePlacePlanes.Clear(); // 기물 배치 가능한 판 저장 컨테이너 비우기
                InGameContext.Current.Data.PlacePlaneManager.Variable.highLightHandler.CanRoadPlacePlanes.Clear(); // 도로 배치 가능한 판 저장 컨테이너 비우기
                InGameContext.Current.Data.PlacePlaneManager.Variable.highLightHandler.CanPieceMovePlanes.Clear(); // 기물 이동 가능한 판 저장 컨테이너 비우기
                InGameContext.Current.Data.PlacePlaneManager.Variable.highLightHandler.CanDigCheckPlacePlanes.Clear(); // 생산 가능 확인에 필요한 판 저장 컨테이너 비우기

                foreach (var piece in InGameContext.Current.Data.PieceManager.CanAttackPieceMap) // 공격 가능 기물 저장 컨테이너 순회
                    piece.Value.Clear(); // 리스트 클리어

                foreach (var piece in InGameContext.Current.Data.PieceManager.CanFirePowerAttackPieceMap) // 화력 공격 가능 기물 저장 컨테이너 순회
                    piece.Value.Clear(); // 리스트 클리어
            }
        }

        // 배치 가능한 도로 칸을 추가하고 그 도로에 인접한 기물들을 찾는 함수(팀 타입, 도로 칸, 한 번만 검사할지)
        private void FindNearPieces(TeamType teamType, RoadPlacePlaneObject road, bool once = false, ObjectType currentObjType = ObjectType.None)
        {
            road.IsChecked = true;

            foreach (var nearPiece in road.nearPiecePlaceTransformList) // 인접한 기물 확인
            {
                if (nearPiece.IsChecked) // 이미 확인을 했었다면
                    continue; // 넘기기
                else if (nearPiece.TeamType != teamType && nearPiece.TeamType != TeamType.None) // 현재 팀이 아니고 다른 팀에 속한 상태라면
                {
                    if(road.TeamType == TeamManager.Instance.CurrentTeamType) // 도로가 우리 팀 도로라면
                    {
                        if (!InGameContext.Current.Data.PieceManager.CanAttackPieceMap[ObjectType.Tank].Contains(nearPiece.PlacedPiece)) // 중복 확인
                        {
                            if(nearPiece.PlacedPiece.CurrentObjectType != ObjectType.Tank) // 근접한 기물 타일에 배치되어있는 기물이 전차가 아닐 경우
                            {
                                InGameContext.Current.Data.PieceManager.CanAttackPieceMap[ObjectType.Tank].Add(nearPiece.PlacedPiece); // 전차의 공격 대상으로 추가
                            }
                        }

                        if (!InGameContext.Current.Data.PieceManager.CanAttackPieceMap[ObjectType.Soldier].Contains(nearPiece.PlacedPiece)) // 중복 확인
                        {
                            if (nearPiece.PlacedPiece.CurrentObjectType != ObjectType.Tank) // 근접한 기물 타일에 배치되어있는 기물이 전차가 아닐 경우
                            {
                                InGameContext.Current.Data.PieceManager.CanAttackPieceMap[ObjectType.Soldier].Add(nearPiece.PlacedPiece); // 보병의 공격 대상으로 추가
                            }
                        }
                    }
                    else // 도로가 우리 팀 도로가 아닐 경우
                    {
                        if (!InGameContext.Current.Data.PieceManager.CanAttackPieceMap[ObjectType.Soldier].Contains(nearPiece.PlacedPiece)) // 중복 확인
                        {
                            if (nearPiece.PlacedPiece.CurrentObjectType != ObjectType.Tank) // 근접한 기물 타일에 배치되어있는 기물이 전차가 아닐 경우
                            {
                                InGameContext.Current.Data.PieceManager.CanAttackPieceMap[ObjectType.Soldier].Add(nearPiece.PlacedPiece); // 보병의 공격 대상으로 추가
                            }
                        }
                    }
                }

                if (nearPiece.PlacedObjectType == ObjectType.None) // 빈 칸이라면
                {
                    if(currentObjType == ObjectType.Miner) // 현재 기물이 광부 객체일 때
                    {
                        if(road.TeamType == TeamManager.Instance.CurrentTeamType) // 도로가 내 팀 도로라면
                        {
                            InGameContext.Current.Data.PlacePlaneManager.Variable.highLightHandler.CanDigCheckPlacePlanes.Add(nearPiece); // 생산 가능 여부 확인 배치칸으로 추가
                        }

                        if(CheckNearRoad(teamType, road)) // 자기 팀의 도로가 있을 경우
                        {
                            InGameContext.Current.Data.PlacePlaneManager.Variable.highLightHandler.CanPieceMovePlanes.Add(nearPiece); // 이동 가능한 기물 배치 칸 추가
                        }
                    }
                    else if(currentObjType == ObjectType.Soldier) // 보병일 경우
                    {
                        InGameContext.Current.Data.PlacePlaneManager.Variable.highLightHandler.CanPieceMovePlanes.Add(nearPiece); // 이동 가능한 기물 배치 칸 추가
                    }
                    else // None 상태일 경우(기본 이동 가능 위치)
                    {
                        if(road.TeamType == TeamManager.Instance.CurrentTeamType) // 도로가 내 팀의 도로라면
                        {
                            InGameContext.Current.Data.PlacePlaneManager.Variable.highLightHandler.CanPieceMovePlanes.Add(nearPiece); // 이동 가능한 기물 배치 칸 추가
                            InGameContext.Current.Data.PlacePlaneManager.Variable.highLightHandler.CanDigCheckPlacePlanes.Add(nearPiece); // 생산 가능 여부 확인 배치칸으로 추가
                        }
                    }

                    if (!once) // 한 번만 확인하는 게 아닐 경우
                        FindNearRoads(teamType, nearPiece); // 해당 기물 칸의 인접한 도로 탐색
                }
                else // 빈 칸이 아니라면 - 즉 내 팀에 속한 기물이 올려져 있다면
                {
                    if(road.TeamType == TeamManager.Instance.CurrentTeamType) // 도로의 팀 타입이 내 팀이라면
                        InGameContext.Current.Data.PlacePlaneManager.Variable.highLightHandler.CanDigCheckPlacePlanes.Add(nearPiece); // 생산 가능 여부 확인 배치칸으로 추가

                    if(!once) // 한 번만 확인하는 게 아닐 경우
                        FindNearRoads(teamType, nearPiece); // 해당 기물 칸의 인접한 도로만 탐색
                }
            }
        }

        public void FindCanFirePowerAttackPiece(TeamType teamType, PiecePlacePlaneObject piece)
        {
            foreach(var nearRoad in piece.nearRoadPlaceTransformList)
            {
                foreach(var nearPiece in nearRoad.nearPiecePlaceTransformList)
                {
                    if (nearPiece == piece) // 자기 자신이라면
                        continue; // 넘기기

                    if (nearPiece.TeamType == teamType || nearPiece.TeamType == TeamType.None) // 공격하려는 전차 기물의 팀이거나 빈 칸이라면
                    {
                        continue; // 넘기기
                    }

                    // 근접 공격으로 공격 가능한 대상이라면
                    if (InGameContext.Current.Data.PieceManager.CanAttackPieceMap[ObjectType.Tank].Contains(nearPiece.PlacedPiece))
                    {
                        continue; // 넘기기
                    }

                    // 공격 가능한 기물 중에 일치하는 기물이 없을 경우
                    if (!InGameContext.Current.Data.PieceManager.CanFirePowerAttackPieceMap[ObjectType.Tank].Contains(nearPiece.PlacedPiece))
                    {
                        InGameContext.Current.Data.PieceManager.CanFirePowerAttackPieceMap[ObjectType.Tank].Add(nearPiece.PlacedPiece);
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
        private void FindNearRoads(TeamType teamType, PiecePlacePlaneObject piece, bool once = false)
        {
            piece.IsChecked = true;
            foreach (var nearRoad in piece.nearRoadPlaceTransformList) // 인접한 도로 확인
            {
                if (nearRoad.IsChecked) // 이미 확인을 했었다면
                     continue; // 넘기기
                else if((nearRoad.TeamType != teamType && nearRoad.TeamType != TeamType.None)) // (현재 팀이 아니면서 다른 팀이라면)
                {
                    if(!InGameContext.Current.Data.PieceManager.CanChangeRoadList.Contains(nearRoad.PlacedPiece)) // 이전에 저장했던 도로가 아닐 경우
                    {
                        InGameContext.Current.Data.PieceManager.CanChangeRoadList.Add(nearRoad.PlacedPiece); // 도로 추가
                    }
                    continue;
                }

                if (nearRoad.PlacedObjectType == ObjectType.None) // 빈 칸이라면
                {
                    InGameContext.Current.Data.PlacePlaneManager.Variable.highLightHandler.CanRoadPlacePlanes.Add(nearRoad); // 배치 가능한 도로 칸에 추가
                }
                else // 빈 칸이 아니라면 - 즉 내 도로 기물이 올라가 있다면
                {
                    if(!once) // 한 번만 확인하는 게 아닐 경우
                        FindNearPieces(teamType, nearRoad); // 해당 도로 칸의 인접한 기물만 탐색
                }
            }
        }
    }
}
// 마지막 작성 일자: 2026.02.03