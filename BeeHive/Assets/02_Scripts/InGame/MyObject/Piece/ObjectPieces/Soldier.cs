using InGame.MyEnum;
using InGame.MyEvent;
using InGame.MyManager.Global;
using InGame.MyManager.Local;
using MyUtil.GameMode;
using MyUtil.MyObjectPool;
using UnityEngine;

namespace InGame.MyObject.Piece.ObjectPieces
{
    // 작성자: 조혜찬
    // 보병 기물 클래스
    public class Soldier : PieceBase
    {
        protected override void Awake()
        {
            base.Awake();

            ParentSet();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            PieceEvents.OnChangeNearRoad += NearRoadChange; // 주위 도로 변경 이벤트 구독
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            PieceEvents.OnChangeNearRoad -= NearRoadChange; // 주위 도로 변경 이벤트 구독 해제
        }

        // 부모 초기화 함수
        private void ParentSet()
        {
            PieceVariable.parent = TeamManager.Instance.GetSoldierTransform(TeamManager.Instance.CurrentTeamType); // 보병 객체의 부모 할당
        }

        private void NearRoadChange(PieceBase pieceBase, TeamType type, PiecePlacePlaneObject piecePlacePlaneObject)
        {
            if (pieceBase.NetworkId != NetworkId) // 자기자신이 부른 게 아닐경우 - 왜?
            {
                return; // 반환
            }

            UIManager.Instance.CanInteractionUI = false;

            foreach (var nearRoad in piecePlacePlaneObject.nearRoadPlaceTransformList)
            {
                if(nearRoad.TeamType != type && nearRoad.TeamType != TeamType.None)
                {
                    switch (type)
                    {
                        case TeamType.Team1:
                            ChangeRoad(ObjectPoolType.Team1Road, nearRoad); // 도로 변경
                            break;
                        case TeamType.Team2:
                            ChangeRoad(ObjectPoolType.Team2Road, nearRoad); // 도로 변경
                            break;
                    }
                }
            }

            UIManager.Instance.CanInteractionUI = true;
        }

        // 도로 변경 함수
        private void ChangeRoad(ObjectPoolType roadType, RoadPlacePlaneObject roadPlacePlaneObject)
        {
            if(roadPlacePlaneObject.IsUpdating) // 업데이트 중이라면
            {
                return; // 반환
            }

            roadPlacePlaneObject.IsUpdating = true; // 업데이트 시작
            float targetAngle = roadPlacePlaneObject.PlacedPiece.gameObject.transform.rotation.eulerAngles.y;
            ObjectPoolManager.Instance.ReturnObject(roadType, roadPlacePlaneObject.PlacedPiece.gameObject, true); // 기존 도로 객체 반환

            roadPlacePlaneObject.PlacedObjectType = ObjectType.None;
            roadPlacePlaneObject.TeamType = TeamType.None;
            roadPlacePlaneObject.PlacedPiece = null;

            if (GameModeManager.Instance.CurrentGameMode.IsTutorial()) // 튜토리얼 일 경우
            {
                GameObject road = ObjectPoolManager.Instance.GetObject(roadType, roadPlacePlaneObject.transform.parent);
                road.transform.localPosition = new Vector3(roadPlacePlaneObject.transform.localPosition.x, ObjectPoolManager.Instance.AnimationYPos, roadPlacePlaneObject.transform.localPosition.z);
                road.transform.Rotate(0, targetAngle, 0);
                ObjectPoolManager.Instance.Animation(road, true, true, roadPlacePlaneObject.transform.localPosition.y);
                PieceBase pieceBase = road.GetComponent<PieceBase>();
                InGameContext.Current.Data.PlacePlaneManager.ChangePlacePlaneState(roadPlacePlaneObject, pieceBase, false); // 배치칸 상태 변경
                InGameContext.Current.Data.PlacePlaneManager.FindCanPlacePlane();
            }
            else // 튜토리얼이 아닐 경우
            {
                ObjectPoolManager.Instance.MakeObject(roadType, roadPlacePlaneObject.transform.localPosition, roadPlacePlaneObject.transform.parent, true, roadPlacePlaneObject.NetworkId, targetAngle);
            }
        }

        public override void ObjectClicked()
        {
            base.ObjectClicked();
        }
    }
}
// 마지막 작성 일자: 2026.04.23