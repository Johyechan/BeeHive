using InGame.MyManager.MyPlacePlane;
using InGame.MyObject;
using InGame.MyObject.MyObjectEnum;

namespace InGame.MySystem
{
    // 작성자: 조혜찬
    // 배치 가능한 판들을 찾는 시스템 클래스
    public class FindCanPlacePlaneSystem
    {
        // 배치 가능한 기물 칸들을 찾는 함수
        public void FindCanPiecePlacePlane(TeamType type)
        {
            foreach (var piece in PlacePlaneManager.Instance.PlacePlaneMapProp.PiecePlacePlanesProp) // 전체 기물 판 순회
            {
                if (piece.TeamTypeProp == type && piece.PlacedObjectTypeProp != ObjectType.None) // 기물 칸의 팀 타입이 현재 탐색 중인 팀 타입이며 빈 곳이 아니라면
                {
                    FindNearRoads(type, piece); // 배치가 가능한 기물 배치 칸 저장 후 인접한 도로 색탐
                }
                else if (piece.isNearToCastle) // 성과 인접한 배치 판이라면
                {
                    piece.IsCheckedProp = true; // 체크 한 것으로 취급
                    if(piece.PlacedObjectTypeProp == ObjectType.None) // 해당 위치에 아무것도 올라와 있지 않을 때
                    {
                        PlacePlaneManager.Instance.HighLightHandlerProp.CanPiecePlacePlanesProp.Add(piece); // 배치가 가능한 기물 배치 칸 저장
                    }
                }
            }
        }

        // 배치 가능한 도로 칸들을 찾는 함수
        public void FindCanRoadPlacePlane(TeamType type)
        {
            foreach (var road in PlacePlaneManager.Instance.PlacePlaneMapProp.RoadPlacePlanesProp) // 전체 도로 판 순회
            {
                if (road.TeamTypeProp == type && road.PlacedObjectTypeProp != ObjectType.None) // 도로 칸의 팀 타입이 현재 탐색 중인 팀 타입이며 빈 곳이 아니라면
                {
                    FindNearPieces(type, road); // 배치가 가능한 도로 배치 칸 저장 후 인접한 기물 탐색
                }
                else if (road.isNearToCastle) // 성과 인접한 배치 판이라면
                {
                    road.IsCheckedProp = true; // 체크 한 것으로 취급
                    if(road.PlacedObjectTypeProp == ObjectType.None) // 아무것도 올라와 있지 않은 상태 일때
                    {
                        PlacePlaneManager.Instance.HighLightHandlerProp.CanRoadPlacePlanesProp.Add(road); // 배치가 가능한 도로 배치 칸 저장
                    }
                }
            }
        }

        // 배치 판 확인 여부 초기화 함수
        public void ResetPlacePlanes()
        {
            foreach (var piece in PlacePlaneManager.Instance.PlacePlaneMapProp.PiecePlacePlanesProp) // 전체 기물 판 순회
            {
                piece.IsCheckedProp = false; // 확인하지 않은 상태로 초기화
            }
            PlacePlaneManager.Instance.HighLightHandlerProp.CanPiecePlacePlanesProp.Clear(); // 기물 배치 가능한 판 저장 컨테이너 비우기

            foreach (var road in PlacePlaneManager.Instance.PlacePlaneMapProp.RoadPlacePlanesProp) // 전체 도로 판 순회
            {
                road.IsCheckedProp = false; // 확인하지 않은 상태로 초기화
            }
            PlacePlaneManager.Instance.HighLightHandlerProp.CanRoadPlacePlanesProp.Clear(); // 도로 배치 가능한 판 저장 컨테이너 비우기
        }

        // 배치 가능한 도로 칸을 추가하고 그 도로에 인접한 기물들을 찾는 함수
        private void FindNearPieces(TeamType teamType, RoadPlacePlaneObject road)
        {   
            road.IsCheckedProp = true; // 체크 완료
            foreach (var nearPiece in road.nearPiecePlaceTransformList) // 인접한 기물 확인
            {
                if (nearPiece.IsCheckedProp || (nearPiece.TeamTypeProp != teamType && nearPiece.TeamTypeProp != TeamType.None)) // 이미 확인을 했었다면 또는 (현재 팀이 아니고 다른 팀에 속한 상태라면)
                    continue; // 넘기기

                if(nearPiece.PlacedObjectTypeProp == ObjectType.None) // 빈 칸이라면
                {
                    PlacePlaneManager.Instance.HighLightHandlerProp.CanMovePlacePlanes.Add(nearPiece); // 이동 가능한 기물 배치 칸 추가
                    FindNearRoads(teamType, nearPiece); // 해당 기물 칸의 인접한 도로 탐색
                }
                else // 빈 칸이 아니라면 - 즉 내 팀에 속한 기물이 올려져 있다면
                {
                    FindNearRoads(teamType, nearPiece); // 해당 기물 칸의 인접한 도로만 탐색
                }
            }
        }

        // 배치 가능한 기물 칸을 추가하고 그 기물에 인접한 도로들을 찾는 함수
        private void FindNearRoads(TeamType teamType, PiecePlacePlaneObject piece)
        {
            piece.IsCheckedProp = true; // 체크 완료
            foreach (var nearRoad in piece.nearRoadPlaceTransformList) // 인접한 도로 확인
            {
                if (nearRoad.IsCheckedProp || (nearRoad.TeamTypeProp != teamType && nearRoad.TeamTypeProp != TeamType.None)) // 이미 확인을 했었다면 또는 (현재 팀이 아니면서 다른 팀이라면)
                     continue; // 넘기기

                if(nearRoad.PlacedObjectTypeProp == ObjectType.None) // 빈 칸이라면
                {
                    PlacePlaneManager.Instance.HighLightHandlerProp.CanRoadPlacePlanesProp.Add(nearRoad); // 배치 가능한 도로 칸에 추가
                }
                else // 빈 칸이 아니라면 - 즉 내 도로 기물이 올라가 있다면
                {
                    FindNearPieces(teamType, nearRoad); // 해당 도로 칸의 인접한 기물만 탐색
                }
            }
        }
    }
}
// 마지막 작성 일자: 2025.07.21