using InGame.MyEnum;
using InGame.MyManager;
using InGame.MyManager.MyPiece;
using InGame.MyManager.MyPlacePlane;
using InGame.MyObject;

namespace InGame.MySystem.Game
{
    // 작성자: 조혜찬
    // 배치 가능한 판들을 찾는 시스템 클래스
    public class FindCanPlacePlaneSystem
    {
        // 배치 가능한 기물 칸들을 찾는 함수
        public void FindCanPlacePiecePlane(TeamType type)
        {
            foreach (var piece in PlacePlaneManager.Instance.PlacePlaneMap.PiecePlacePlanes) // 전체 기물 판 순회
            {
                if (piece.isNearToCastle && piece.currentPlayerTeamType == TeamManager.Instance.CurrentTeamType) // 성과 인접한 배치 판이면서 같은 팀일 경우
                {
                    piece.IsChecked = true; // 체크 한 것으로 취급
                    if(piece.PlacedObjectType == ObjectType.None) // 해당 위치에 아무것도 올라와 있지 않을 때
                    {
                        PlacePlaneManager.Instance.HighLightHandler.CanPiecePlacePlanes.Add(piece); // 배치가 가능한 기물 배치 칸 저장
                    }
                }
            }
        }

        // 움직일 수 있는 칸을 찾는 함수
        public void FindCanMovePlacePlane(PiecePlacePlaneObject piece, TeamType teamType)
        {
            bool findTeamRoad = false; // 팀 도로를 찾았는지 여부를 체크하는 변수로 찾지 못했다고 초기화

            ResetPlacePlanes(false); // 전체 도로 및 기물 칸 접근 여부 false로 초기화 - 이동 가능한 칸을 찾기 위함
            PlacePlaneManager.Instance.HighLightHandler.CanPieceMovePlanes.Clear(); // 기물 이동 가능한 판 저장 컨테이너 비우기 - 이전에 저장했던 이동 가능한 판들을 초기화

            foreach (var nearRoad in piece.nearRoadPlaceTransformList) // 해당 기물 칸 주위 도로 칸 순회
            {
                if(nearRoad.TeamType == teamType && nearRoad.PlacedObjectType == ObjectType.Road) // 내 도로가 있다면
                {
                    findTeamRoad = true; // 팀 도로를 찾았다고 할당
                    break; // 반복문 나가기
                }
            }

            foreach (var nearRoad in piece.nearRoadPlaceTransformList) // 현재 기물의 근접한 도로 순회
            {
                FindNearPieces(teamType, nearRoad, !findTeamRoad); // 근접한 기물 찾기 - 팀 도로를 찾았다면 쭉 탐색, 팀 도로를 찾지 못했다면 한 번만 탐색
            }
        }

