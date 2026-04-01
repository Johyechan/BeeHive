using InGame.MyEnum;
using InGame.MyEvent;
using InGame.MyManager.Global;
using InGame.MyManager.Local;
using InGame.MyManager.MyPlacePlane;
using InGame.MyObject;
using InGame.MyObject.Piece;
using MyUtil.GameMode;
using System.Collections;
using System.Threading.Tasks;
using Tutorial;
using Tutorial.MyEnum;
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
            int isFirePowerAttack = returnPiece.PieceVariable.isFirePowerAttackTarget ? 1 : 0; // 원거리 공격 여부 할당(1: 참, 0: 거짓)
            bool isNearToCastle = returnPiece.PieceVariable.currentPlacePlane.isNearToCastle; // 공격 당하는 기물이 성 주위에 배치되어있다면
            TeamType attackedTeam = returnPiece.CurrentTeamType; // 공격 당한 기물의 배치 칸의 팀 타입 저장

            InGameContext.Current.Data.GameManager.PieceCanMoveMap[attackPiece.CurrentObjectType] = false; // 공격 시 이동 한 것으로 판정

            HighLightEvents.OnPieceMovementHighLight?.Invoke(false, false); // 하이라이트 끄기, 이동 가능 배치 칸 대상
            HighLightEvents.OnRoadPlacementHighLight?.Invoke(false); // 도로 배치 칸 하이라이트 끄기
            HighLightEvents.OnPiecePlacementHighLight?.Invoke(false, true); // 기물 배치 칸 하이라이트 끄기, 배치 가능 배치 판 대상
            PieceEvents.OnHideCanAttackPieces?.Invoke(true); // 공격 가능한 기물들 하이라이트 끄기

            UIManager.Instance.CanInteractionUI = false; // UI 상호작용 불가능 상태로 할당

            if(isFirePowerAttack == 0) // 공격 받은 기물이 원거리 공격 대상이 아닐 경우
            {
                InGameContext.Current.Data.PlacePlaneManager.ChangePlacePlaneState(returnPiece.PieceVariable.currentPlacePlane, attackPiece, true); // 현재 배치칸 상태 변경
            }
            else // 공격 받은 기물이 원거리 공격 대상일 경우
            {
                returnPiece.PieceVariable.currentPlacePlane.PlacedObjectType = ObjectType.None;
                if(!isNearToCastle) // 성 주위 배치칸이 아닐 때
                {
                    returnPiece.PieceVariable.currentPlacePlane.TeamType = TeamType.None; // 배치 칸에 올라가 있는 팀 상태 변경
                }
                returnPiece.PieceVariable.currentPlacePlane.PlacedPiece = null;
            }

            returnPiece.PieceVariable.currentPlacePlane = null; // 공격 받은 기물의 배치된 칸을 null로 초기화

            await returnPiece.MoveToPlacePlane(returnParent, returnPos); // 공격 받은 기물 이동
            if(isFirePowerAttack == 0) // 공격 받은 기물이 원거리 공격 대상이 아닐 경우
            {
                await attackPiece.MoveToPlacePlane(attackParent, attackPos); // 공격한 기물 이동
            }

            if (attackPiece.CurrentObjectType == ObjectType.Soldier)
            {
                if (attackPiece.CurrentTeamType == TeamManager.Instance.CurrentTeamType) // 공격한 기물이 현재 팀의 기물일 경우에만
                {
                    if (GameModeManager.Instance.CurrentGameMode.UseServer()) // 게임 서버를 사용하는 경우
                    {
                        PieceChangeRoadInfo pieceChangeRoadInfo = new PieceChangeRoadInfo
                        {
                            roomID = SceneMgr.Instance.CurrentRoomID, // 현재 방 ID
                            teamType = (int)attackPiece.CurrentTeamType, // 공격한 기물 팀 타입
                            placePlaneID = attackPiece.PieceVariable.currentPlacePlane.NetworkId, // 공격한 기물의 목적지 칸의 ID
                            pieceID = attackPiece.NetworkId // 주위 도로를 변경 시킬 기물 ID
                        };

                        string json = JsonUtility.ToJson(pieceChangeRoadInfo);

                        NetworkManager.Instance.Socket.Emit("pieceChangeRoad", json);
                    }

                    if (NetworkManager.Instance.IsClientOver) // 클라이언트가 종료 되었다면
                        return; // 반환

                    PieceEvents.OnChangeNearRoad?.Invoke(attackPiece, attackPiece.CurrentTeamType, attackPiece.PieceVariable.currentPlacePlane); // 도로 변경 이벤트 호출
                }
            }

            if(isNearToCastle) // 성 주위가 공격 당했다면
            {
                NetworkManager.Instance.Socket.Emit("debug", $"공격 당한 팀: {attackedTeam}");
                Castle castle = TeamManager.Instance.GetCastle(attackedTeam); // 공격 당한 팀 성 가져오기
                NetworkManager.Instance.Socket.Emit("debug", $"공격 당한 성: {castle}");
                NetworkManager.Instance.Socket.Emit("debug", $"공격하는 기물: {attackPiece}");
                castle.CastleHit(attackPiece.Damage); // 성에 피해주기
                attackPiece.PieceDestroy();
            }

            if (GameModeManager.Instance.CurrentGameMode.IsTutorial()) // 튜토리얼 일 때
            {
                switch (TutorialManager.Instance.CurrentTutorialState) // 현재 튜토리얼 상태가
                {
                    case TutorialState.Turn4_Player:
                        TutorialManager.Instance.SetTutorialPanel(true, "보병과 광부는 원거리 공격을 당하면 즉시 파괴됩니다. \n 도로를 하나만 더 배치합시다.", "버튼 클릭", 0.1f, 0.008f, new Vector4(0.356f, 0.123f), new Vector4(0.5f, 0.3f));
                        break;
                    case TutorialState.Turn5_Player:
                        TutorialManager.Instance.SetTutorialPanel(true, "상대가 전차간의 싸움에서 화력을 사용하지 않는다면 전차 또한 즉시 파괴됩니다. \n 다음 턴을 눌러 턴을 종료합시다.", "버튼 클릭", 0.18f, 0.008f, new Vector4(0.92f, 0.095f), new Vector4(0.66f, 0.4f));
                        break;
                    case TutorialState.Turn6_Player:
                        TutorialManager.Instance.SetTutorialPanel(true, "보병과 광부는 원거리 공격을 당하면 즉시 파괴됩니다. \n 이제 광부를 이동합시다.", "대상 클릭", 0.08f, 0.008f, new Vector4(0.371f, 0.382f), new Vector4(0.3f, 0.3f));
                        break;
                    case TutorialState.Turn7_Player:
                        TutorialManager.Instance.SetTutorialPanel(true, "상대 팀의 생성 위치에 공격을 가하면 상대 성에 피해를 줄 수 있습니다(상대 기물이 올려져 있다면 상대 기물이 파괴됨과 동시에 상대 성에 데미지도 가합니다.)\n이제 보병을 움직입시다.", "대상 클릭", 0.08f, 0.008f, new Vector4(0.454f, 0.576f), new Vector4(0.3f, 0.3f), new Vector2(0, 250f));
                        break;
                }
            }

            InGameContext.Current.Data.PieceManager.FindCanPlacePlane(); // 다시 이동가능한 위치 찾기
        }
    }
}
// 마지막 작성 일자: 2026.03.30