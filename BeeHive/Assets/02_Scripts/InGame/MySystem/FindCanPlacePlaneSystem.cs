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
        public void FindCanPiecePlacePlane(NearCastleType type)
        {
            foreach (var piece in PlacePlaneManager.Instance.PlacePlaneMapProp.PiecePlacePlanesProp)
            {
                if (piece.NearCastleTypeProp == type)
                {
                    FindNearRoads(piece); // 인접한 도로 탐색
                }
            }
        }

        // 배치 가능한 도로 칸들을 찾는 함수
        public void FindCanRoadPlacePlane(NearCastleType type)
        {

        }

        // 인접한 기물들을 찾는 함수
        private void FindNearPieces(RoadPlacePlaneObject road)
        {

        }

        // 인접한 도로들을 찾는 함수
        private void FindNearRoads(PiecePlacePlaneObject piece)
        {
            PlacePlaneManager.Instance.HighLightHandlerProp.CanPlacePlanesProp.Add(piece); // 하이라이트가 켜질 배치 칸 저장 컨테이너에 추가
            piece.IsCheckedProp = true; // 체크 완료
        }
    }
}
// 마지막 작성 일자: 2025.07.17