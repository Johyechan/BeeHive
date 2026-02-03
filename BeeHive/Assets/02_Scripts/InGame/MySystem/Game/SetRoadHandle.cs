using InGame.MyEnum;
using InGame.MyManager;
using InGame.MyManager.Global;
using InGame.MyObject;
using InGame.MyObject.Piece;
using MyUtil.MyObjectPool;
using System.Threading.Tasks;
using UnityEngine;

namespace InGame.MySystem
{
    // 작성자: 조혜찬
    // 도로 세팅 핸들 클래스
    public class SetRoadHandle
    {
        public async Task SetRoad(int roadID, int placePlaneId, int placedType, int roadTeamType, string roadParentName, string targetParentName, Vector3 targetPos, float angle)
        {
            GameObject newRoad = ObjectIdManager.Instance.FindObject(roadID); // 도로 객체 탐색
            GameObject roadParent = GameObject.Find(roadParentName); // 도로 부모 객체 탐색
            GameObject targetParent = GameObject.Find(targetParentName); // 최종 위치의 부모 객체 탐색
            GameObject plane = ObjectIdManager.Instance.FindObject(placePlaneId); // 배치 칸 탐색

            PieceBase roadPiece = newRoad.GetComponent<PieceBase>();
            PlacePlaneObjectBase placePlaneBase = plane.GetComponent<PlacePlaneObjectBase>();

            roadPiece.PieceVariable.currentRoadPlacePlane = placePlaneBase as RoadPlacePlaneObject;
            placePlaneBase.PlacedObjectType = (ObjectType)placedType; // 배치 성공 시 배치된 객체가 배치되었다고 할당
            placePlaneBase.TeamType = roadPiece.CurrentTeamType; // 현재 배치 가능한 칸의 팀 타입을 도로 기물의 팀 타입으로 지정
            placePlaneBase.PlacedPiece = roadPiece; // 배치된 기물에 도로 할당
            await roadPiece.MoveToPlacePlane(targetParent.transform, targetPos, angle); // 기물을 현재 배치 판 부모의 자식으로 변경 + 현재 이 배치판 위치 이동 + 각도 회전
        }
    }
}
// 마지막 작성 일자: 2026.02.03