using InGame.MyEnum;
using InGame.MyEvent;
using InGame.MyManager.Local;
using InGame.MyManager.Local.Turn;
using InGame.MyObject;
using InGame.MyObject.Piece;
using MyUtil.Interface;
using Tutorial.MyEnum;
using UnityEngine;

namespace Tutorial.FSM.State.Third
{
    // 작성자: 조혜찬
    // 세 번째 턴(AI 턴) 상태 클래스
    public class TutorialThirdTurnAIState : IState
    {
        private PieceBase _soldier; // 생성 및 이동할 보병

        private PiecePlacePlaneObject _createPlacePlane; // 생성 칸 객체
        private PiecePlacePlaneObject _movePlacePlane; // 이동 칸 객체

        public TutorialThirdTurnAIState(PieceBase soldier, PiecePlacePlaneObject createPlacePlane, PiecePlacePlaneObject movePlacePlane)
        {
            _soldier = soldier;
            _createPlacePlane = createPlacePlane;
            _movePlacePlane = movePlacePlane;
        }

        public void Enter()
        {
            
        }

        public void Exit()
        {
            _ = InGameContext.Current.Data.TurnManager.NextTurn(TurnType.ChangeTeam); // 팀 변경 턴(다음 팀 턴 - 튜토리얼에선 두 번째 플레이어 턴)으로 변경
        }

        public async void Update()
        {
            if(TutorialManager.Instance.TurnEnd) // 현재 턴이 종료되었다면
            {
                TutorialManager.Instance.TurnEnd = false; // 초기화

                switch(InGameContext.Current.Data.TurnManager.CurrentTurnType)
                {
                    case TurnType.ChangeTeam:
                        _ = InGameContext.Current.Data.TurnManager.NextTurn(TurnType.MakeTurn);
                        break;
                    case TurnType.MakeTurn:
                        await TurnEvents.OnMakeTurn.ActionlistPlay(); // 생산 턴의 작업 실행

                        _ = InGameContext.Current.Data.TurnManager.NextTurn(TurnType.DrawTurn);
                        break;
                    case TurnType.DrawTurn:
                        _ = InGameContext.Current.Data.TurnManager.NextTurn(TurnType.MainTurn);
                        break;
                    case TurnType.MainTurn:
                        // 보병 생성 칸 상태 변경
                        InGameContext.Current.Data.PlacePlaneManager.ChangePlacePlaneState(_createPlacePlane, _soldier, false);

                        // 보병 생성 위치로 이동
                        await _soldier.MoveToPlacePlane(_createPlacePlane.transform.parent, _createPlacePlane.transform.localPosition, false);

                        InGameContext.Current.Data.GameManager.CurrentMovePiece = _soldier.gameObject; // 현재 선택된 객체를 할당

                        // 보병 이동 칸 상태 변경
                        InGameContext.Current.Data.PlacePlaneManager.ChangePlacePlaneState(_movePlacePlane, _soldier, true);

                        // 보병 이동 위치로 이동
                        await _soldier.MoveToPlacePlane(_movePlacePlane.transform.parent, _movePlacePlane.transform.localPosition, true);

                        PieceEvents.OnChangeNearRoad?.Invoke(_soldier, _soldier.CurrentTeamType, _soldier.PieceVariable.currentPlacePlane); // 도로 변경 이벤트 호출

                        _ = InGameContext.Current.Data.TurnManager.NextTurn(TurnType.TurnEnd); // 턴 종료 턴으로 턴 변경
                        break;
                    case TurnType.TurnEnd:
                        TutorialManager.Instance.ChangeTutorialState(TutorialState.Turn4_Player); // 네 번째 턴(플레이어 턴)으로 튜토리얼 상태 변경
                        break;
                }
            }
        }
    }
}
// 마지막 작성 일자: 2026.03.19