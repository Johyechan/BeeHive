using InGame.MyEnum;
using InGame.MyEvent;
using InGame.MyManager.MyPlacePlane;
using InGame.MyObject.Piece;
using System.Threading.Tasks;
using UnityEngine;

namespace InGame.MyManager.MyPiece.Handler
{
    // 작성자: 조혜찬
    // 공격 관련 기물들을 이동하는 기능을 처리하는 핸들러
    public class AttackRelatedPiecesMoveHandler
    {
        // 공격 당한 기물과 공격한 기물이 이동하는 함수(공격 당한 기물, 공격한 기물공격 당한 기물의 부모, 공격한 기물의 부모, 공격 당한 기물의 목적지, 공격한 기물의 목적지)
        public async Task AttackRelatedPiecesMove(PieceBase returnPiece, PieceBase attackPiece, Transform returnParent, Transform attackParent, Vector3 returnPos, Vector3 attackPos)
        {
            HighLightEvents.OnPieceMovementHighLight?.Invoke(false, false); // 하이라이트 끄기, 이동 가능 배치 칸 대상
            HighLightEvents.OnRoadPlacementHighLight?.Invoke(false); // 도로 배치 칸 하이라이트 끄기
            HighLightEvents.OnPiecePlacementHighLight?.Invoke(false, true); // 기물 배치 칸 하이라이트 끄기, 배치 가능 배치 판 대상
            await PieceEvents.OnHideCanAttackPieces?.Invoke(); // 공격 가능한 기물들 하이라이트 끄기

            UIManager.Instance.CanInteractionUI = false; // UI 상호작용 불가능 상태로 할당

            await PlacePlaneManager.Instance.ChangePlacePlaneState(returnPiece.PieceVariable.currentPlacePlane, attackPiece, true); // 현재 배치칸 상태 변경

            returnPiece.PieceVariable.currentPlacePlane = null; // 공격 받은 기물의 배치된 칸을 null로 초기화

            await returnPiece.MoveToPlacePlane(returnParent, returnPos); // 공격 받은 기물 이동
            await attackPiece.MoveToPlacePlane(attackParent, attackPos); // 공격한 기물 이동

            if (attackPiece.CurrentObjectType == ObjectType.Soldier)
            {
                if (attackPiece.CurrentTeamType == TeamManager.Instance.CurrentTeamType) // 공격한 기물이 현재 팀의 기물일 경우에만
                {
                    ChangeRoadInfo changeRoadInfo = new ChangeRoadInfo
                    {
                        roomID = SceneMgr.Instance.CurrentRoomID, // 현재 방 ID
                        teamType = (int)attackPiece.CurrentTeamType, // 공격한 기물 팀 타입
                        placePlaneID = attackPiece.PieceVariable.currentPlacePlane.Id // 공격한 기물의 목적지 칸의 ID
                    };

                    string json = JsonUtility.ToJson(changeRoadInfo);

                    NetworkManager.Instance.Socket.Emit("changeRoad", json);

                    await PieceEvents.OnChangeNearRoad?.Invoke(attackPiece.CurrentTeamType, attackPiece.PieceVariable.currentPlacePlane); // 도로 변경 이벤트 호출
                }
            }

            await PieceManager.Instance.FindCanPlacePlane(); // 다시 이동가능한 위치 찾기
        }
    }
}
// 마지막 작성 일자: 2025.09.19