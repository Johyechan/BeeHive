using DG.Tweening;
using InGame.MyEnum;
using InGame.MyEvent;
using InGame.MyManager.Local;
using InGame.MyObject;
using InGame.MyObject.Piece;
using InGame.MyObject.Piece.ObjectPieces;
using InGame.MyUI;
using MyUtil.Interface;
using Tutorial.MyEnum;
using UnityEngine;

namespace Tutorial.FSM.State.Second
{
    // 작성자: 조혜찬
    // 두 번째 턴(AI 턴) 상태 클래스
    public class TutorialSecondTurnAIState : IState
    {
        private PieceBase _attackTank; // 생성 및 이동할 보병

        private PiecePlacePlaneObject _createPlacePlane; // 생성 칸 객체
        private PiecePlacePlaneObject _movePlacePlane; // 이동 칸 객체

        private ConfirmUI _confirmUI; // 확인 UI

        private bool _confirmEnd = false; // 확인 종료

        public TutorialSecondTurnAIState(PieceBase tank, PiecePlacePlaneObject createPlacePlane, PiecePlacePlaneObject movePlacePlane, ConfirmUI confirmUI)
        {
            _attackTank = tank;
            _createPlacePlane = createPlacePlane;
            _movePlacePlane = movePlacePlane;
            _confirmUI = confirmUI;
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
            if(_confirmEnd) // 확인이 끝났을 때
            {
                if (TutorialManager.Instance.IsInputDelayOver) // 인풋 딜레이가 지났나서 인풋이 들어왔다면
                {
                    _ = InGameContext.Current.Data.TurnManager.NextTurn(TurnType.TurnEnd); // 턴 종료 턴으로 턴 변경
                    TutorialManager.Instance.IsInputDelayOver = false;
                }
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
                        // 전차 생성 칸 상태 변경
                        InGameContext.Current.Data.PlacePlaneManager.ChangePlacePlaneState(_createPlacePlane, _attackTank, false);

                        // 전차 생성 위치로 이동
                        await _attackTank.MoveToPlacePlane(_createPlacePlane.transform.parent, _createPlacePlane.transform.localPosition, false);

                        InGameContext.Current.Data.GameManager.CurrentMovePiece = _attackTank.gameObject;

                        // 전차 이동 칸 상태 변경
                        InGameContext.Current.Data.PlacePlaneManager.ChangePlacePlaneState(_movePlacePlane, _attackTank, true);

                        // 전차 이동 위치로 이동
                        await _attackTank.MoveToPlacePlane(_movePlacePlane.transform.parent, _movePlacePlane.transform.localPosition, true);

                        _confirmUI.gameObject.SetActive(true);
                        _confirmUI.Confirm(result =>
                        {
                            TutorialManager.Instance.IsInputDelayOver = false;
                            TutorialManager.Instance.InputOn = true;
                            TutorialManager.Instance.SetTutorialPanel(true, "전차간의 싸움에서는 화력을 소모하여 방어할 수 있습니다.", "엔터 클릭");
                            _confirmEnd = true;
                        }, "화력을 사용하여 방어하시겠습니까?");

                        TutorialManager.Instance.SetTutorialPanel(true, "상대 전차의 공격을 방어합시다.", "버튼 클릭", 0.1f, 0.008f, new Vector4(0.422f, 0.223f), new Vector4(1f, 0.3f), new Vector2(0, 400f));

                        break;
                    case TurnType.TurnEnd: // 턴 종료 턴이라면
                        TutorialManager.Instance.ChangeTutorialState(TutorialState.Turn3_Player); // 두 번째 턴(플레이어 턴)으로 튜토리얼 상태 변경
                        break;
                }
            }
        }
    }
}
// 마지막 작성 일자: 2026.03.20