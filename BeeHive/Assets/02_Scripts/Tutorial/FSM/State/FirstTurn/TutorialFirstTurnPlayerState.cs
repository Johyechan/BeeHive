using InGame.MyEnum;
using InGame.MyManager.Global;
using InGame.MyManager.Local;
using MyUtil.Interface;
using UnityEngine;

namespace Tutorial.FSM.State.First
{
    // 작성자: 조혜찬
    // 첫 번째 턴(플레이어 턴) 상태 클래스
    public class TutorialFirstTurnPlayerState : IState
    {
        public void Enter()
        {
            
        }

        public void Exit()
        {
            
        }

        public void Update()
        {
            switch(InGameContext.Current.Data.TurnManager.CurrentTurnType) // 현재 턴이
            {
                case TurnType.MakeTurn: // 생성 턴일 경우
                    break;
                case TurnType.DrawTurn: // 드로우 턴일 경우
                    break;
                case TurnType.MainTurn: // 메인 턴일 경우
                    break;
            }
        }
    }
}
// 마지막 작성 일자: 2026.03.13