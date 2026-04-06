using InGame.MyEnum;
using InGame.MyEvent;
using InGame.MyManager.Global;
using InGame.MyManager.Local;
using MyUtil.GameMode;
using System;
using Tutorial;
using Tutorial.MyEnum;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Settings;

namespace InGame.MyUI.MyUIButton
{
    // 작성자: 조혜찬
    // 기물 UI 버튼 클래스
    public class PieceButton : PlaceUIButton
    {
        // 클릭 시 실행될 함수
        public override void OnUIClick()
        {
            string str = LocalizationSettings.StringDatabase.GetLocalizedString(
                "Game",
                "Game_UI_NotMainTurnCanNotCreatePieces"
            );

            // 현재 턴이 메인 턴이 아니라면
            if (!WarningEvent.OnCheckCurrentTurn.Invoke(TurnType.MainTurn, str))
            {
                EventSystem.current.SetSelectedGameObject(null); // 선택한 객체 초기화
                return; // 반환
            }

            if (!WarningEvent.OnCanMakePiece.Invoke()) // 생성이 불가능하다면
            {
                EventSystem.current.SetSelectedGameObject(null); // 선택한 객체 초기화
                return;
            }

            if (!UIManager.Instance.CanInteractionUI) // 만약 UI 상호작용 불가능 상태라면
            {
                EventSystem.current.SetSelectedGameObject(null); // 선택한 객체 초기화
                return; // 반환
            }

            if (!_isHighLightOn) // 하이라이트가 꺼져 있을 때
            {
                HighLightEvents.OnPieceMovementHighLight?.Invoke(false, false); // 기물 이동 칸 하이라이트 끄기, 이동 가능한 배치 칸 대상
                PieceEvents.OnHideCanAttackPieces?.Invoke(true); // 공격 가능한 기물들 하이라이트 끄기

                foreach (var piece in InGameContext.Current.Data.PlacePlaneManager.Variable.highLightHandler.CanPiecePlacePlanes) // 배치 가능한 기물 칸들 순회
                {
                    try
                    {
                        piece.CanPlacePieceType = _canPlaceType; // 배치 가능한 타입을 할당
                        piece.Cost = _cost; // 비용 할당
                        piece.LeftPieceCount = _objectParent.childCount; // 남은 기물 수 할당
                    }
                    catch(Exception ex)
                    {
                        if (GameModeManager.Instance.CurrentGameMode.UseServer())
                            NetworkManager.Instance.Socket.Emit("debug", $"예외 발생: {ex}");
                    }
                }

                if(GameModeManager.Instance.CurrentGameMode.IsTutorial()) // 현재 게임 모드가 튜토리얼일 경우
                {
                    switch(TutorialManager.Instance.CurrentTutorialState)
                    {
                        case TutorialState.Turn1_Player:
                            string selectMinerCreatePlace = LocalizationSettings.StringDatabase.GetLocalizedString(
                                "Tutorial",
                                "Tutorial_SelectCreateMinerPlace"
                            );
                            TutorialManager.Instance.SetTutorialPanel(true, selectMinerCreatePlace, TutorialManager.Instance.TargetClick, 0.08f, 0.008f, new Vector4(0.475f, 0.383f), new Vector4(0.3f, 0.3f), new Vector2(0, 110f));
                            break;
                        case TutorialState.Turn2_Player:
                            TutorialManager.Instance.SetTutorialPanel(true, "광부 생성 위치를 선택합시다.", "대상 클릭", 0.08f, 0.008f, new Vector4(0.475f, 0.383f), new Vector4(0.3f, 0.3f), new Vector2(0, 110f));
                            break;
                        case TutorialState.Turn4_Player:
                            TutorialManager.Instance.SetTutorialPanel(true, "전차 생성 위치를 선택합시다.", "대상 클릭", 0.08f, 0.008f, new Vector4(0.475f, 0.383f), new Vector4(0.3f, 0.3f), new Vector2(0, 110f));
                            break;
                        case TutorialState.Turn6_Player:
                            TutorialManager.Instance.SetTutorialPanel(true, "보병 생성 위치를 선택합시다.", "대상 클릭", 0.08f, 0.008f, new Vector4(0.475f, 0.383f), new Vector4(0.3f, 0.3f), new Vector2(0, 110f));
                            break;
                        case TutorialState.Turn8_Player:
                            TutorialManager.Instance.SetTutorialPanel(true, "전차 생성 위치를 선택합시다.", "대상 클릭", 0.08f, 0.008f, new Vector4(0.475f, 0.383f), new Vector4(0.3f, 0.3f), new Vector2(0, 110f));
                            break;
                    }
                }

                if (HighLightEvents.SelectedPlacementType != _canPlaceType) // 만약 현재 배치 가능한 타입이 다르다면
                {
                    HighLightEvents.OnRoadPlacementHighLight?.Invoke(false); // 도로 배치 칸 하이라이트 끄기
                    HighLightEvents.OnPiecePlacementHighLight?.Invoke(true, true); // 기물 배치 칸 하이라이트 키기(하이라이트 키기 여부, 배치 칸 이동 칸 여부 - true는 배치칸, false는 이동칸)
                    _isHighLightOn = true; // 현재 하이라이트가 켜져있다고 할당
                    HighLightEvents.SelectedPlacementType = _canPlaceType; // 현재 배치 가능한 타입을 변경
                }

            }
            else // 하이라이트가 켜져있을 때
            {
                if (HighLightEvents.SelectedPlacementType == _canPlaceType) // 현재 배치 가능한 타입이 같다면
                {
                    HighLightEvents.OnPiecePlacementHighLight?.Invoke(false, true); // 배치 가능한 기물 칸 하이라이트 키기(하이라이트 키기 여부, 배치 칸 이동 칸 여부 - true는 배치칸, false는 이동칸)
                    _isHighLightOn = false; // 현재 하이라이트가 꺼졌다고 할당
                    HighLightEvents.SelectedPlacementType = ObjectType.None; // 아무것도 배치할 수 없는 타입으로 초기화
                }
            }

            EventSystem.current.SetSelectedGameObject(null); // 선택한 객체 초기화
        }
    }
}
// 마지막 작성 일자: 2026.04.06