using InGame.MyEnum;
using InGame.MyManager;
using InGame.MyObject;
using UnityEngine;

namespace InGame.MySystem.Game
{
    // 작성자: 조혜찬
    // 기물 및 도로 위치 이동 핸들
    public class SetPieceHandle
    {
        public void SetPiece(int pieceID, int placePlaneID, string parentName, int placedObjectType, Vector3 targetPos, bool isMove, float angle = 0)
        {
            GameObject piece = ObjectIdManager.Instance.FindObject(pieceID); // 기물 탐색
            GameObject plane = ObjectIdManager.Instance.FindObject(placePlaneID); // 배치 칸 탐색

            if (piece == null || plane == null) // 기물 또는 배치 칸이 없다면
                return; // 반환

            PieceBase pieceBase = piece.GetComponent<PieceBase>(); // 기물 또는 도로 이동을 위해서 객체에서 PieceBase 클래스 가져오기
            PlacePlaneObjectBase placePlane = plane.GetComponent<PlacePlaneObjectBase>(); // 기물에게 현재 어떤 칸에 올라가 있는지 알려주기 위해서PlacePlaneObjectBase 클래스 가져오기

            if(isMove) // 이동이라면
            {
                if(pieceBase.CurrentPlacePlane != null) // 현재 배치된 칸이 존재할 때
                {
                    pieceBase.CurrentPlacePlane.PlacedObjectType = ObjectType.None; // 현재 배치된 칸에 올라가 있는 기물을 삭제
                    pieceBase.CurrentPlacePlane.TeamType = TeamType.None; // 팀도 아무 팀도 아닌 상태로 초기화
                }
            }
            pieceBase.CurrentPlacePlane = (PiecePlacePlaneObject)placePlane; // 기물에게 PiecePlacePlaneObject 형식으로 현재 배치된 칸 할당

            pieceBase.CurrentPlacePlane.PlacedObjectType = (ObjectType)placedObjectType; // 현재 기물
            placePlane.TeamType = pieceBase.teamType; // 배치 칸의 팀을 기물의 팀으로 할당
            placePlane.PlacedPiece = pieceBase; // 배치 칸에 배치된 기물 객체 할당

            GameObject parent = GameObject.Find(parentName); // 부모 객체 찾기
            pieceBase.MoveToPlacePlane(parent.transform, targetPos, angle); // 기물 또는 도로 이동
        }
    }
}
// 마지막 작성 일자: 2025.09.11