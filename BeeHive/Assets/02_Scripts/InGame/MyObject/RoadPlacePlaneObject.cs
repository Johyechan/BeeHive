using InGame.MyEvent;
using InGame.MyManager;
using InGame.MyObject.MyObjectEnum;
using MyUtil.MyObjectPool;
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

            _roadParent = GameObject.Find("PlayerRoad").transform; // 도로 기물의 부모 탐색 후 할당
        }

        // 클릭 시 실행될 함수
        public override void ObjectClicked()
        {
            GameObject newRoad = ObjectPoolManager.Instance.GetObject(ObjectPoolType.Road, _roadParent); // 새로운 도로 기물 생성
            newRoad.SetActive(false);
            newRoad.transform.localPosition = Vector3.zero;
            newRoad.SetActive(true);
            PieceBase roadPiece = newRoad.GetComponent<PieceBase>();
            // 현재 턴의 팀 타입으로 roadPiece 팀 타입 결정
            roadPiece.teamType = TeamType.Team1; // 임시

            if (roadPiece != null)
            {
                UIManager.Instance.CanInteractionUI = false; // UI 상호작용 불가능 상태로 할당
                PlacedObjectTypeProp = CanPlacePieceTypeProp; // 배치 성공 시 배치 가능한 기물이 위에 배치 되었다고 할당
                TeamTypeProp = roadPiece.teamType; // 현재 배치 가능한 칸의 팀 타입을 도로 기물의 팀 타입으로 지정
                roadPiece.MoveToPlacePlane(transform.parent, transform.localPosition, _roadAngle); // 기물을 현재 배치 판 부모의 자식으로 변경 + 현재 이 배치판 위치 이동 + 각도 회전
                HighLightEventSystem.OnRoadHighLightUIAction?.Invoke(false); // 도로 칸 하이라이트를 끄는 매개변수로 이벤트 콜
            }
        }
        // 이거 시작했을 때 리스트에 있던 값들 사라짐
    }
}
// 마지막 작성 일자: 2025.07.21