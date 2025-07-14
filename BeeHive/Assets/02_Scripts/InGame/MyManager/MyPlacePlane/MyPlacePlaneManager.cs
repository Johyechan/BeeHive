using MyUtil;
using System.Collections.Generic;
using UnityEngine;

namespace InGame.MyManager.MyPlacePlane
{
    // 작성자: 조혜찬
    // 배치가 가능한 배치 판들을 저장하는 싱글톤 클래스
    public class MyPlacePlaneManager : MonoSingleton<MyPlacePlaneManager>
    {
        private List<Transform> _canPlacePiecePlacePlaneList = new(); // 기물 배치가 가능한 배치 판들을 모으는 리스트
        private List<Transform> _canPlaceRoadPlacePlaneList = new(); // 도로 배치가 가능한 도로 판들을 모으는 리스트

        public List<Transform> CanPlacePiecePlacePlaneList // 기물 배치가 가능한 배치 판들을 모아둔 리스트의 프로퍼티
        {
            get { return _canPlacePiecePlacePlaneList; }
        }
        public List<Transform> CanPlaceRoadPlacePlaneList // 도로 배치가 가능한 배치 판들을 모아둔 리스트의 프로퍼티
        {
            get { return _canPlaceRoadPlacePlaneList; }
        }
    }
}
// 마지막 작성 일자: 2025.07.14
