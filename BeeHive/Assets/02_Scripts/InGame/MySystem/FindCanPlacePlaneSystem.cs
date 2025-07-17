using InGame.MyManager.MyPlacePlane;
using InGame.MyObject;
using InGame.MyObject.MyObjectEnum;
using UnityEngine;

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
                if (piece.isNearToCastle) // 성과 인접한 배치 판이라면
                {
                    FindNearRoads(type, piece); // 인접한 도로 탐색
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

            foreach (var road in PlacePlaneManager.Instance.PlacePlaneMapProp.RoadPlacePlanesProp) // 전체 도로 판 순회
            {
                road.IsCheckedProp = false; // 확인하지 않은 상태로 초기화
            }
        }

        // 배치 가능한 도로 칸들을 찾는 함수
        public void FindCanRoadPlacePlane(TeamType type)
        {
            foreach (var road in PlacePlaneManager.Instance.PlacePlaneMapProp.RoadPlacePlanesProp) // 전체 도로 판 순회
            {
                if (road.isNearToCastle) // 성과 인접한 배치 판이라면
                {
                    FindNearPieces(type, road); // 인접한 기물 탐색
                }
            }
        }

        // 인접한 기물들을 찾는 함수
        private void FindNearPieces(TeamType teamType, RoadPlacePlaneObject road)
        {
            if(!PlacePlaneManager.Instance.HighLightHandlerProp.CanRoadPlacePlanesProp.Contains(road)) // 동일한 대상을 찾을 수 없다면
            {
                PlacePlaneManager.Instance.HighLightHandlerProp.CanRoadPlacePlanesProp.Add(road); // 배치가 가능한 도로 배치 칸 저장 컨테이너에 추가
            }
            
            road.IsCheckedProp = true; // 체크 완료
            foreach(var nearPiece in road.nearPiecePlaceTransformList) // 인접한 기물 확인
            {
                if (nearPiece.IsCheckedProp || nearPiece.TeamTypeProp != teamType) // 이미 확인을 했었다면 또는 현재 팀의 타입과 기물의 타입이 다르다면
                    continue; // 넘기기

                if(nearPiece.PlacedObjectTypeProp == ObjectType.None) // 아무것도 배치되어있지 않다면
                {
                    FindNearRoads(teamType, nearPiece); // 해당 기물칸의 인접한 도로 탐색
                }
            }
        }

        // 인접한 도로들을 찾는 함수
        private void FindNearRoads(TeamType teamType, PiecePlacePlaneObject piece)
        {
            if(!PlacePlaneManager.Instance.HighLightHandlerProp.CanPiecePlacePlanesProp.Contains(piece)) // 동일한 대상을 찾을 수 없다면
            {
                PlacePlaneManager.Instance.HighLightHandlerProp.CanPiecePlacePlanesProp.Add(piece); // 배치가 가능한 기물 배치 칸 저장 컨테이너에 추가
            }
            
            piece.IsCheckedProp = true; // 체크 완료
            foreach(var nearRoad in piece.nearRoadPlaceTransformList) // 인접한 도로 확인
            {
                if (nearRoad.IsCheckedProp || teamType != nearRoad.TeamTypeProp) // 이미 확인을 했었다면 또는 현재 팀의 타입과 도로의 타입이 다르다면
                    continue; // 넘기기

                if(nearRoad.PlacedObjectTypeProp == ObjectType.Road) // 도로 기물이 올라가 있다면
                {
                    FindNearPieces(teamType, nearRoad); // 해당 도로칸의 인접한 기물 탐색
                }
            }
        }
    }
}
// 마지막 작성 일자: 2025.07.17