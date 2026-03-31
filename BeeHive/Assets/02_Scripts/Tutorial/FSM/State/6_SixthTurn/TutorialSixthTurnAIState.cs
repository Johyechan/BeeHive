using InGame.MyEnum;
using InGame.MyEvent;
using InGame.MyManager.Local;
using InGame.MyObject;
using InGame.MyObject.Piece;
using InGame.MyObject.Piece.ObjectPieces;
using MyUtil.Interface;
using System.Threading.Tasks;
using Tutorial.MyEnum;
using UnityEngine;

namespace Tutorial.FSM.State.Sixth
{
    // 작성자: 조혜찬
    // 여섯 번째 턴(AI 턴) 상태
    public class TutorialSixthTurnAIState : IState
    {
        private PieceBase _soldier; // 생성 및 이동할 보병

        private PiecePlacePlaneObject _createPlacePlane; // 생성 칸 객체
        private PiecePlacePlaneObject _movePlacePlane; // 이동 칸 객체

        private int _count = 0; // 카운팅 변수

        private TurnType _currentTurnType = TurnType.ChangeTeam; // 현재 턴 타입

        private TaskCompletionSource<bool> _guidetutorialEnd; // 안내 튜토리얼 종료 대기

        public TutorialSixthTurnAIState(PieceBase soldier, PiecePlacePlaneObject createPlacePlane, PiecePlacePlaneObject movePlacePlane)
        {
            _soldier = soldier;
            _createPlacePlane = createPlacePlane;
            _movePlacePlane = movePlacePlane;
        }

        public void Enter()
        {
            TutorialManager.Instance.IsInputDelayOver = false;
        }

        public void Exit()
        {
            TutorialManager.Instance.InputOn = false;
            _ = InGameContext.Current.Data.TurnManager.NextTurn(TurnType.ChangeTeam); // 팀 변경 턴(다음 팀 턴 - 튜토리얼에선 두 번째 플레이어 턴)으로 변경
        }

        public async void Update()
        {
            if (TutorialManager.Instance.IsInputDelayOver)
            {
                switch (_currentTurnType)
                {
                    case TurnType.MainTurn:
                        switch(_count)
                        {
                            case 0:
                                TutorialManager.Instance.SetTutorialPanel(false);
                                _currentTurnType = TurnType.TurnEnd;
                                _guidetutorialEnd.SetResult(true);
                                TutorialManager.Instance.InputOn = false;
                                break;
                        }
                        break;
                }
                _count++;
                TutorialManager.Instance.IsInputDelayOver = false;
            }

            if (TutorialManager.Instance.TurnEnd) // 현재 턴이 끝났을 때
            {
                TutorialManager.Instance.TurnEnd = false; // 초기화
                switch (InGameContext.Current.Data.TurnManager.CurrentTurnType) // 현재 턴이
                {
                    case TurnType.ChangeTeam: // 팀 변경 턴이라면
                        _ = InGameContext.Current.Data.TurnManager.NextTurn(TurnType.MakeTurn); // 생성 턴으로 턴 변경
                        break;
                    case TurnType.MakeTurn: // 생성 턴이라면
                        await TurnEvents.OnMakeTurn.ActionlistPlay(); // 생산 턴의 작업 실행

                        _ = InGameContext.Current.Data.TurnManager.NextTurn(TurnType.DrawTurn); // 드로우 턴으로 턴 변경
                        break;
                    case TurnType.DrawTurn: // 드로우 턴이라면
                        _ = InGameContext.Current.Data.TurnManager.NextTurn(TurnType.MainTurn); // 메인 턴으로 턴 변경
                        break;
                    case TurnType.MainTurn: // 메인 턴이라면
                        WalletEvent.OnUseGoldBar(3); // 금괴 사용(보병 비용)
                        // 보병 생성
                        await TutorialManager.Instance.ObjectPlace(_createPlacePlane, _soldier, false);
                        InGameContext.Current.Data.GameManager.CurrentMovePiece = _soldier.gameObject; // 현재 선택된 객체를 할당

                        _guidetutorialEnd = new TaskCompletionSource<bool>();
                        TutorialManager.Instance.InputOn = true;
                        _count = 0;
                        _currentTurnType = TurnType.MainTurn;
                        TutorialManager.Instance.SetTutorialPanel(true, "생성 위치 앞 지점에 상대 기물이 올라가 있을 경우 해당 위치의 생성은 막히게 됩니다.", "엔터 클릭", 0.08f, 0.008f, new Vector4(0.53f, 0.71f), new Vector4(1f, 1f));

                        await _guidetutorialEnd.Task; // 안내 튜토리얼 종료 대기

                        // 보병 이동
                        await TutorialManager.Instance.ObjectPlace(_movePlacePlane, _soldier, true);
                        PieceEvents.OnChangeNearRoad?.Invoke(_soldier, _soldier.CurrentTeamType, _soldier.PieceVariable.currentPlacePlane); // 도로 변경 이벤트 호출

                        _ = InGameContext.Current.Data.TurnManager.NextTurn(TurnType.TurnEnd); // 턴 종료 턴으로 턴 변경
                        break;
                    case TurnType.TurnEnd: // 턴 종료 턴이라면
                        TutorialManager.Instance.ChangeTutorialState(TutorialState.Turn7_Player); // 일곱 번째 턴(플레이어 턴)으로 튜토리얼 상태 변경
                        break;
                }
            }
        }
    }
}
// 마지막 작성 일자: 2026.03.31


