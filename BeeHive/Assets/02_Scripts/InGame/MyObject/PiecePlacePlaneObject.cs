using InGame.MyEnum;
using InGame.MyEvent;
using InGame.MyManager;
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

            ParentSet();
        }

        // 부모 초기화 함수
        private void ParentSet()
        {
            _minerParent = GameObject.Find(TeamManager.Instance.MinerParentName).transform; // 광부 기물들의 부모 탐색 후 할당
            _soldierParent = GameObject.Find(TeamManager.Instance.SoldierParentName).transform; // 보병 기물들의 부모 탐색 후 할당
            _tankParent = GameObject.Find(TeamManager.Instance.TankParentName).transform; // 전차 기물들의 부모 탐색 후 할당
            _pieceMap.Clear(); // 맵 비우기
            _pieceMap.Add(ObjectType.Miner, _minerParent); // 광부 추가
            _pieceMap.Add(ObjectType.Soldier, _soldierParent); // 보병 추가
            _pieceMap.Add(ObjectType.Tank, _tankParent); // 전차 추가
        }

        private void HighLightOffEvent()
        {
            HighLightEvents.OnPiecePlacementHighLight?.Invoke(false, true); // 기물 칸 하이라이트를 끄는 매개변수로 이벤트 콜(하이라이트 키기 여부, 배치 칸 이동 칸 여부 - true는 배치칸, false는 이동칸)
            HighLightEvents.OnPieceMovementHighLight?.Invoke(false, false); // 기물 칸 하이라이트를 끄는 매개변수로 이벤트 콜(하이라이트 키기 여부, 배치 칸 이동 칸 여부 - true는 배치칸, false는 이동칸)
        }

        // 마우스로 클릭 시 실행될 함수
        public override void ObjectClicked()
        {
            if (GameManager.Instance.CurrentMovePiece != null) // 현재 이동 가능한 객체 있다면
            {
                if (!WarningEvent.OnCheckCurrentTurnTeam()) // 현재 턴이 자신의 턴이 아닐 경우
                {
                    HighLightOffEvent(); // 하이라이트 끄기
                    return; // 반환
                }

                // 현재 턴이 메인 턴이 아니라면
                if (!WarningEvent.OnCheckCurrentTurn.Invoke(TurnType.MainTurn, "메인 턴이 아니라서 기물을 배치할 수 없습니다."))
                {
                    HighLightOffEvent(); // 하이라이트 끄기
                    return; // 반환
                }

                ObjectMove(); // 기물 이동 함수 실행
            }
            else // 현재 이동 가능한 객체가 없다면
            {
                if (!WarningEvent.OnCheckCurrentTurnTeam()) // 현재 턴이 자신의 턴이 아닐 경우
                {
                    HighLightOffEvent(); // 하이라이트 끄기
                    return; // 반환
                }

                // 현재 턴이 메인 턴이 아니라면
                if (!WarningEvent.OnCheckCurrentTurn.Invoke(TurnType.MainTurn, "메인 턴이 아니라서 기물을 배치할 수 없습니다."))
                {
                    HighLightOffEvent(); // 하이라이트 끄기
                    return; // 반환
                }

                if (!WarningEvent.OnCheckLeftPieceCount(_leftPieceCount, "남은 기물이 없어 배치할 수 없습니다")) // 남은 도로가 없다면
                {
                    HighLightOffEvent(); // 하이라이트 끄기
                    return; // 반환
                }

                if (!WarningEvent.OnCanPayCost.Invoke(_cost, "금괴가 부족하여 기물을 배치할 수 없습니다.")) // 비용을 지불할 수 없다면
                {
                    HighLightOffEvent(); // 하이라이트 끄기
                    return; // 반환
                }

                ObjectPlace(); // 기물 배치 함수 실행
            }
        }

        // 객체를 이동하는 기능 함수
        private void ObjectMove()
        {
            if (!WarningEvent.OnCanMovePiece.Invoke(CanPlacePieceType))
            {
                HighLightEvents.OnPieceMovementHighLight?.Invoke(false, false);
                return;
            }

            GameManager.Instance.PieceCanMoveMap[CanPlacePieceType] = false; // 현재 이동하는 타입의 기물을 이후로는 같은 타입의 기물 이동이 불가한 상태로 할당
            PlacePiece(GameManager.Instance.CurrentMovePiece, true); // 기물 이동
        }

        // 객체를 배치하는 기능 함수
        private void ObjectPlace()
        {
            GameManager.Instance.CanMakePiece = false;
            Transform pieceParent = _pieceMap[CanPlacePieceType]; // 현재 배치 가능한 타입의 객체 부모
            int pieceCount = pieceParent.childCount; // 현재 보유 중인 배치 가능한 타입의 기물 수

            PlacePiece(pieceParent.GetChild(pieceCount - 1).gameObject, false); // 기물 배치
        }

        private void PlacePiece(GameObject pieceObj, bool isMove)
        {
            PieceBase pieceBase = pieceObj.GetComponent<PieceBase>(); // 객체의 PieceBase를 가져오기

            if (pieceBase != null) // null 체크
            {
                if(isMove) // 이동 상태이고
                {
                    if(pieceBase.CurrentPlacePlane != null) // 움직일 기물이 배치 되어 있는 칸이 있을 때
                    {
                        pieceBase.CurrentPlacePlane.PlacedObjectType = ObjectType.None; // 움직일 기물이 배치 되어있던 칸을 빈 칸으로 초기화
                        pieceBase.CurrentPlacePlane.TeamType = TeamType.None; // 팀도 아무 팀도 아닌 상태로 초기화
                    }
                }
                pieceBase.CurrentPlacePlane = this; // 현재 기물이 올라가 있는 배치 칸을 자기 자신으로 할당

                UIManager.Instance.CanInteractionUI = false; // UI 상호작용 불가능 상태로 할당
                PlacedObjectType = CanPlacePieceType; // 현재 배치가 가능한 기물을 이 배치판에 배치되어있는 기물로 지정
                TeamType = pieceBase.teamType; // 현재 기물 배치 칸의 팀 타입을 기물의 팀 타입으로 할당

                PieceInfo pieceInfo = new PieceInfo()
                {
                    roomID = SceneMgr.Instance.CurrentRoomID, // 현재 방 ID
                    pieceID = pieceBase.Id, // 기물 객체 ID
                    placePlaneID = _id, // 배치 칸 ID
                    parentName = transform.parent.name, // 부모 객체 명
                    placedObjectType = (int)CanPlacePieceType, // 기물 객체 타입
                    targetPos = transform.localPosition, // 기물 객체 최종 위치
                    isMove = isMove // 생성인지 이동인지 여부
                }; 
                string json = JsonUtility.ToJson(pieceInfo); // Json으로 변환
                NetworkManager.Instance.Socket.Emit("movePiece", json); // 서버에 movePiece 이벤트 전달
                pieceBase.MoveToPlacePlane(transform.parent, transform.localPosition); // 기물을 현재 배치판의 부모 자식으로 변경, 기물을 현재 배치할 배치 판의 위치로 이동

                _ = FindCanPlacePlane();

                HighLightOffEvent(); // 하이라이트 끄기
            }
        }
    }
}
// 마지막 작성 일자: 2025.09.04