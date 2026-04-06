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
    // 도로 UI 버튼 클래스
    public class RoadButton : PlaceUIButton
    {
        private int _tutorialCreateCount = 0; // 튜토리얼 전용 도로 생성 카운팅 변수

        // 클릭 시 실행될 함수
        public override void OnUIClick()
        {
            bool isWarning = false;

            try
            {
                string str = LocalizationSettings.StringDatabase.GetLocalizedString(
                    "Game",
                    "Game_UI_NotMainTurnCanNotCreateRoad"
                );
                isWarning = WarningEvent.OnCheckCurrentTurn.Invoke(TurnType.MainTurn, str);
            }
            catch(Exception ex)
            {
                if (GameModeManager.Instance.CurrentGameMode.UseServer())
                    NetworkManager.Instance.Socket.Emit("debug", $"경고 이벤트에서 예외 발생: {ex} - RoadButton.cs:24");
            }

            // 현재 턴이 메인 턴이 아니라면
            if (!isWarning)
            {
                EventSystem.current.SetSelectedGameObject(null); // 선택한 객체 초기화
                return; // 반환
            }

            if (!UIManager.Instance.CanInteractionUI) // 만약 UI 상호작용 불가능 상태라면
            {
                EventSystem.current.SetSelectedGameObject(null); // 선택한 객체 초기화
                return; // 반환
            }

            if (!_isHighLightOn) // 하이라이트가 꺼져있을 때
            {
                HighLightEvents.SelectedPlacementType = _canPlaceType; // 현재 배치 가능한 타입을 현재 타입으로 할당
                HighLightEvents.OnPieceMovementHighLight?.Invoke(false, false); // 기물 이동 칸 하이라이트 끄기, 이동 가능한 배치 칸 대상
                HighLightEvents.OnPiecePlacementHighLight?.Invoke(false, true); // 기물 배치 칸 하이라이트 끄기(하이라이트 키기 여부, 배치 칸 이동 칸 여부 - true는 배치칸, false는 이동칸)
                PieceEvents.OnHideCanAttackPieces?.Invoke(true); // 공격 가능한 기물들 하이라이트 끄기

                foreach (var road in InGameContext.Current.Data.PlacePlaneManager.Variable.highLightHandler.CanRoadPlacePlanes) // 배치 가능한 도로 칸들 순회
                {
                    try
                    {
                        road.CanPlacePieceType = _canPlaceType; // 배치 가능한 타입을 할당
                        road.Cost = _cost; // 비용 할당
                        road.LeftPieceCount = _objectParent.childCount; // 남은 기물 수 할당
                    }
                    catch(Exception ex)
                    {
                        if (GameModeManager.Instance.CurrentGameMode.UseServer())
                            NetworkManager.Instance.Socket.Emit("debug", $"예외 발생: {ex}");
                    }
                }

                HighLightEvents.OnRoadPlacementHighLight?.Invoke(true); // 도로 배치 칸 하이라이트 키기

                if(GameModeManager.Instance.CurrentGameMode.IsTutorial()) // 튜토리얼 일 경우
                {
                    switch(TutorialManager.Instance.CurrentTutorialState) // 현재 튜토리얼 상태가
                    {
                        case TutorialState.Turn1_Player: // 첫 번째 턴(플레이어 턴) 일 경우
                            if(_tutorialCreateCount == 0) // 처음 도로를 생성하는 경우
                            {
                                string firstCreateRoad = LocalizationSettings.StringDatabase.GetLocalizedString(
                                    "Tutorial",
                                    "Tutorial_CreateRoad"
                                );
                                TutorialManager.Instance.SetTutorialPanel(true, firstCreateRoad, TutorialManager.Instance.TargetClick, 0.08f, 0.008f, new Vector4(0.4621f, 0.416f), new Vector4(0.3f, 0.3f), new Vector2(0, 250f));
                                _tutorialCreateCount++;
                            }
                            else // 두 번째 도로를 생성하는 경우
                            {
                                string secondCreateRoad = LocalizationSettings.StringDatabase.GetLocalizedString(
                                    "Tutorial",
                                    "Tutorial_secondCreateRoad"
                                );
                                TutorialManager.Instance.SetTutorialPanel(true, secondCreateRoad, TutorialManager.Instance.TargetClick, 0.08f, 0.008f, new Vector4(0.426f, 0.452f), new Vector4(0.3f, 0.3f), new Vector2(0, 250f));
                                _tutorialCreateCount = 0;
                            }
                            break;
                        case TutorialState.Turn2_Player:
                            if (_tutorialCreateCount == 0) // 처음 도로를 생성하는 경우
                            {
                                TutorialManager.Instance.SetTutorialPanel(true, "도로를 생성합시다.", "대상 클릭", 0.08f, 0.008f, new Vector4(0.39f, 0.485f), new Vector4(0.3f, 0.3f), new Vector2(0, 250f));
                                _tutorialCreateCount++;
                            }
                            else // 두 번째 도로를 생성하는 경우
                            {
                                TutorialManager.Instance.SetTutorialPanel(true, "한 번 더 도로를 생성합시다.", "대상 클릭", 0.08f, 0.008f, new Vector4(0.356f, 0.516f), new Vector4(0.3f, 0.3f), new Vector2(0, 250f));
                                _tutorialCreateCount = 0;
                            }
                            break;
                        case TutorialState.Turn3_Player:
                            if (_tutorialCreateCount == 0) // 처음 도로를 생성하는 경우
                            {
                                TutorialManager.Instance.SetTutorialPanel(true, "도로를 생성합시다.", "대상 클릭", 0.08f, 0.008f, new Vector4(0.394f, 0.546f), new Vector4(0.3f, 0.3f), new Vector2(0, 250f));
                                _tutorialCreateCount++;
                            }
                            else // 두 번째 도로를 생성하는 경우
                            {
                                TutorialManager.Instance.SetTutorialPanel(true, "한 번 더 도로를 생성합시다.", "대상 클릭", 0.08f, 0.008f, new Vector4(0.464f, 0.485f), new Vector4(0.3f, 0.3f), new Vector2(0, 250f));
                                _tutorialCreateCount = 0;
                            }
                            break;
                        case TutorialState.Turn4_Player:
                            TutorialManager.Instance.SetTutorialPanel(true, "도로를 생성합시다.", "대상 클릭", 0.08f, 0.008f, new Vector4(0.465f, 0.546f), new Vector4(0.3f, 0.3f), new Vector2(0, 250f));
                            break;
                        case TutorialState.Turn6_Player:
                            if (_tutorialCreateCount == 0) // 처음 도로를 생성하는 경우
                            {
                                TutorialManager.Instance.SetTutorialPanel(true, "도로를 생성합시다.", "대상 클릭", 0.08f, 0.008f, new Vector4(0.5f, 0.632f), new Vector4(0.3f, 0.3f), new Vector2(0, 450f));
                                _tutorialCreateCount++;
                            }
                            else // 두 번째 도로를 생성하는 경우
                            {
                                TutorialManager.Instance.SetTutorialPanel(true, "한 번 더 도로를 생성합시다.", "대상 클릭", 0.08f, 0.008f, new Vector4(0.533f, 0.659f), new Vector4(0.3f, 0.3f), new Vector2(0, 450f));
                                _tutorialCreateCount = 0;
                            }
                            break;
                    }
                }

                _isHighLightOn = true; // 하이라이트가 켜져있는 상태라고 할당
            }
            else // 하이라이트가 켜져있을 때
            {
                HighLightEvents.SelectedPlacementType = ObjectType.None; // 아무것도 배치 할 수 없는 타입으로 초기화
                HighLightEvents.OnRoadPlacementHighLight?.Invoke(false); // 배치 가능한 도로 칸 하이라이트 끄기
            }

            EventSystem.current.SetSelectedGameObject(null); // 선택한 객체 초기화
        }
    }
}
// 마지막 작성 일자: 2026.04.06