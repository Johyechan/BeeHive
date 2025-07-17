using InGame.MyObject;
using System.Collections.Generic;
using UnityEngine;

namespace InGame.MySystem
{
    // 작성자: 조혜찬
    // 전체 기물 판들을 저장하는 클래스
    public class PlacePlaneMap
    {
        private HashSet<PiecePlacePlaneObject> _piecePlacePlanes = new(); // 전체 기물 배치판 해시 테이블 기반 컨테이너
        public HashSet<PiecePlacePlaneObject> PiecePlacePlanesProp { get { return _piecePlacePlanes; } } // _allPiecePlacePlane 프로퍼티

        private HashSet<RoadPlacePlaneObject> _roadPlacePlanes = new(); // 전체 도로 배치판 해시 테이블 기반 컨테이너
        public HashSet<RoadPlacePlaneObject> RoadPlacePlanesProp { get { return _roadPlacePlanes; } } // _allRoadPlacePlane 프로퍼티

        // 모든 배치 판을 기물 따로 도로 따로 저장하는 함수
        public void PlacePlaneSet(Transform placePlaneParent)
        {
            for (int i = 0; i < placePlaneParent.childCount; i++) // 기물 판들을 전부 순회
            {
                if (placePlaneParent.GetChild(i).TryGetComponent<PiecePlacePlaneObject>(out var piece)) // 만약 기물 판 클래스를 가져올 수 있다면
                {
                    _piecePlacePlanes.Add(piece); // 기물 판 컨테이너에 추가
                }
                else if (placePlaneParent.GetChild(i).TryGetComponent<RoadPlacePlaneObject>(out var road)) // 만약 도로 판 클래스를 가져올 수 있다면
                {
                    _roadPlacePlanes.Add(road); // 도로 판 컨테이너에 추가
                }
                else
                {
                    Debug.Log("가져올 수 없음");
                }
            }
        }
    }
}
// 마지막 작성 일자: 2025.07.17