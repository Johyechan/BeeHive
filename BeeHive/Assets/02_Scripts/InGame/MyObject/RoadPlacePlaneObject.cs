using InGame.MyEvent;
using InGame.MyManager;
using InGame.MyEnum;
using System.Collections.Generic;
using UnityEngine;
using InGame.MyObject.Piece;
using InGame.MyManager.MyPiece;
using InGame.MyObject.Handler;

namespace InGame.MyObject
{
    // 작성자: 조혜찬
    // 도로 배치 칸의 기능
    public class RoadPlacePlaneObject : PlacePlaneObjectBase
    {
        [SerializeField] private float _roadAngle; // 도로 배치시 도로의 회전 값

        public List<PiecePlacePlaneObject> nearPiecePlaceTransformList = new(); // 가깝게 붙어있는 기물 칸을 저장하는 리스트

        private Transform _roadParent; // 도로 기물의 부모

        private RoadPlaceReturnCheckHandler _roadPlaceReturnCheckHandler; // 도로 배치 가능 여부를 확인하는 핸들러

        private RoadPlaceHandler _roadPlaceHandler; // 도로 배치 기능 핸들러

        protected override void Awake()
        {
            base.Awake();

            _roadPlaceReturnCheckHandler = new RoadPlaceReturnCheckHandler();
            _roadPlaceHandler = new RoadPlaceHandler();

            _roadParent = TeamManager.Instance.GetRoadTransform(TeamManager.Instance.CurrentTeamType); // 도로 기물의 부모 탐색 후 할당
        }

        // 클릭 시 실행될 함수
        public override async void ObjectClicked()
        {
            if (await _roadPlaceReturnCheckHandler.IsReturn(_leftPieceCount, _cost))
                return;

            GameObject newRoad = _roadParent.GetChild(_roadParent.childCount - 1).gameObject; // 도로 객체들의 부모 객체에서 도로 객체 가져오기
            PieceBase roadPiece = newRoad.GetComponent<PieceBase>();

            if (roadPiece != null)
            {
                PlacedPiece = roadPiece; // 배치된 기물에 도로 할당
                PlacedPiece.PieceVariable.currentRoadPlacePlane = this; // 배치된 도로의 배치칸을 할당
                await _roadPlaceHandler.Place(this, roadPiece, _roadParent, _roadAngle); // 도로 배치 기능
            }
        }
    }
}
// 마지막 작성 일자: 2025.09.29