using System.Collections.Generic;
using UnityEngine;

namespace InGame.MyObject
{
    // 작성자: 조혜찬
    // 도로 배치 칸의 기능
    public class RoadPlacePlaneObject : PlacePlaneObjectBase
    {
        [SerializeField] private List<PiecePlacePlaneObject> _neerPiecePlaceTransformList; // 가깝게 붙어있는 기물 칸을 저장하는 리스트

    }
}
// 마지막 작성 일자: 2025.07.09