using InGame.MyEnum;
using InGame.MyEvent;
using InGame.MyManager.Local;
using InGame.MyObject;
using InGame.MyObject.Piece;
using InGame.MyObject.Piece.ObjectPieces;
using MyUtil.Interface;
using Tutorial.MyEnum;
using UnityEngine;

namespace Tutorial.FSM.State.First
{
    // 작성자: 조혜찬
    // 첫 번째 턴(AI 턴) 상태 클래스
    public class TutorialFirstTurnAIState : IState
    {
        private PieceBase _soldier; // 생성 및 이동할 보병

        private PiecePlacePlaneObject _createPlacePlane; // 생성 칸 객체
        private PiecePlacePlaneObject _movePlacePlane; // 이동 칸 객체

        public TutorialFirstTurnAIState(PieceBase soldier, PiecePlacePlaneObject createPlacePlane, PiecePlacePlaneObject movePlacePlane)
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
            if(TutorialManager.Instance.TurnEnd) // 현재 턴이 끝났을 때
            {
                TutorialManager.Instance.TurnEnd = false; // 초기화
                switch(InGameContext.Current.Data.TurnManager.CurrentTurnType) // 현재 턴이
                {
                    case TurnType.ChangeTeam: // 팀 변경 턴이라면
                        _ = InGameContext.Current.Data.TurnManager.NextTurn(TurnType.MakeTurn); // 생성 턴으로 턴 변경
                        break;
                    case TurnType.MakeTurn: // 생성 턴이라면
                        _ = InGameContext.Current.Data.TurnManager.NextTurn(TurnType.DrawTurn); // 드로우 턴으로 턴 변경
                        break;
                    case TurnType.DrawTurn: // 드로우 턴이라면
                        _ = InGameContext.Current.Data.TurnManager.NextTurn(TurnType.MainTurn); // 메인 턴으로 턴 변경
                        break;
                    case TurnType.MainTurn: // 메인 턴이라면
                        // 보병 생성 칸 상태 변경
                        InGameContext.Current.Data.PlacePlaneManager.ChangePlacePlaneState(_createPlacePlane, _soldier, false); 

                        // 보병 생성 위치로 이동
                        await _soldier.MoveToPlacePlane(_createPlacePlane.transform.parent, _createPlacePlane.transform.localPosition, false);

                        InGameContext.Current.Data.GameManager.CurrentMovePiece = _soldier.gameObject;

                        // 보병 이동 칸 상태 변경
                        InGameContext.Current.Data.PlacePlaneManager.ChangePlacePlaneState(_movePlacePlane, _soldier, true);

                        // 보병 이동 위치로 이동
                        await _soldier.MoveToPlacePlane(_movePlacePlane.transform.parent, _movePlacePlane.transform.localPosition, true);

                        PieceEvents.OnChangeNearRoad?.Invoke(_soldier, _soldier.CurrentTeamType, _soldier.PieceVariable.currentPlacePlane); // 도로 변경 이벤트 호출

                        _ = InGameContext.Current.Data.TurnManager.NextTurn(TurnType.TurnEnd); // 턴 종료 턴으로 턴 변경
                        break;
                    case TurnType.TurnEnd: // 턴 종료 턴이라면
                        TutorialManager.Instance.ChangeTutorialState(TutorialState.Turn2_Player); // 두 번째 턴(플레이어 턴)으로 튜토리얼 상태 변경
                        break;
                }
            }
        }
    }
}
// 마지막 작성 일자: 2026.03.18