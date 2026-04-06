using InGame.MyEnum;
using InGame.MyManager.Local;
using InGame.MyObject;
using MyUtil.Interface;
using System.Collections.Generic;
using Tutorial.MyEnum;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Tutorial.FSM.State.First
{
    // 작성자: 조혜찬
    // 첫 번째 턴(플레이어 턴) 상태 클래스
    public class TutorialFirstTurnPlayerState : IState
    {
        private int _count; // 카운팅 변수

        private TurnType _currentTurnType; // 현재 턴

        private List<PiecePlacePlaneObject> _goldCoin1PlacePlanes; // 금화 1개를 버는 위치 리스트
        private List<PiecePlacePlaneObject> _goldCoin3PlacePlanes; // 금화 3개를 버는 위치 리스트
        private List<PiecePlacePlaneObject> _goldCoin5PlacePlanes; // 금화 5개를 버는 위치 리스트

        public TutorialFirstTurnPlayerState(List<PiecePlacePlaneObject> goldCoin1PlacePlanes, List<PiecePlacePlaneObject> goldCoin3PlacePlanes, List<PiecePlacePlaneObject> goldCoin5PlacePlanes)
        {
            _goldCoin1PlacePlanes = goldCoin1PlacePlanes;
            _goldCoin3PlacePlanes = goldCoin3PlacePlanes;
            _goldCoin5PlacePlanes = goldCoin5PlacePlanes;
        }

        public void Enter()
        {
            TutorialManager.Instance.IsInputDelayOver = false;
        }

        public void Exit()
        {
            TutorialManager.Instance.InputOn = false;
            _ = InGameContext.Current.Data.TurnManager.NextTurn(TurnType.ChangeTeam); // 팀 변경 턴(다음 팀 턴 - 튜토리얼에선 첫 번째 AI 턴)으로 변경
        }

        public void Update()
        {
            if(TutorialManager.Instance.IsInputDelayOver) // 인풋이 딜레이 이후 들어오면
            {
                _count++; // 카운팅
                TutorialManager.Instance.IsInputDelayOver = false;
            }

            switch(_currentTurnType) // 현재 턴이
            {
                case TurnType.MakeTurn: // 생산 턴일 때
                    switch (_count) // 카운팅 된 개수가
                    {
                        case 1:
                            string goldBarCreateTwo = LocalizationSettings.StringDatabase.GetLocalizedString(
                                "Tutorial",
                                "Tutorial_GoldBarCreateTwo"
                            );
                            TutorialManager.Instance.SetTutorialPanel(true, goldBarCreateTwo, TutorialManager.Instance.EnterClick, 0.08f, 0.008f, new Vector4(0.155f, 0.257f), new Vector4(1f, 0.6f));
                            break;
                        case 2:
                            TutorialManager.Instance.SetTutorialPanel(false);
                            _ = InGameContext.Current.Data.TurnManager.NextTurn(TurnType.DrawTurn); // 드로우 턴으로 턴 넘기기
                            _currentTurnType = TurnType.DrawTurn; // 현재 턴 변경(이걸 안하면 계속 생산 턴으로 인식하는 문제 발생)
                            TutorialManager.Instance.InputOn = false;
                            break;
                    }
                    break;
                case TurnType.MainTurn:
                    switch (_count) // 카운팅 된 개수가
                    {
                        case 1:
                            string minerGuide1 = LocalizationSettings.StringDatabase.GetLocalizedString(
                                "Tutorial",
                                "Tutorial_MinerGuide1"
                            );
                            TutorialManager.Instance.SetTutorialPanel(true, minerGuide1, TutorialManager.Instance.EnterClick);
                            break;
                        case 2:
                            HightLightOnOff(_goldCoin1PlacePlanes, true);
                            string minerGuide2 = LocalizationSettings.StringDatabase.GetLocalizedString(
                                "Tutorial",
                                "Tutorial_MinerGuide2"
                            );
                            TutorialManager.Instance.SetTutorialPanel(true, minerGuide2, TutorialManager.Instance.EnterClick, 0, 0, default, default, new Vector2(0, 100f));
                            break;
                        case 3:
                            HightLightOnOff(_goldCoin1PlacePlanes, false);
                            HightLightOnOff(_goldCoin3PlacePlanes, true);
                            string minerGuide3 = LocalizationSettings.StringDatabase.GetLocalizedString(
                                "Tutorial",
                                "Tutorial_MinerGuide3"
                            );
                            TutorialManager.Instance.SetTutorialPanel(true, minerGuide3, TutorialManager.Instance.EnterClick, 0, 0, default, default, new Vector2(0, 350f));
                            break;
                        case 4:
                            HightLightOnOff(_goldCoin3PlacePlanes, false);
                            HightLightOnOff(_goldCoin5PlacePlanes, true);
                            string minerGuide4 = LocalizationSettings.StringDatabase.GetLocalizedString(
                                "Tutorial",
                                "Tutorial_MinerGuide4"
                            );
                            TutorialManager.Instance.SetTutorialPanel(true, minerGuide4, TutorialManager.Instance.EnterClick, 0, 0, default, default, new Vector2(0, 300f));
                            break;
                        case 5:
                            HightLightOnOff(_goldCoin5PlacePlanes, false);
                            string minerGuide5 = LocalizationSettings.StringDatabase.GetLocalizedString(
                                "Tutorial",
                                "Tutorial_MinerGuide5"
                            );
                            TutorialManager.Instance.SetTutorialPanel(true, minerGuide5, TutorialManager.Instance.EnterClick);
                            break;
                        case 6:
                            string clickPieceShowButton = LocalizationSettings.StringDatabase.GetLocalizedString(
                                "Tutorial",
                                "Tutorial_ClickPieceButton"
                            );
                            TutorialManager.Instance.SetTutorialPanel(true, clickPieceShowButton, TutorialManager.Instance.ButtonClick, 0.1f, 0.008f, new Vector4(0.196f, 0.095f), new Vector4(0.7f, 0.7f));
                            _currentTurnType = TurnType.TurnEnd; // 현재 턴 변경(이걸 안하면 계속 생산 턴으로 인식하는 문제 발생)
                            TutorialManager.Instance.InputOn = false;
                            break;
                    }
                    break;
            }

            if(TutorialManager.Instance.TurnEnd) // 턴이 종료되었을 때
            {
                TutorialManager.Instance.TurnEnd = false; // 턴 종료 상태를 초기화

                switch (InGameContext.Current.Data.TurnManager.CurrentTurnType) // 현재 턴이
                {
                    case TurnType.ChangeTeam: // 팀 변경 턴일 경우
                        _ = InGameContext.Current.Data.TurnManager.NextTurn(TurnType.MakeTurn); // 생성 턴으로 턴 넘기기
                        break;
                    case TurnType.MakeTurn: // 생성 턴일 경우
                        TutorialManager.Instance.InputOn = true;
                        string roadCreateTwo = LocalizationSettings.StringDatabase.GetLocalizedString(
                            "Tutorial",
                            "Tutorial_RoadCreateTwo"
                        );
                        TutorialManager.Instance.SetTutorialPanel(true, roadCreateTwo, TutorialManager.Instance.EnterClick, 0.07f, 0.008f, new Vector4(0.271f, 0.533f), new Vector4(0.55f, 0.55f));
                        SetCountAndTurn(TurnType.MakeTurn);
                        break;
                    case TurnType.DrawTurn: // 드로우 턴일 경우
                        string goToMainTurn = LocalizationSettings.StringDatabase.GetLocalizedString(
                            "Tutorial",
                            "Tutorial_GoToMainTurn"
                        );
                        TutorialManager.Instance.SetTutorialPanel(true, goToMainTurn, TutorialManager.Instance.ButtonClick, 0.18f, 0.008f, new Vector4(0.92f, 0.095f), new Vector4(0.66f, 0.4f));
                        break;
                    case TurnType.MainTurn: // 메인 턴일 경우 
                        TutorialManager.Instance.IsInputDelayOver = false;
                        SetCountAndTurn(TurnType.MainTurn);
                        string firstCreateMiner = LocalizationSettings.StringDatabase.GetLocalizedString(
                            "Tutorial",
                            "Tutorial_FirstCreateMiner"
                        );
                        TutorialManager.Instance.SetTutorialPanel(true, firstCreateMiner, TutorialManager.Instance.EnterClick);
                        TutorialManager.Instance.InputOn = true;
                        break;
                    case TurnType.TurnEnd: // 턴 종료 턴일 경우
                        TutorialManager.Instance.ChangeTutorialState(TutorialState.Turn1_AI); // 첫 번째 턴(AI 턴)으로 튜토리얼 상태 변경
                        break;
                }
            }
        }

        private void HightLightOnOff(List<PiecePlacePlaneObject> goldCoinPlacePlanes, bool on)
        {
            foreach (var placePlane in goldCoinPlacePlanes)
            {
                if (on)
                {
                    placePlane.HighLightOn();
                }
                else
                {
                    placePlane.HighLightOff();
                }
            }
        }

        private void SetCountAndTurn(TurnType turnType)
        {
            _count = 0;
            _currentTurnType = turnType;
        }
    }
}
// 마지막 작성 일자: 2026.04.06