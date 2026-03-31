using InGame.MyEnum;
using InGame.MyManager.Local;
using InGame.MyObject;
using MyUtil.Interface;
using NUnit.Framework;
using System.Collections.Generic;
using Tutorial.MyEnum;
using UnityEngine;

namespace Tutorial.FSM.State.Second
{
    // 작성자: 조혜찬
    // 두 번째 턴(플레이어 턴) 상태 클래스
    public class TutorialSecondTurnPlayerState : IState
    {
        private int _count = 0; // 카운팅 변수

        private TurnType _currentTurnType = TurnType.ChangeTeam; // 현재 턴 타입

        public void Enter()
        {
            TutorialManager.Instance.IsInputDelayOver = false;
        }

        public void Exit()
        {
            TutorialManager.Instance.InputOn = false;
            _ = InGameContext.Current.Data.TurnManager.NextTurn(TurnType.ChangeTeam); // 팀 변경 턴(다음 팀 턴 - 튜토리얼에선 두 번째 플레이어 턴)으로 변경
        }

        public void Update()
        {
            if(TutorialManager.Instance.IsInputDelayOver)
            {
                switch (_currentTurnType)
                {
                    case TurnType.MakeTurn:
                        switch (_count)
                        {
                            case 0:
                                TutorialManager.Instance.SetTutorialPanel(false);
                                _ = InGameContext.Current.Data.TurnManager.NextTurn(TurnType.DrawTurn);
                                _currentTurnType = TurnType.DrawTurn;
                                TutorialManager.Instance.InputOn = false;
                                break;
                        }
                        break;
                }
                _count++;
                TutorialManager.Instance.IsInputDelayOver = false;
            }

            if(TutorialManager.Instance.TurnEnd) // 현재 턴이 종료 되었을 때
            {
                TutorialManager.Instance.TurnEnd = false; // 턴 종료 여부 초기화

                switch(InGameContext.Current.Data.TurnManager.CurrentTurnType) // 현재 턴이
                {
                    case TurnType.ChangeTeam: // 팀 변경 턴일 때
                        _ = InGameContext.Current.Data.TurnManager.NextTurn(TurnType.MakeTurn); // 생산 턴으로 턴 넘기기
                        break;
                    case TurnType.MakeTurn: // 생산 턴일 때
                        TutorialManager.Instance.SetTutorialPanel(true, "이번 턴에 벌린 금화는 광부가 생산한 것입니다.", "엔터 클릭", 0.08f, 0.008f, new Vector4(0.17f, 0.37f), new Vector4(0.8f, 0.3f));
                        TutorialManager.Instance.InputOn = true;
                        _count = 0;
                        _currentTurnType = TurnType.MakeTurn;
                        break;
                    case TurnType.DrawTurn: // 드로우 턴일 때
                        TutorialManager.Instance.SetTutorialPanel(true, "다음 턴을 눌러 메인 턴을 진행합시다.", "버튼 클릭", 0.18f, 0.008f, new Vector4(0.92f, 0.095f), new Vector4(0.66f, 0.4f));
                        break;
                    case TurnType.MainTurn: // 메인 턴일 때
                        TutorialManager.Instance.SetTutorialPanel(true, "도로를 생성합시다.", "버튼 클릭", 0.1f, 0.008f, new Vector4(0.356f, 0.123f), new Vector4(0.5f, 0.3f));
                        break;
                    case TurnType.TurnEnd: // 턴 종료 턴일 때
                        TutorialManager.Instance.ChangeTutorialState(TutorialState.Turn2_AI); // 첫 번째 턴(AI 턴)으로 튜토리얼 상태 변경
                        break;
                }
            }
        }
    }
}
// 마지막 작성 일자: 2026.03.31