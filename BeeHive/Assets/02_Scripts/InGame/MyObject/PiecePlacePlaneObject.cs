using InGame.MyObject.MyObjectEnum;
using System.Collections.Generic;
using UnityEngine;

namespace InGame.MyObject
{
    // 작성자: 조혜찬
    // 기물 배치 칸의 기능 클래스
    public class PiecePlacePlaneObject : PlacePlaneObjectBase
    {
        public List<RoadPlacePlaneObject> nearRoadPlaceTransformList; // 가깝게 붙어있는 도로 칸을 저장하는 리스트

        private ObjectType _canPlacePiece; // 현재 배치가 가능한 기물 객체를 확인하는 변수\
        public ObjectType CanPlacePiece { get { return _canPlacePiece; } set { _canPlacePiece = value; } } // 현재 배치가 가능한 기물 객체 프로퍼티

        [SerializeField] private Transform _minerParent; // 광부 기물들의 부모
        [SerializeField] private Transform _soldierParent; // 보병 기물들의 부모
        [SerializeField] private Transform _tankParent; // 전차 기물들의 부모

        private Dictionary<ObjectType, Transform> _pieceMap = new(); // 타입에 따라 필요한 객체를 가지는 부모를 찾기 위한 맵

        private void Awake()
        {
            // 타입에 맞게 맵 초기화
            _pieceMap.Clear(); // 맵 비우기
            _pieceMap.Add(ObjectType.Miner, _minerParent); // 광부 추가
            _pieceMap.Add(ObjectType.Soldier, _soldierParent); // 보병 추가
            _pieceMap.Add(ObjectType.Tank, _tankParent); // 전차 추가
        }

        // 마우스로 클릭 시 실행될 함수
        public override void ObjectClicked()
        {
            Transform pieceParent = _pieceMap[_canPlacePiece]; // 현재 배치 가능한 타입의 객체 부모
            int pieceCount = pieceParent.childCount; // 현재 보유 중인 배치 가능한 타입의 기물 수

            PieceBase pieceBase = pieceParent.GetChild(pieceCount - 1).GetComponent<PieceBase>(); // 현재 배치 가능한 타입의 객체의 마지막 객체의 PieceBase를 가져오기
            if(pieceBase != null) // null 체크
            {
                PlacedObjectType = _canPlacePiece; // 현재 배치가 가능한 기물로 이 배치판에 배치되어있는 기물로 지정
                pieceBase.MoveToPlacePlane(transform.position); // 기물을 현재 배치할 배치 판의 위치로 이동
            }
        }
    }
}
// 마지막 작성 일자: 2025.07.14