using System.Collections.Generic;
using UnityEngine;

namespace InGame.MyObject
{
    // 작성자: 조혜찬
    // 기물 배치 칸의 기능 클래스
    public class PiecePlacePlaneObject : PlacePlaneObjectBase
    {
        public List<RoadPlacePlaneObject> nearRoadPlaceTransformList; // 가깝게 붙어있는 도로 칸을 저장하는 리스트
    }
}
// 마지막 작성 일자: 2025.07.09