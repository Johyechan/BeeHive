using InGame.MyEnum;
using InGame.MyEvent;
using InGame.MyManager;
using InGame.MyManager.MyPiece;
using InGame.MyManager.MyPlacePlane;
using InGame.MyObject.Handler;
using InGame.MyObject.Piece.Data;
using InGame.MyUI;
using InGame.MyUI.Card;
using System.Threading.Tasks;
using UnityEngine;

namespace InGame.MyObject.Piece.Handler
{
    // 작성자: 조혜찬
    // 공격 받는 기능 핸들러 클래스
    public class PieceAttackedHandler
    {
        private PieceBase _pieceBase; // 기물 클래스

        private PieceData _pieceData; // 불변 변수를 가지는 구조체

        public PieceAttackedHandler(PieceBase pieceBase, PieceData pieceData)
        {
            _pieceBase = pieceBase;
            _pieceData = pieceData;
        }

        // 배치 칸 비활성화
        private async Task HighLightOffFunction(bool changeFirePowerAttack)
        {
            HighLightEvents.OnPieceMovementHighLight?.Invoke(false, false); // 하이라이트 끄기, 이동 가능 배치 칸 대상
            HighLightEvents.OnRoadPlacementHighLight?.Invoke(false); // 도로 배치 칸 하이라이트 끄기
            HighLightEvents.OnPiecePlacementHighLight?.Invoke(false, true); // 기물 배치 칸 하이라이트 끄기, 배치 가능 배치 판 대상
            await PieceEvents.OnHideCanAttackPieces?.Invoke(changeFirePowerAttack); // 공격 가능한 기물들 하이라이트 끄기
        }

        public async Task PieceAttacked()
        {
            PieceBase attackPieceBase = GameManager.Instance.CurrentMovePiece.GetComponent<PieceBase>(); // 공격한 객체의 PieceBase 가져오기

            if (!await WarningEvent.OnCanMovePiece?.Invoke(attackPieceBase.CurrentObjectType, true)) // 이미 이동 또는 공격을 했던 기물과 같은 타입의 기물이 공격 했었다면
            {
                await HighLightOffFunction(true);

                await Task.CompletedTask; // 테스크 종료
                return; // 함수 종료
            }

            if (attackPieceBase.CurrentObjectType == ObjectType.Tank) // 공격한 기물이 전차일 경우
            {
                if (_pieceBase.PieceVariable.isFirePowerAttackTarget) // 공격 받은 기물이 원거리 공격 대상이라면
                {
                    if (CardManager.Instance.HaveFirePowerCard) // 공격한 기물의 팀이 화력 카드를 가지고 있다면
                    {
                        await HighLightOffFunction(false); // 배치칸 비활성화
                        _pieceData.confirmUI = Object.FindAnyObjectByType<ConfirmUI>(FindObjectsInactive.Include);
                        _pieceData.confirmUI.gameObject.SetActive(true); // 객체 활성화

                        bool result = await _pieceData.confirmUI.Confirm();
                        if (!result) // 결과가 거짓이라면
                        {
                            await HighLightOffFunction(true);
                            await Task.CompletedTask; // 테스크 종료
                            return; // 함수 종료
                        }

                        if (_pieceBase.CurrentObjectType == ObjectType.Tank) // 만약 공격 받는 기물도 전차라면
                        {
                            NetworkManager.Instance.Socket.Emit("tankAttackedTank", SceneMgr.Instance.CurrentRoomID); // 상대 전차를 공격했다고 서버로 이벤트 호출

                            bool opponentChooseDefense = await PieceManager.Instance.OpponentChoice() == 1 ? true : false; // 상대가 결정할 때까지 대기

                            if (opponentChooseDefense) // 상대가 방어를 했다면
                            {
                                GameManager.Instance.PieceCanMoveMap[attackPieceBase.CurrentObjectType] = false; // 공격한 기물이 이동 한 것으로 판정

                                await HighLightOffFunction(true);

                                UICardBase uiCardBase = CardManager.Instance.FindFirePowerCard(); // 자신의 패에서 화력 카드 탐색

                                if (uiCardBase == null) // 자신의 패에 화력 카드가 없다면
                                {
                                    CardManager.Instance.HaveFirePowerCard = false; // 화력 카드가 없는 상태로 전환
                                }

                                await PieceManager.Instance.FindCanPlacePlane(); // 재탐색
                                await Task.CompletedTask; // 테스크 종료
                                return; // 함수 종료
                            }
                        }
                    }
                }
                
            }

            int isFirePowerAttack = _pieceBase.PieceVariable.isFirePowerAttackTarget ? 1 : 0; // 원거리 공격 여부 할당(1: 참, 0: 거짓)

            int attackObjID = attackPieceBase.PieceVariable.id; // 공격한 객체의 ID
            int returnObjID = _pieceBase.PieceVariable.id; // 공격 받은 객체의 ID

            Transform returnParent = null; // 공격 받은 기물의 부모 객체
            Transform returnPieceTrans = ObjectIdManager.Instance.FindObject(returnObjID).transform; // 공격 받은 기물의 트랜스폼

            switch (_pieceData.currentObjectType) // 배치 가능한 타입(즉 객체 타입)
            {
                case ObjectType.Miner:
                    returnParent = TeamManager.Instance.GetMinerTransform(_pieceData.teamType); // 기물의 팀 타입의 부모 할당
                    break;
                case ObjectType.Soldier:
                    returnParent = TeamManager.Instance.GetSoldierTransform(_pieceData.teamType); // 기물의 팀 타입의 부모 할당
                    break;
                case ObjectType.Tank:
                    returnParent = TeamManager.Instance.GetTankTransform(_pieceData.teamType); // 기물의 팀 타입의 부모 할당
                    break;
            }

            Vector3 returnPos = new Vector3(_pieceData.xInterval * returnParent.childCount, 0, 0); // 공격 당한 기물의 목적지
            Vector3 attackPos = returnPieceTrans.localPosition; // 공격한 기물의 목적지

            AttackInfo attackInfo = new AttackInfo()
            {
                roomID = SceneMgr.Instance.CurrentRoomID, // 현재 방 ID
                returnPieceID = returnObjID, // 공격 당한 기물 ID
                returnPos = returnPos, // 공격 당한 기물의 목적지
                returnParentName = returnParent.name, // 공격 당한 기물의 부모 객체 명
                attackPieceID = attackObjID, // 공격한 기물 ID
                attackPos = attackPos, // 공격한 기물의 목적지
                isFirePowerAttack = isFirePowerAttack, // 원거리 공격 여부 할당(1: 참, 0: 거짓)
            };

            string json = JsonUtility.ToJson(attackInfo);

            NetworkManager.Instance.Socket.Emit("attackPiece", json);

            await PieceManager.Instance.AttackRelatedPiecesMove(_pieceBase, attackPieceBase, returnParent, returnPieceTrans.parent, returnPos, attackPos); // 공격 당한 기물과 공격한 기물이 이동하는 함수
        }
    }
}
// 마지막 작성 일자: 2025.11.03