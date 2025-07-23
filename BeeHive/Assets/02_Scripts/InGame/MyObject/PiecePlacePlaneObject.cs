using InGame.MyEvent;
using InGame.MyManager;
using InGame.MyObject.MyObjectEnum;
using System.Collections.Generic;
using UnityEngine;

namespace InGame.MyObject
{
    // 작성자: 조혜찬
    // 기물 배치 칸의 기능 클래스
    public class PiecePlacePlaneObject : PlacePlaneObjectBase
    {
        public List<RoadPlacePlaneObject> nearRoadPlaceTransformList = new(); // 가깝게 붙어있는 도로 칸을 저장하는 리스트

        private Transform _minerParent; // 광부 기물들의 부모
        private Transform _soldierParent; // 보병 기물들의 부모
        private Transform _tankParent; // 전차 기물들의 부모

        private Dictionary<ObjectType, Transform> _pieceMap = new(); // 타입에 따라 필요한 객체를 가지는 부모를 찾기 위한 맵

        protected override void Awake()
        {
            base.Awake();

            _minerParent = GameObject.Find("PlayerMiners").transform; // 광부 기물들의 부모 탐색 후 할당
            _soldierParent = GameObject.Find("PlayerSoldiers").transform; // 보병 기물들의 부모 탐색 후 할당
            _tankParent = GameObject.Find("PlayerTanks").transform; // 전차 기물들의 부모 탐색 후 할당

            // 타입에 맞게 맵 초기화
            _pieceMap.Clear(); // 맵 비우기
            _pieceMap.Add(ObjectType.Miner, _minerParent); // 광부 추가
            _pieceMap.Add(ObjectType.Soldier, _soldierParent); // 보병 추가
            _pieceMap.Add(ObjectType.Tank, _tankParent); // 전차 추가
        }

        // 마우스로 클릭 시 실행될 함수
        public override void ObjectClicked()
        {
            Transform pieceParent = _pieceMap[CanPlacePieceTypeProp]; // 현재 배치 가능한 타입의 객체 부모
            int pieceCount = pieceParent.childCount; // 현재 보유 중인 배치 가능한 타입의 기물 수

            PieceBase pieceBase = pieceParent.GetChild(pieceCount - 1).GetComponent<PieceBase>(); // 현재 배치 가능한 타입의 객체의 마지막 객체의 PieceBase를 가져오기
            // 현재 턴의 팀 타입으로 pieceBase 팀 타입 결정
            pieceBase.teamType = TeamType.Team1; // 임시

            if(pieceBase != null) // null 체크
            {
                UIManager.Instance.CanInteractionUI = false; // UI 상호작용 불가능 상태로 할당
                PlacedObjectTypeProp = CanPlacePieceTypeProp; // 현재 배치가 가능한 기물을 이 배치판에 배치되어있는 기물로 지정
                TeamTypeProp = pieceBase.teamType; // 현재 기물 배치 칸의 팀 타입을 기물의 팀 타입으로 할당
                pieceBase.MoveToPlacePlane(transform.parent, transform.localPosition); // 기물을 현재 배치판의 부모 자식으로 변경, 기물을 현재 배치할 배치 판의 위치로 이동
                HighLightEvents.OnPiecePlacementHighLight?.Invoke(false, true); // 기물 칸 하이라이트를 끄는 매개변수로 이벤트 콜(하이라이트 키기 여부, 배치 칸 이동 칸 여부 - true는 배치칸, false는 이동칸)
            }
        }
    }
}
// 마지막 작성 일자: 2025.07.22