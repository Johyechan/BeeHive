using InGame.MyEnum;
using InGame.MyManager;
using InGame.MyObject;
using MyUtil.MyObjectPool;
using UnityEngine;

namespace InGame.MySystem
{
    // 작성자: 조혜찬
    // 도로 세팅 핸들 클래스
    public class SetRoadHandle
    {
        public void SetRoad(int placePlaneId, int placedType, int roadTeamType, string roadParentName, string targetParentName, Vector3 targetPos, float angle)
        {
            GameObject newRoad = null;
            GameObject roadParent = GameObject.Find(roadParentName); // 도로 부모 객체 탐색
            GameObject targetParent = GameObject.Find(targetParentName); // 최종 위치의 부모 객체 탐색
            GameObject plane = ObjectIdManager.Instance.FindObject(placePlaneId); // 배치 칸 탐색
            
            switch ((TeamType)roadTeamType) // 생성된 도로 팀에 따라 도로 팀 결정
            {
                case TeamType.Team1:
                    newRoad = ObjectPoolManager.Instance.GetObject(ObjectPoolType.Team1Road, roadParent.transform); // 새로운 도로 기물 생성
                    break;
                case TeamType.Team2:
                    newRoad = ObjectPoolManager.Instance.GetObject(ObjectPoolType.Team2Road, roadParent.transform); // 새로운 도로 기물 생성
                    break;
                case TeamType.Team3:
                    newRoad = ObjectPoolManager.Instance.GetObject(ObjectPoolType.Team3Road, roadParent.transform); // 새로운 도로 기물 생성
                    break;
            }

            newRoad.SetActive(false);
            newRoad.transform.localPosition = Vector3.zero;
            newRoad.SetActive(true);

            PieceBase roadPiece = newRoad.GetComponent<PieceBase>();
            PlacePlaneObjectBase placePlaneBase = plane.GetComponent<PlacePlaneObjectBase>();

            placePlaneBase.PlacedObjectType = (ObjectType)placedType; // 배치 성공 시 배치된 객체가 배치되었다고 할당
            placePlaneBase.TeamType = roadPiece.teamType; // 현재 배치 가능한 칸의 팀 타입을 도로 기물의 팀 타입으로 지정
            roadPiece.MoveToPlacePlane(targetParent.transform, targetPos, angle); // 기물을 현재 배치 판 부모의 자식으로 변경 + 현재 이 배치판 위치 이동 + 각도 회전
        }
    }
}
// 마지막 작성 일자: 2025.09.03