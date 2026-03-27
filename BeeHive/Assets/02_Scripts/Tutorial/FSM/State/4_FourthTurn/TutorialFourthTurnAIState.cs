using InGame.MyEnum;
using InGame.MyEvent;
using InGame.MyManager.Local;
using InGame.MyObject;
using InGame.MyObject.Piece;
using InGame.MyObject.Piece.ObjectPieces;
using InGame.MyUI;
using MyUtil.Interface;
using System.Threading.Tasks;
using Tutorial.MyEnum;
using UnityEngine;

namespace Tutorial.FSM.State.Fourth
{
    // 작성자: 조혜찬
    // 네 번째 턴(AI 턴) 상태
    public class TutorialFourthTurnAIState : IState
    {
        private PieceBase _tank; // 생성 및 이동할 전차

        private PiecePlacePlaneObject _createPlacePlane; // 생성 칸 객체
        private PiecePlacePlaneObject _movePlacePlane; // 이동 칸 객체

        private ConfirmUI _confirmUI; // 방어 여부를 물을 UI

        public TutorialFourthTurnAIState(PieceBase tank, PiecePlacePlaneObject createPlacePlane, PiecePlacePlaneObject movePlacePlane, ConfirmUI confirmUI)
        {
            _tank = tank;
            _createPlacePlane = createPlacePlane;
            _movePlacePlane = movePlacePlane;
            _confirmUI = confirmUI;
        }

        public void Enter()
        {
            
        }

        public void Exit()
        {
            _ = InGameContext.Current.Data.TurnManager.NextTurn(TurnType.ChangeTeam); // 팀 변경 턴(다음 팀 턴 - 튜토리얼에선 두 번째 플레이어 턴)으로 변경
        }

        public  async void Update()
        {
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

                        WalletEvent.OnUseGoldBar(5); // 금괴 사용(전차 비용)
                        // 전차 생성
                        await TutorialManager.Instance.ObjectPlace(_createPlacePlane, _tank, false);

                        InGameContext.Current.Data.GameManager.CurrentMovePiece = _tank.gameObject; // 현재 선택된 객체를 할당
                        // 전차 이동   
                        await TutorialManager.Instance.ObjectPlace(_movePlacePlane, _tank, true);

                        // 방어 여부 묻기
                        _confirmUI.gameObject.SetActive(true);
                        TutorialManager.Instance.SetTutorialPanel(true, "상대 전차의 공격을 방어합시다", "버튼 클릭", 0.08f, 0.008f, new Vector4(0.422f, 0.224f), new Vector4(1.2f, 0.3f), new Vector2(0, 450f));
                        TaskCompletionSource<bool> confirmResultTcs = new TaskCompletionSource<bool>(); // 확인 결과를 가지는 tcs
                        _confirmUI.Confirm(result =>
                        {
                            TutorialManager.Instance.SetTutorialPanel(false);
                            _confirmUI.ConfirmEnd(); // 확인 완료
                            confirmResultTcs.TrySetResult(result); // 확인 결과(result) 할당
                        }, "화력을 사용하여 방어하시겠습니까?");

                        await confirmResultTcs.Task;

                        _ = InGameContext.Current.Data.TurnManager.NextTurn(TurnType.TurnEnd); // 턴 종료 턴으로 턴 변경

                        break;
                    case TurnType.TurnEnd: // 턴 종료 턴이라면
                        TutorialManager.Instance.ChangeTutorialState(TutorialState.Turn5_Player); // 다섯 번째 턴(플레이어 턴)으로 튜토리얼 상태 변경
                        break;
                }
            }
        }
    }
}
// 마지막 작성 일자: 2026.03.27