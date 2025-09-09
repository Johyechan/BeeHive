using InGame.MyEvent;
using InGame.MyManager;
using MyUtil.MyObjectPool;
using InGame.MyEnum;
using System.Collections.Generic;
using UnityEngine;

namespace InGame.MyObject
{
    // 작성자: 조혜찬
    // 도로 배치 칸의 기능
    public class RoadPlacePlaneObject : PlacePlaneObjectBase
    {
        [SerializeField] private float _roadAngle; // 도로 배치시 도로의 회전 값

        public List<PiecePlacePlaneObject> nearPiecePlaceTransformList = new(); // 가깝게 붙어있는 기물 칸을 저장하는 리스트

        private Transform _roadParent; // 도로 기물의 부모

        protected override void Awake()
        {
            base.Awake();

            ParentSet();
        }

        // 부모 초기화 함수
        private void ParentSet()
        {
            _roadParent = GameObject.Find(TeamManager.Instance.RoadParentName).transform; // 도로 기물의 부모 탐색 후 할당
        }

        // 클릭 시 실행될 함수
        public override async void ObjectClicked()
        {
            if (!await WarningEvent.OnCheckCurrentTurnTeam()) // 현재 턴의 팀을 확인해서 현재 턴이 내 턴이 아니라면
            {
                HighLightEvents.OnRoadPlacementHighLight?.Invoke(false); // 도로 칸 하이라이트를 끄는 매개변수로 이벤트 콜
                return; // 반환
            }

            // 현재 턴이 메인 턴이 아니라면
            if (!await WarningEvent.OnCheckCurrentTurn.Invoke(TurnType.MainTurn, "메인 턴이 아니라서 도로를 배치할 수 없습니다."))
            {
                HighLightEvents.OnRoadPlacementHighLight?.Invoke(false); // 도로 칸 하이라이트를 끄는 매개변수로 이벤트 콜
                return; // 반환
            }

            if (!await WarningEvent.OnCheckLeftPieceCount(_leftPieceCount, "남은 도로가 없어 배치할 수 없습니다")) // 남은 도로가 없다면
            {
                HighLightEvents.OnRoadPlacementHighLight?.Invoke(false); // 도로 칸 하이라이트를 끄는 매개변수로 이벤트 콜
                return; // 반환
            }

            if (!await WarningEvent.OnCanPayCost.Invoke(_cost, "금괴가 부족하여 도로를 배치할 수 없습니다.")) // 비용을 지불할 수 없다면
            {
                HighLightEvents.OnRoadPlacementHighLight?.Invoke(false); // 도로 칸 하이라이트를 끄는 매개변수로 이벤트 콜
                return; // 반환
            }

            GameObject newRoad = _roadParent.GetChild(_roadParent.childCount - 1).gameObject; // 도로 객체들의 부모 객체에서 도로 객체 가져오기
            PieceBase roadPiece = newRoad.GetComponent<PieceBase>();

            if (roadPiece != null)
            {
                UIManager.Instance.CanInteractionUI = false; // UI 상호작용 불가능 상태로 할당
                PlacedObjectType = CanPlacePieceType; // 배치 성공 시 배치 가능한 기물이 위에 배치 되었다고 할당
                TeamType = roadPiece.teamType; // 현재 배치 가능한 칸의 팀 타입을 도로 기물의 팀 타입으로 지정

                RoadInfo roadInfo = new RoadInfo()
                {
                    roomID = SceneMgr.Instance.CurrentRoomID, // 현재 방 ID
                    roadID = roadPiece.Id, // 도로 객체 ID
                    placePlaneId = _id, // 현재 객체 ID
                    placedType = (int)CanPlacePieceType, // 배치 객체 타입
                    roadTeamType = (int)TeamType, // 배치 객체 팀 타입
                    roadParentName = _roadParent.name, // 부모 객체 이름
                    targetParentName = transform.parent.name, // 부모 객체 이름
                    targetPos = transform.localPosition, // 최종 위치
                    angle = _roadAngle // 최종 각도
                };
                string json = JsonUtility.ToJson(roadInfo); // Json으로 변환
                NetworkManager.Instance.Socket.Emit("makeRoad", json);

                roadPiece.MoveToPlacePlane(transform.parent, transform.localPosition, _roadAngle); // 기물을 현재 배치 판 부모의 자식으로 변경 + 현재 이 배치판 위치 이동 + 각도 회전

                _ = FindCanPlacePlane();

                UIEvents.OnSetLeftPieceText?.Invoke(); // 남은 기물 수 변경
                HighLightEvents.OnRoadPlacementHighLight?.Invoke(false); // 도로 칸 하이라이트를 끄는 매개변수로 이벤트 콜
            }
        }
    }
}
// 마지막 작성 일자: 2025.09.09