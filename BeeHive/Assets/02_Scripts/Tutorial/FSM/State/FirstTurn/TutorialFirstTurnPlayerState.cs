using InGame.MyEnum;
using InGame.MyManager.Local;
using MyUtil.Interface;
using UnityEngine;

namespace Tutorial.FSM.State.First
{
    // 작성자: 조혜찬
    // 첫 번째 턴(플레이어 턴) 상태 클래스
    public class TutorialFirstTurnPlayerState : IState
    {
        private int _count; // 카운팅 변수

        private TurnType _currentTurnType; // 현재 턴

        public void Enter()
        {
            
        }

        public void Exit()
        {
            
        }

        public void Update()
        {
            if(TutorialManager.Instance.IsInputDelayOver()) // 인풋이 딜레이 이후 들어오면
            {
                _count++; // 카운팅
            }

            switch(_currentTurnType) // 현재 턴이
            {
                case TurnType.MakeTurn: // 생산 턴일 때
                    switch (_count) // 카운팅 된 개수가
                    {
                        case 1:
                            TutorialManager.Instance.SetTutorialPanel(true, "금괴도 2개 생성됩니다.", 0.16f, 0.008f, new Vector4(0.2f, 0.29f), new Vector4(1.5f, 0.6f));
                            break;
                        case 2:
                            _ = InGameContext.Current.Data.TurnManager.TurnChange(TurnType.DrawTurn); // 드로우 턴으로 턴 넘기기
                            break;
                    }
                    break;
                case TurnType.MainTurn: // 메인 턴일 때
                    switch(_count)
                    {
                        case 1:
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
                        _ = InGameContext.Current.Data.TurnManager.TurnChange(TurnType.MakeTurn); // 생성 턴으로 턴 넘기기
                        break;
                    case TurnType.MakeTurn: // 생성 턴일 경우
                        TutorialManager.Instance.SetTutorialPanel(true, "매 턴 도로가 2개 생성되고,", 0.07f, 0.008f, new Vector4(0.271f, 0.533f), new Vector4(0.55f, 0.55f));
                        _count = 0;
                        _currentTurnType = TurnType.MakeTurn;
                        break;
                    case TurnType.DrawTurn: // 드로우 턴일 경우
                        TutorialManager.Instance.SetTutorialPanel(true, "다음 턴을 눌러 메인 턴을 진행합시다.", 0.18f, 0.008f, new Vector4(0.92f, 0.095f), new Vector4(0.66f, 0.4f));
                        break;
                    case TurnType.MainTurn: // 메인 턴일 경우 
                        break;
                }
            }
        }
    }
}
// 마지막 작성 일자: 2026.03.16