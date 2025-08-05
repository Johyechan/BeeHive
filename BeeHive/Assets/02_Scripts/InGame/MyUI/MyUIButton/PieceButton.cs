using InGame.MyEnum;
using InGame.MyEvent;
using InGame.MyManager;
using InGame.MyManager.MyPlacePlane;
using InGame.MyUI.MyUIInterface;
using UnityEngine;

namespace InGame.MyUI.MyUIButton
{
    // 작성자: 조혜찬
    // 기물 UI 버튼 클래스
    public class PieceButton : PlaceUIButton
    {
        // 클릭 시 실행될 함수
        public override void OnUIClick()
        {
            // 만약 UI 상호작용 불가능 상태라면 또는 배치하려는 객체가 없을 경우(이 경우 특수하게 효과 줄 예정)
            if (!UIManager.Instance.CanInteractionUI || _objectParent.childCount <= 0) 
                return; // 반환 - UI 클릭 무시

            if (!_isHighLightOn) // 하이라이트가 꺼져 있을 때
            {
                HighLightEvents.OnPieceMovementHighLight?.Invoke(false, false); // 기물 이동 칸 하이라이트 끄기, 이동 가능한 배치 칸 대상
                foreach (var piece in PlacePlaneManager.Instance.HighLightHandler.CanPiecePlacePlanes) // 배치 가능한 기물 칸들 순회
                {
                    piece.CanPlacePieceType = _canPlaceType; // 배치 가능한 타입을 할당
                }

                if(HighLightEvents.SelectedPlacementType != _canPlaceType) // 만약 현재 배치 가능한 타입이 다르다면
                {
                    HighLightEvents.OnRoadPlacementHighLight?.Invoke(false); // 도로 배치 칸 하이라이트 끄기
                    HighLightEvents.OnPiecePlacementHighLight?.Invoke(true, true); // 기물 배치 칸 하이라이트 키기(하이라이트 키기 여부, 배치 칸 이동 칸 여부 - true는 배치칸, false는 이동칸)
                    _isHighLightOn = true; // 현재 하이라이트가 켜져있다고 할당
                    HighLightEvents.SelectedPlacementType = _canPlaceType; // 현재 배치 가능한 타입을 변경
                }
            }
            else // 하이라이트가 켜져있을 때
            {
                if(HighLightEvents.SelectedPlacementType == _canPlaceType) // 현재 배치 가능한 타입이 같다면
                {
                    HighLightEvents.OnPiecePlacementHighLight?.Invoke(false, true); // 배치 가능한 기물 칸 하이라이트 키기(하이라이트 키기 여부, 배치 칸 이동 칸 여부 - true는 배치칸, false는 이동칸)
                    _isHighLightOn = false; // 현재 하이라이트가 꺼졌다고 할당
                    HighLightEvents.SelectedPlacementType = ObjectType.None; // 아무것도 배치할 수 없는 타입으로 초기화
                }
            }
        }
    }
}
// 마지막 작성 일자: 2025.07.23