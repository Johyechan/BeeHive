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
        private PieceBase _soldier; // 생성 및 이동할 보병

        private PiecePlacePlaneObject _movePlacePlane; // 이동 칸 객체

        private Transform _roadParent; // 도로 부모 객체

        private RoadPlacePlaneObject _firstRoadPlacePlane; // 첫 번째 도로 배치 칸
        private RoadPlacePlaneObject _secondRoadPlacePlane; // 두 번째 도로 배치 칸

        public TutorialSecondTurnAIState(PieceBase soldier, PiecePlacePlaneObject movePlacePlane, Transform roadParent, RoadPlacePlaneObject firstRoadPlacePlane, RoadPlacePlaneObject secondRoadPlacePlane)
        {
            _soldier = soldier;
            _movePlacePlane = movePlacePlane;
            _roadParent = roadParent;
            _firstRoadPlacePlane = firstRoadPlacePlane;
            _secondRoadPlacePlane = secondRoadPlacePlane;
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

                        // 첫 번째 도로 생성
                        Road road = _roadParent.GetChild(_roadParent.childCount - 1).GetComponent<Road>(); // 도로 부모에서 도로 가져오기
                        await TutorialManager.Instance.ObjectPlace(_firstRoadPlacePlane, road, false);

                        // 두 번째 도로 생성
                        road = _roadParent.GetChild(_roadParent.childCount - 1).GetComponent<Road>(); // 도로 부모에서 도로 가져오기
                        await TutorialManager.Instance.ObjectPlace(_secondRoadPlacePlane, road, false);

                        InGameContext.Current.Data.GameManager.CurrentMovePiece = _soldier.gameObject; // 현재 선택된 객체를 할당

                        // 보병 이동
                        await TutorialManager.Instance.ObjectPlace(_movePlacePlane, _soldier, true);
                        PieceEvents.OnChangeNearRoad?.Invoke(_soldier, _soldier.CurrentTeamType, _soldier.PieceVariable.currentPlacePlane); // 도로 변경 이벤트 호출

                        _ = InGameContext.Current.Data.TurnManager.NextTurn(TurnType.TurnEnd); // 턴 종료 턴으로 턴 변경

                        break;
                    case TurnType.TurnEnd: // 턴 종료 턴이라면
                        TutorialManager.Instance.ChangeTutorialState(TutorialState.Turn3_Player); // 두 번째 턴(플레이어 턴)으로 튜토리얼 상태 변경
                        break;
                }
            }
        }
    }
}
// 마지막 작성 일자: 2026.03.24