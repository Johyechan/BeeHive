using InGame.MyEnum;
using InGame.MyEvent;
using InGame.MyManager;
using InGame.MyManager.Global;
using InGame.MyManager.Local;
using InGame.MyManager.MyPlacePlane;
using InGame.MyObject.Piece.Data;
using MyUtil.GameMode;
using System.Threading.Tasks;
using Tutorial;
using Tutorial.MyEnum;
using UnityEngine;

namespace InGame.MyObject.Piece.Handler
{
    // 작성자: 조혜찬
    public class PieceSelectHandler
    {
        private PieceBase _pieceBase; // 기물 클래스

        private PieceData _pieceData; // 불변 변수를 가지는 구조체

        private int _tutorialSelectedCount = 0; // 튜토리얼에서 선택된 횟수 카운팅 변수

        // 생성자(불변 변수를 가지는 구조체)
        public PieceSelectHandler(PieceBase pieceBase, PieceData pieceData)
        {
            _pieceBase = pieceBase;
            _pieceData = pieceData;
        }

        public void PieceSelect()
        {
            HighLightEvents.OnPieceMovementHighLight?.Invoke(false, false); // 하이라이트 끄기, 이동 가능 배치 칸 대상
            HighLightEvents.OnRoadPlacementHighLight?.Invoke(false); // 도로 배치 칸 하이라이트 끄기
            HighLightEvents.OnPiecePlacementHighLight?.Invoke(false, true); // 기물 배치 칸 하이라이트 끄기, 배치 가능 배치 판 대상
            PieceEvents.OnHideCanAttackPieces?.Invoke(true); // 공격 가능한 기물들 하이라이트 끄기

            InGameContext.Current.Data.PlacePlaneManager.Variable.findCanPlacePlaneSystem.FindCanMovePlacePlane(_pieceBase.PieceVariable.currentPlacePlane, TeamManager.Instance.CurrentTeamType, _pieceData.currentObjectType); // 한 칸 이동 가능한 칸 찾기

            if(_pieceBase.CurrentObjectType == ObjectType.Tank) // 전차일 경우
            {
                if(InGameContext.Current.Data.CardManager.HaveFirePowerCard) // 화력 카드를 가지고 있을 때
                {
                    InGameContext.Current.Data.PlacePlaneManager.Variable.findCanPlacePlaneSystem.FindCanFirePowerAttackPiece(_pieceBase.CurrentTeamType, _pieceBase.PieceVariable.currentPlacePlane); // 한 칸 떨어진 기물들을 공격 가능 대상으로 지정
                }
            }

            InGameContext.Current.Data.GameManager.CurrentMovePiece = _pieceBase.gameObject; // 현재 객체를 현재 이동하려는 기물로 할당
            HighLightEvents.SelectedPlacementType = ObjectType.None; // 배치 하는 것이 아닌 이동의 여부이기에 None으로 설정

            foreach (var piece in InGameContext.Current.Data.PlacePlaneManager.Variable.highLightHandler.CanPieceMovePlanes) // 배치 가능한 도로 칸들 순회
            {
                piece.CanPlacePieceType = _pieceData.currentObjectType; // 배치 가능한 타입을 할당
            }

            HighLightEvents.OnPieceMovementHighLight?.Invoke(true, false); // 기물 이동 칸 하이라이트 키기, 이동 가능 배치 판 대상

            PieceEvents.OnShowCanAttackPieces?.Invoke(_pieceData.currentObjectType); // 보병이 공격 가능한 기물들 하이라이트 키기 (공격하는 기물)

            _pieceBase.PieceVariable.isSelected = true; // 선택 되었다고 할당

            if(GameModeManager.Instance.CurrentGameMode.IsTutorial()) // 튜토리얼 일 경우
            {
                switch(TutorialManager.Instance.CurrentTutorialState) // 현재 튜토리얼 상태가
                {
                    case TutorialState.Turn1_Player: // 첫 번째 턴(플레이어 턴)인 경우
                        switch(_pieceBase.CurrentObjectType) // 기물이
                        {
                            case ObjectType.Miner: // 광부 일 경우
                                TutorialManager.Instance.SetTutorialPanel(true, "이동 위치를 선택합시다.", "대상 클릭", 0.08f, 0.008f, new Vector4(0.401f, 0.452f), new Vector4(0.3f, 0.3f), new Vector2(0, 110f));
                                break;
                        }
                        break;
                    case TutorialState.Turn2_Player: // 두 번째 턴(플레이어 턴)인 경우
                        switch(_pieceBase.CurrentObjectType) // 기물이
                        {
                            case ObjectType.Miner: // 광부 일 경우
                                TutorialManager.Instance.SetTutorialPanel(true, "이동 위치를 선택합시다.", "대상 클릭", 0.08f, 0.008f, new Vector4(0.3325f, 0.516f), new Vector4(0.3f, 0.3f), new Vector2(0, 110f));
                                break;
                        }
                        break;
                    case TutorialState.Turn4_Player:
                        switch (_pieceBase.CurrentObjectType) // 기물이
                        {
                            case ObjectType.Miner: // 광부 일 경우
                                TutorialManager.Instance.SetTutorialPanel(true, "이동 위치를 선택합시다.", "대상 클릭", 0.08f, 0.008f, new Vector4(0.372f, 0.384f), new Vector4(0.3f, 0.3f), new Vector2(0, 110f));
                                break;
                            case ObjectType.Tank: // 전차 일 경우
                                if(_tutorialSelectedCount <= 0) // 처음 선택했다면
                                {
                                    TutorialManager.Instance.SetTutorialPanel(true, "이동 위치를 선택합시다.", "대상 클릭", 0.08f, 0.008f, new Vector4(0.401f, 0.452f), new Vector4(0.3f, 0.3f), new Vector2(0, 250f));
                                    _tutorialSelectedCount++;
                                }
                                else // 중복 선택 했다면
                                {
                                    TutorialManager.Instance.SetTutorialPanel(true, "원거리 공격 대상을 선택합시다.", "대상 클릭", 0.08f, 0.008f, new Vector4(0.381f, 0.517f), new Vector4(0.3f, 0.3f), new Vector2(0, 250f));
                                    _tutorialSelectedCount = 0;
                                }
                                break;
                        }
                        break;
                    case TutorialState.Turn5_Player:
                        switch (_pieceBase.CurrentObjectType) // 기물이
                        {
                            case ObjectType.Tank: // 전차 일 경우
                                TutorialManager.Instance.SetTutorialPanel(true, "원거리 공격 대상을 선택합시다.", "대상 클릭", 0.08f, 0.008f, new Vector4(0.381f, 0.517f), new Vector4(0.3f, 0.3f), new Vector2(0, 250f));
                                break;
                        }
                        break;
                    case TutorialState.Turn6_Player:
                        switch (_pieceBase.CurrentObjectType) // 기물이
                        {
                            case ObjectType.Miner: // 광부 일 경우
                                TutorialManager.Instance.SetTutorialPanel(true, "이동 위치를 선택합시다.", "대상 클릭", 0.08f, 0.008f, new Vector4(0.401f, 0.453f), new Vector4(0.3f, 0.3f), new Vector2(0, 250f));
                                break;
                            case ObjectType.Soldier: // 보병 일 경우
                                TutorialManager.Instance.SetTutorialPanel(true, "이동 위치를 선택합시다.", "대상 클릭", 0.08f, 0.008f, new Vector4(0.454f, 0.576f), new Vector4(0.3f, 0.3f), new Vector2(0, 250f));
                                break;
                            case ObjectType.Tank: // 전차 일 경우
                                if (_tutorialSelectedCount <= 0) // 처음 선택했다면
                                {
                                    TutorialManager.Instance.SetTutorialPanel(true, "이동 위치를 선택합시다.", "대상 클릭", 0.08f, 0.008f, new Vector4(0.543f, 0.684f), new Vector4(0.3f, 0.3f), new Vector2(0, 450f));
                                }
                                else // 중복 선택이라면
                                {
                                    TutorialManager.Instance.SetTutorialPanel(true, "공격 대상을 선택합시다.", "대상 클릭", 0.08f, 0.008f, new Vector4(0.586f, 0.684f), new Vector4(0.3f, 0.3f), new Vector2(0, 450f));
                                }
                                break;
                        }
                        break;
                    case TutorialState.Turn7_Player:
                        switch (_pieceBase.CurrentObjectType) // 기물이
                        {
                            case ObjectType.Soldier: // 보병 일 경우
                                TutorialManager.Instance.SetTutorialPanel(true, "이동 위치를 선택합시다.", "대상 클릭", 0.08f, 0.008f, new Vector4(0.543f, 0.684f), new Vector4(0.3f, 0.3f), new Vector2(0, 450f));
                                break;
                            case ObjectType.Tank: // 전차 일 경우
                                TutorialManager.Instance.SetTutorialPanel(true, "공격 대상을 선택합시다.", "대상 클릭", 0.08f, 0.008f, new Vector4(0.52f, 0.734f), new Vector4(0.3f, 0.3f));
                                break;
                        }
                        break;
                    case TutorialState.Turn8_Player:
                        switch (_pieceBase.CurrentObjectType) // 기물이
                        {
                            case ObjectType.Soldier: // 보병 일 경우
                                TutorialManager.Instance.SetTutorialPanel(true, "이동 위치를 선택합시다.", "대상 클릭", 0.08f, 0.008f, new Vector4(0.52f, 0.734f), new Vector4(0.3f, 0.3f));
                                break;
                            case ObjectType.Tank: // 전차 일 경우
                                TutorialManager.Instance.SetTutorialPanel(true, "이동 위치를 선택합시다.", "대상 클릭", 0.08f, 0.008f, new Vector4(0.52f, 0.734f), new Vector4(0.3f, 0.3f));
                                break;
                        }
                        break;
                }
            }
        }
    }
}
// 마지막 작성 일자: 2026.03.25