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

            if(roadPiece != null)
            {
                Debug.Log($"그래서 배치 시 배치 가능한 건? {CanPlacePieceTypeProp}");
                PlacedObjectTypeProp = CanPlacePieceTypeProp; // 배치 성공 시 배치 가능한 기물이 위에 배치 되었다고 할당
                Debug.Log($"그래서 배치 시 배치 된 건? {CanPlacePieceTypeProp}");
                roadPiece.MoveToPlacePlane(transform.parent, transform.localPosition, _roadAngle); // 기물을 현재 배치 판 부모의 자식으로 변경 + 현재 이 배치판 위치 이동 + 각도 회전
                HighLightEventSystem.OnRoadHighLight?.Invoke(false); // 도로 칸 하이라이트를 끄는 매개변수로 이벤트 콜
            }
        }
        // 이거 시작했을 때 리스트에 있던 값들 사라짐
    }
}
// 마지막 작성 일자: 2025.07.09