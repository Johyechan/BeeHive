using InGame.MyEvent;
using InGame.MyManager;
using InGame.MyManager.MyPlacePlane;
using InGame.MyObject.MyObjectEnum;
using InGame.MyUI.MyUIInterface;
using UnityEngine;

namespace InGame.MyUI.MyUIButton
{
    // 작성자: 조혜찬
    // 도로 UI 버튼 클래스
    public class RoadButton : PlaceUIButton
    {
        // 클릭 시 실행될 함수
        public override void OnUIButtonClick()
        {
            if(!UIManager.Instance.CanInteractionUI) // 만약 UI 상호작용 불가능 상태라면
                return; // 반환 - UI 클릭 무시

            if (!_isHighLightOn) // 하이라이트가 꺼져있을 때
            {
                HighLightEventSystem.CurrentCanPlaceType = _canPlaceType; // 현재 배치 가능한 타입을 현재 타입으로 할당
                HighLightEventSystem.OnPieceHighLightObjAction?.Invoke(false, false); // 하이라이트 끄기, 이동 가능한 배치 칸 대상
                foreach (var road in PlacePlaneManager.Instance.HighLightHandlerProp.CanRoadPlacePlanesProp) // 배치 가능한 도로 칸들 순회
                {
                    road.CanPlacePieceTypeProp = _canPlaceType; // 배치 가능한 타입을 할당
                }

                HighLightEventSystem.OnPieceHighLightUIAction?.Invoke(false, true); // 배치 가능한 기물 칸 하이라이트 끄기(하이라이트 키기 여부, 배치 칸 이동 칸 여부 - true는 배치칸, false는 이동칸)
                HighLightEventSystem.OnRoadHighLightUIAction?.Invoke(true); // 배치 가능한 도로 칸 하이라이트 키기
                _isHighLightOn = true; // 하이라이트가 켜져있는 상태라고 할당
            }
            else // 하이라이트가 켜져있을 때
            {
                HighLightEventSystem.CurrentCanPlaceType = ObjectType.None; // 아무것도 배치 할 수 없는 타입으로 초기화
                HighLightEventSystem.OnRoadHighLightUIAction?.Invoke(false); // 배치 가능한 도로 칸 하이라이트 끄기
            }
        }
    }
}
// 마지막 작성 일자: 2025.07.22