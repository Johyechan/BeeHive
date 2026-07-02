using InGame.MyEnum;
using InGame.MyEvent;
using InGame.MyManager;
using InGame.MyManager.Global;
using InGame.MyManager.Local;
using InGame.MyObject.Piece.Data;
using InGame.MyObject.Piece.ObjectPieces;
using InGame.MyUI;
using InGame.MyUI.Card;
using MyUtil.GameMode;
using System.Threading.Tasks;
using Tutorial;
using UnityEngine;
using UnityEngine.Localization.Settings;

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
        private void HighLightOffFunction(bool changeFirePowerAttack)
        {
            HighLightEvents.OnPieceMovementHighLight?.Invoke(false, false); // 하이라이트 끄기, 이동 가능 배치 칸 대상
            HighLightEvents.OnRoadPlacementHighLight?.Invoke(false); // 도로 배치 칸 하이라이트 끄기
            HighLightEvents.OnPiecePlacementHighLight?.Invoke(false, true); // 기물 배치 칸 하이라이트 끄기, 배치 가능 배치 판 대상
            PieceEvents.OnHideCanAttackPieces?.Invoke(changeFirePowerAttack); // 공격 가능한 기물들 하이라이트 끄기
        }

        public async Task PieceAttacked()
        {
            PieceBase attackPieceBase = InGameContext.Current.Data.GameManager.CurrentMovePiece.GetComponent<PieceBase>(); // 공격한 객체의 PieceBase 가져오기
            Tank tank = attackPieceBase as Tank;
            bool isRangedAttack = false; // 전차 원거리 공격 여부

            if (attackPieceBase.CurrentObjectType == ObjectType.Tank) // 공격한 기물이 전차일 경우
            {
                if (_pieceBase.PieceVariable.isFirePowerAttackTarget) // 공격 받은 기물이 원거리 공격 대상이라면
                {
                    UICardBase uiCardBase = InGameContext.Current.Data.CardManager.FindFirePowerCard();

                    // 뭔가 전에 행동을 한 전차가 현재 공격을 행한 전차가 아닌 동시에 null이 아닌 즉 이미 다른 전차가 행동을 한 상태라면
                    if (InGameContext.Current.Data.GameManager.DoSomethingTankID != tank.NetworkId && InGameContext.Current.Data.GameManager.DoSomethingTankID != -1)
                    {
                        // 공격 불가하다는 경고문 띄우기
                        string tankCanNotAttack = LocalizationSettings.StringDatabase.GetLocalizedString(
                            "Game",
                            "Game_UI_ThisTankCanNotDoSometing"
                        );
                        UIManager.Instance.WarningUIMake(tankCanNotAttack);

                        HighLightOffFunction(true);

                        await Task.CompletedTask; // 테스크 종료
                        return; // 함수 종료
                    }

                    // 공격한 기물의 팀이 화력 카드를 가지고 있으며 원거리 공격을 한 번도 안한 상태라면
                    if (uiCardBase != null && !InGameContext.Current.Data.GameManager.TankRangedAttacked) 
                    {
                        HighLightOffFunction(false); // 배치칸 비활성화
                        _pieceData.confirmUI = Object.FindAnyObjectByType<ConfirmUI>(FindObjectsInactive.Include);
                        _pieceData.confirmUI.gameObject.SetActive(true); // 객체 활성화
                        if(GameModeManager.Instance.CurrentGameMode.IsTutorial()) // 튜토리얼 일 경우
                        {
                            string letsAttack = LocalizationSettings.StringDatabase.GetLocalizedString(
                                "Tutorial",
                                "Tutorial_LetsAttack"
                            );
                            TutorialManager.Instance.SetTutorialPanel(true, letsAttack, TutorialManager.Instance.ButtonClick, 0.08f, 0.008f, new Vector4(0.422f, 0.224f), new Vector4(1.2f, 0.3f), new Vector2(0, 450f));
                        }
                        TaskCompletionSource<bool> confirmResultTcs = new TaskCompletionSource<bool>(); // 확인 결과를 가지는 tcs

                        string attack = LocalizationSettings.StringDatabase.GetLocalizedString(
                            "Game",
                            "Game_UI_AttackUseFirePower"
                        );

                        _pieceData.confirmUI.Confirm(result =>
                        {
                            if (GameModeManager.Instance.CurrentGameMode.IsTutorial()) // 튜토리얼 일 경우
                            {
                                TutorialManager.Instance.SetTutorialPanel(false);
                            }
                            _pieceData.confirmUI.ConfirmEnd(); // 확인 완료
                            confirmResultTcs.TrySetResult(result); // 확인 결과(result) 할당
                        }, attack);

                        bool result = await confirmResultTcs.Task; // 확인 대기

                        if (NetworkManager.Instance.IsClientOver) // 클라이언트가 종료 되었다면
                            return; // 반환

                        if (!result) // 결과가 거짓이라면
                        {
                            HighLightOffFunction(true);
                            await Task.CompletedTask; // 테스크 종료
                            return; // 함수 종료
                        }

                        isRangedAttack = true; // 전차 원거리 공격으로 판정
                        InGameContext.Current.Data.GameManager.TankRangedAttacked = true; // 원거리 공격한 것으로 판정
                        InGameContext.Current.Data.GameManager.DoSomethingTankID = tank.NetworkId; // 전차로 변환 후 할당

                        await InGameContext.Current.Data.CardManager.UsedCardShowOver?.Task; // 사용한 카드 보여주기가 끝날 때까지 대기

                        if (_pieceBase.CurrentObjectType == ObjectType.Tank) // 만약 공격 받는 기물도 전차라면
                        {
                            if (GameModeManager.Instance.CurrentGameMode.UseServer())
                            {
                                NetworkManager.Instance.Socket.Emit("tankAttackedTank", SceneMgr.Instance.CurrentRoomID); // 상대 전차를 공격했다고 서버로 이벤트 호출

                                InGameContext.Current.Data.PieceManager.FadeInOutWaitConfirmUI(1, InGameContext.Current.Data.PieceManager.WaitTime); // 대기 UI 활성화
                            }

                            bool opponentChooseDefense = GameModeManager.Instance.CurrentGameMode.UseServer() ? await InGameContext.Current.Data.PieceManager.OpponentChoice(InGameContext.Current.Data.PieceManager.WaitTime) == 1 ? true : false : false; // 상대가 결정할 때까지 대기(서버를 사용하지 않는 경우 바로 false 반환)

                            if (GameModeManager.Instance.CurrentGameMode.UseServer()) // 서버를 사용하는 경우
                            {
                                InGameContext.Current.Data.PieceManager.FadeInOutWaitConfirmUI(0); // 대기 UI 비활성화
                            }

                            if (NetworkManager.Instance.IsClientOver) // 클라이언트가 종료 되었다면
                                return; // 반환

                            if (opponentChooseDefense) // 상대가 방어를 했다면
                            {
                                InGameContext.Current.Data.GameManager.PieceCanMoveMap[attackPieceBase.CurrentObjectType] = false; // 공격한 기물이 이동 한 것으로 판정

                                HighLightOffFunction(true);

                                InGameContext.Current.Data.PieceManager.FindCanPlacePlane(); // 재탐색
                                await Task.CompletedTask; // 테스크 종료
                                return; // 함수 종료
                            }
                        }
                    }
                }
            }

            if (NetworkManager.Instance.IsClientOver) // 클라이언트가 종료 되었다면
                return; // 반환

            if (!isRangedAttack) // 전차 원거리 공격이 아닐 경우
            {
                if (!WarningEvent.OnCanMovePiece.Invoke(attackPieceBase.CurrentObjectType, true)) // 이미 이동 또는 공격을 했던 기물과 같은 타입의 기물이 공격 했었다면
                {
                    HighLightOffFunction(true);

                    await Task.CompletedTask; // 테스크 종료
                    return; // 함수 종료
                }

                if (tank != null)
                {
                    if (InGameContext.Current.Data.GameManager.DoSomethingTankID != tank.NetworkId && InGameContext.Current.Data.GameManager.DoSomethingTankID != -1)
                    {
                        // 공격 불가하다는 경고문 띄우기
                        string tankCanNotAttack = LocalizationSettings.StringDatabase.GetLocalizedString(
                            "Game",
                            "Game_UI_ThisTankCanNotDoSometing"
                        );
                        UIManager.Instance.WarningUIMake(tankCanNotAttack);

                        HighLightOffFunction(true);

                        await Task.CompletedTask; // 테스크 종료
                        return; // 함수 종료
                    }
                }
            }

            int isFirePowerAttack = isRangedAttack == true ? 1 : 0; // 원거리 공격 여부 할당(1: 참, 0: 거짓)
            NetworkManager.Instance.Socket.Emit("debug", $"처음 판단 할 때 원거리 공격 여부 isFirePowerAttack: {isFirePowerAttack}, 그리고 전차가 원거리 공격 했는가?: isRangedAttack: {isRangedAttack}");

            int attackObjID = attackPieceBase.NetworkId; // 공격한 객체의 ID
            int returnObjID = _pieceBase.NetworkId; // 공격 받은 객체의 ID

            if (NetworkManager.Instance.IsClientOver) // 클라이언트가 종료 되었다면
                return; // 반환

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

            Vector3 returnPos = new Vector3(0, 0, _pieceData.zInterval * returnParent.childCount); // 공격 당한 기물의 목적지
            Vector3 attackPos = returnPieceTrans.localPosition; // 공격한 기물의 목적지

            AttackInfo attackInfo = new AttackInfo()
            {
                roomID = SceneMgr.Instance.CurrentRoomID, // 현재 방 ID
                returnPieceID = returnObjID, // 공격 당한 기물 ID
                returnPos = returnPos,
                returnParentName = returnParent.name, // 공격 당한 기물의 부모 객체 명
                attackPieceID = attackObjID, // 공격한 기물 ID
                attackPos = attackPos,
                isFirePowerAttack = isFirePowerAttack, // 원거리 공격 여부 할당(1: 참, 0: 거짓)
            };

            NetworkManager.Instance.Socket.Emit("debug", $"서버에 보내기 직전 attackInfo.isFirePowerAttack: {attackInfo.isFirePowerAttack}");
            string json = JsonUtility.ToJson(attackInfo);
            if (GameModeManager.Instance.CurrentGameMode.UseServer())
                NetworkManager.Instance.Socket.Emit("attackPiece", json);

            await InGameContext.Current.Data.PieceManager.AttackRelatedPiecesMove(_pieceBase, attackPieceBase, returnParent, returnPieceTrans.parent, returnPos, attackPos, isFirePowerAttack); // 공격 당한 기물과 공격한 기물이 이동하는 함수
        }
    }
}
// 마지막 작성 일자: 2026.07.02