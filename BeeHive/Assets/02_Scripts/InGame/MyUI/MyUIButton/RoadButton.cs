using InGame.MyEnum;
using InGame.MyEvent;
using InGame.MyManager;
using InGame.MyManager.MyPlacePlane;

namespace InGame.MyUI.MyUIButton
{
    // 작성자: 조혜찬
    // 도로 UI 버튼 클래스
    public class RoadButton : PlaceUIButton
    {
        // 클릭 시 실행될 함수
        public override async void OnUIClick()
        {
            // 현재 턴이 메인 턴이 아니라면
            if (!await WarningEvent.OnCheckCurrentTurn.Invoke(TurnType.MainTurn, "메인 턴이 아니라서 도로를 생성할 수 없습니다."))
                return; // 반환

            if (!UIManager.Instance.CanInteractionUI) // 만약 UI 상호작용 불가능 상태라면
                return; // 반환

            if (!_isHighLightOn) // 하이라이트가 꺼져있을 때
            {
                HighLightEvents.SelectedPlacementType = _canPlaceType; // 현재 배치 가능한 타입을 현재 타입으로 할당
                HighLightEvents.OnPieceMovementHighLight?.Invoke(false, false); // 기물 이동 칸 하이라이트 끄기, 이동 가능한 배치 칸 대상
                HighLightEvents.OnPiecePlacementHighLight?.Invoke(false, true); // 기물 배치 칸 하이라이트 끄기(하이라이트 키기 여부, 배치 칸 이동 칸 여부 - true는 배치칸, false는 이동칸)
                await PieceEvents.OnHideCanAttackPieces?.Invoke(); // 공격 가능한 기물들 하이라이트 끄기

                foreach (var road in PlacePlaneManager.Instance.Variable.highLightHandler.CanRoadPlacePlanes) // 배치 가능한 도로 칸들 순회
                {
                    road.CanPlacePieceType = _canPlaceType; // 배치 가능한 타입을 할당
                    road.Cost = _cost; // 비용 할당
                    road.LeftPieceCount = _objectParent.childCount; // 남은 기물 수 할당
                }

                HighLightEvents.OnRoadPlacementHighLight?.Invoke(true); // 도로 배치 칸 하이라이트 키기
                _isHighLightOn = true; // 하이라이트가 켜져있는 상태라고 할당
            }
            else // 하이라이트가 켜져있을 때
            {
                HighLightEvents.SelectedPlacementType = ObjectType.None; // 아무것도 배치 할 수 없는 타입으로 초기화
                HighLightEvents.OnRoadPlacementHighLight?.Invoke(false); // 배치 가능한 도로 칸 하이라이트 끄기
            }
        }
    }
}
// 마지막 작성 일자: 2025.09.23