        // 배치 가능한 도로 칸들을 찾는 함수
        public void FindCanPlaceRoadPlane(TeamType type)
        {
            foreach (var road in PlacePlaneManager.Instance.PlacePlaneMap.RoadPlacePlanes) // 전체 도로 판 순회
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
                        PlacePlaneManager.Instance.HighLightHandler.CanRoadPlacePlanes.Add(road); // 배치가 가능한 도로 배치 칸 저장
                    }
                }
            }
        }

        // 배치 판 확인 여부 초기화 함수(완전 초기화 할지 여부)
        public void ResetPlacePlanes(bool isClear = true)
        {
            foreach (var piece in PlacePlaneManager.Instance.PlacePlaneMap.PiecePlacePlanes) // 전체 기물 판 순회
            {
                piece.IsChecked = false; // 확인하지 않은 상태로 초기화
            }

            foreach (var road in PlacePlaneManager.Instance.PlacePlaneMap.RoadPlacePlanes) // 전체 도로 판 순회
            {
                road.IsChecked = false; // 확인하지 않은 상태로 초기화
            }

            if(isClear)
            {
                PlacePlaneManager.Instance.HighLightHandler.CanPiecePlacePlanes.Clear(); // 기물 배치 가능한 판 저장 컨테이너 비우기
                PlacePlaneManager.Instance.HighLightHandler.CanRoadPlacePlanes.Clear(); // 도로 배치 가능한 판 저장 컨테이너 비우기
                PlacePlaneManager.Instance.HighLightHandler.CanPieceMovePlanes.Clear(); // 기물 이동 가능한 판 저장 컨테이너 비우기

                foreach (var piece in PieceManager.Instance.CanAttackPieceMap) // 공격 가능 기물 저장 컨테이너 순회
                    piece.Value.Clear(); // 리스트 클리어                                                                   
            }

        }

        // 배치 가능한 도로 칸을 추가하고 그 도로에 인접한 기물들을 찾는 함수(팀 타입, 도로 칸, 한 번만 검사할지)
        private void FindNearPieces(TeamType teamType, RoadPlacePlaneObject road, bool once = false)
        {
            road.IsChecked = true;
            foreach (var nearPiece in road.nearPiecePlaceTransformList) // 인접한 기물 확인
            {
                if (nearPiece.IsChecked) // 이미 확인을 했었다면
                    continue; // 넘기기
                else if ((nearPiece.TeamType != teamType && nearPiece.TeamType != TeamType.None)) // 현재 팀이 아니고 다른 팀에 속한 상태라면
                {
                    ObjectType objType = nearPiece.PlacedObjectType; // 배치되어 있는 객체 할당
                    switch (objType)
                    {
                        case ObjectType.Miner: // 광부가 배치되어 있다면
                            if (!PieceManager.Instance.CanAttackPieceMap[ObjectType.Miner].Contains(nearPiece.PlacedPiece))
                                PieceManager.Instance.CanAttackPieceMap[ObjectType.Miner].Add(nearPiece.PlacedPiece); // 공격 가능한 광부 객체 리스트에 추가
                            break;
                        case ObjectType.Soldier: // 보병이 배치되어 있다면
                            if (!PieceManager.Instance.CanAttackPieceMap[ObjectType.Soldier].Contains(nearPiece.PlacedPiece))
                                PieceManager.Instance.CanAttackPieceMap[ObjectType.Soldier].Add(nearPiece.PlacedPiece); // 공격 가능한 보병 객체 리스트에 추가
                            break;
                        case ObjectType.Tank: // 전차가 배치되어 있다면
                            if (!PieceManager.Instance.CanAttackPieceMap[ObjectType.Tank].Contains(nearPiece.PlacedPiece))
                                PieceManager.Instance.CanAttackPieceMap[ObjectType.Tank].Add(nearPiece.PlacedPiece); // 공격 가능한 전차 객체 리스트에 추가
                            break;
                    }

                    continue;
                }

                if (nearPiece.PlacedObjectType == ObjectType.None) // 빈 칸이라면
                {
                    PlacePlaneManager.Instance.HighLightHandler.CanPieceMovePlanes.Add(nearPiece); // 이동 가능한 기물 배치 칸 추가

                    if(!once) // 한 번만 확인하는 게 아닐 경우
                        FindNearRoads(teamType, nearPiece); // 해당 기물 칸의 인접한 도로 탐색
                }
                else // 빈 칸이 아니라면 - 즉 내 팀에 속한 기물이 올려져 있다면
                {
                    if(!once) // 한 번만 확인하는 게 아닐 경우
                        FindNearRoads(teamType, nearPiece); // 해당 기물 칸의 인접한 도로만 탐색
                }
            }
        }

        // 배치 가능한 기물 칸을 추가하고 그 기물에 인접한 도로들을 찾는 함수(팀 타입, 기물 칸, 한 번만 검사할지)
        private void FindNearRoads(TeamType teamType, PiecePlacePlaneObject piece, bool once = false)
        {
            piece.IsChecked = true;
            foreach (var nearRoad in piece.nearRoadPlaceTransformList) // 인접한 도로 확인
            {
                if (nearRoad.IsChecked || (nearRoad.TeamType != teamType && nearRoad.TeamType != TeamType.None)) // 이미 확인을 했었다면 또는 (현재 팀이 아니면서 다른 팀이라면)
                     continue; // 넘기기

                if(nearRoad.PlacedObjectType == ObjectType.None) // 빈 칸이라면
                {
                    PlacePlaneManager.Instance.HighLightHandler.CanRoadPlacePlanes.Add(nearRoad); // 배치 가능한 도로 칸에 추가
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
// 마지막 작성 일자: 2025.09.12