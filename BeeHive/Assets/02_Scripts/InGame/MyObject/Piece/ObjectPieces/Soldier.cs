using InGame.MyEnum;
using InGame.MyEvent;
using InGame.MyManager;
using InGame.MyManager.Global;
using InGame.MyManager.Local;
using InGame.MyManager.MyPiece;
using InGame.MyManager.MyPlacePlane;
using MyUtil.MyObjectPool;
using System.Threading.Tasks;
using Unity.VisualScripting;
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
                        case TeamType.Team3:
                            ChangeRoad(ObjectPoolType.Team3Road, nearRoad); // 도로 변경
                            break;
                    }
                }
            }

            UIManager.Instance.CanInteractionUI = true;
        }

        // 도로 변경 함수
        private void ChangeRoad(ObjectPoolType type, RoadPlacePlaneObject roadPlacePlaneObject)
        {
            float targetAngle = roadPlacePlaneObject.PlacedPiece.gameObject.transform.rotation.eulerAngles.y;
            switch(roadPlacePlaneObject.PlacedPiece.CurrentTeamType) // 기존 도로의 팀 타입
            {
                case TeamType.Team1:
                    ObjectPoolManager.Instance.ReturnObject(ObjectPoolType.Team1Road, roadPlacePlaneObject.PlacedPiece.gameObject); // 기존 도로 객체 반환
                    break;
                case TeamType.Team2:
                    ObjectPoolManager.Instance.ReturnObject(ObjectPoolType.Team2Road, roadPlacePlaneObject.PlacedPiece.gameObject); // 기존 도로 객체 반환
                    break;
                case TeamType.Team3:
                    ObjectPoolManager.Instance.ReturnObject(ObjectPoolType.Team3Road, roadPlacePlaneObject.PlacedPiece.gameObject); // 기존 도로 객체 반환
                    break;
            }

            ObjectPoolManager.Instance.MakeObject(type, roadPlacePlaneObject.transform.localPosition, roadPlacePlaneObject.transform.parent, roadPlacePlaneObject.NetworkId, targetAngle);
        }

        public override void ObjectClicked()
        {
            base.ObjectClicked();
        }
    }
}
// 마지막 작성 일자: 2026.02.13