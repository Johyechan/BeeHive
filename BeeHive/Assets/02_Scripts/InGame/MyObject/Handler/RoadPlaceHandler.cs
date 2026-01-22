using InGame.MyEnum;
using InGame.MyEvent;
using InGame.MyManager;
using InGame.MyManager.MyPiece;
using InGame.MyManager.MyPlacePlane;
using InGame.MyObject.Piece;
using System.Threading.Tasks;
using UnityEngine;

namespace InGame.MyObject.Handler
{
    // 작성자: 조혜찬
    // 도로 배치 기능 핸들러
    public class RoadPlaceHandler
    {
        public async Task Place(RoadPlacePlaneObject roadPlacePlane, PieceBase roadPiece, Transform roadParent, float roadAngle)
        {
            UIManager.Instance.CanInteractionUI = false; // UI 상호작용 불가능 상태로 할당

            PlacePlaneManager.Instance.ChangePlacePlaneState(roadPlacePlane, roadPiece, false); // 현재 배치칸 상태 변경

            RoadInfo roadInfo = new RoadInfo()
            {
                roomID = SceneMgr.Instance.CurrentRoomID, // 현재 방 ID
                roadID = roadPiece.PieceVariable.id, // 도로 객체 ID
                placePlaneId = roadPlacePlane.Id, // 현재 객체 ID
                placedType = (int)roadPlacePlane.CanPlacePieceType, // 배치 객체 타입
                roadTeamType = (int)roadPlacePlane.TeamType, // 배치 객체 팀 타입
                roadParentName = roadParent.name, // 부모 객체 이름
                targetParentName = roadPlacePlane.transform.parent.name, // 부모 객체 이름
                targetPos = roadPlacePlane.transform.localPosition, // 최종 위치
                angle = roadAngle // 최종 각도
            };
            string json = JsonUtility.ToJson(roadInfo); // Json으로 변환
            NetworkManager.Instance.Socket.Emit("makeRoad", json);

            HighLightEvents.OnRoadPlacementHighLight?.Invoke(false); // 도로 칸 하이라이트를 끄는 매개변수로 이벤트 콜

            await roadPiece.MoveToPlacePlane(roadPlacePlane.transform.parent, roadPlacePlane.transform.localPosition, roadAngle); // 기물을 현재 배치 판 부모의 자식으로 변경 + 현재 이 배치판 위치 이동 + 각도 회전

            PieceManager.Instance.FindCanPlacePlane();

            if (NetworkManager.Instance.IsClientOver) // 클라이언트가 종료 되었다면
                return; // 반환

            UIEvents.OnSetLeftPieceText?.Invoke(); // 남은 기물 수 변경
        }
    }
}
// 마지막 작성 일자: 2026.01.22