using InGame.MyObject;
using MyUtil;
using System.Collections.Generic;
using UnityEngine;

namespace InGame.MyManager.MyPlacePlane
{
    // 작성자: 조혜찬
    // 배치가 가능한 배치 판들을 저장하는 싱글톤 클래스
    public class MyPlacePlaneManager : MonoSingleton<MyPlacePlaneManager>
    {
        [SerializeField] private Transform _placePlaneParent; // 배치 판들의 부모

        private HashSet<PiecePlacePlaneObject> _allPiecePlacePlane = new(); // 전체 기물 배치판 해시 테이블 기반 컨테이너
        public HashSet<PiecePlacePlaneObject> AllPiecePlacePlane { get  { return _allPiecePlacePlane; } } // _allPiecePlacePlane 프로퍼티

        private HashSet<RoadPlacePlaneObject> _allRoadPlacePlane = new(); // 전체 도로 배치판 해시 테이블 기반 컨테이너
        public HashSet<RoadPlacePlaneObject> AllRoadPlacePlane { get { return _allRoadPlacePlane; } } // _allRoadPlacePlane 프로퍼티

        private HashSet<PlacePlaneObjectBase> _highLightOnPlacePlanes = new(); // 하이라이트가 켜진 배치 판들을 저장해두는 해시 테이블 기반 컨테이너
        public HashSet<PlacePlaneObjectBase> HighLightOnPlacePlanes { get { return _highLightOnPlacePlanes; } } // _highLightOnPlacePlanes 프로퍼티

        protected override void Awake()
        {
            base.Awake();

            for (int i = 0; i < _placePlaneParent.childCount; i++) // 기물 판 전부 순회
            {
                if(_placePlaneParent.GetChild(i).TryGetComponent<PiecePlacePlaneObject>(out var piece)) // 만약 기물 판 클래스를 가져올 수 있다면
                {
                    _allPiecePlacePlane.Add(piece); // 기물 판 컨테이너에 추가
                }
                else if(_placePlaneParent.GetChild(i).TryGetComponent<RoadPlacePlaneObject>(out var road)) // 만약 도로 판 클래스를 가져올 수 있다면
                {
                    _allRoadPlacePlane.Add(road); // 도로 판 컨테이너에 추가
                }
                else
                {
                    Debug.Log("가져올 수 없음");
                }
            }
        }

        private void OnEnable()
        {
            PlacedEventSystem.OnPlaced += HighLightOff; // 이벤트 구독
        }

        private void OnDisable()
        {
            PlacedEventSystem.OnPlaced -= HighLightOff; // 이벤트 해제
        }

        private void HighLightOff()
        {
            if (_highLightOnPlacePlanes.Count <= 0) // 하이라이트가 켜져 있는 객체 존재하지 않다면
                return; // 그냥 반환

            foreach(var placePlane in _highLightOnPlacePlanes) // 하이라이트가 켜져 있는 객체들 순회
            {
                placePlane.HighLightOff(); // 하이라이트 끄기
            }
        }
    }
}
// 마지막 작성 일자: 2025.07.14
