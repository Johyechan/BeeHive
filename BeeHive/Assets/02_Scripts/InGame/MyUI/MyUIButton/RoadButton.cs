using InGame.MyManager.MyPlacePlane;
using InGame.MyObject.MyObjectEnum;
using InGame.MyUI.MyUIInterface;
using UnityEngine;

namespace InGame.MyUI.MyUIButton
{
    // 작성자: 조혜찬
    // 도로 UI 버튼 클래스
    public class RoadButton : MonoBehaviour, IUIButton
    {
        [SerializeField] private ObjectType _objectType; // 배치 가능한 객체 타입 변수

        private bool _isHighLightOn; // 하이라이트가 켜졌는지 확인하는 변수

        private void Awake()
        {
            _isHighLightOn = false; // 하이라이트 꺼짐 상태로 초기화
        }

        private void OnEnable()
        {
            HighLightEventSystem.OnRoadHighLight += HightLightOnOff; // 이벤트 구독
        }

        private void OnDisable()
        {
            HighLightEventSystem.OnRoadHighLight -= HightLightOnOff; // 이벤트 구독 해제
        }

        // 하이라이트가 켜질 때 하이라이트가 켜진 상태 할당, 꺼졌을 때는 꺼진 상태 할당
        private void HightLightOnOff(bool isOn)
        {
            _isHighLightOn = isOn;
        }

        public void OnUIButtonClick()
        {
            if(!_isHighLightOn) // 하이라이트가 꺼져있을 때
            {
                foreach (var road in PlacePlaneManager.Instance.HighLightHandlerProp.CanRoadPlacePlanesProp) // 배치 가능한 도로 칸들 순회
                {
                    Debug.Log($"버튼 눌렀을 때 배치 가능한 객체는 {_objectType}");
                    road.CanPlacePieceTypeProp = _objectType; // 배치 가능한 타입을 할당
                    Debug.Log($"도로 판이 판단했을 때 배치 가능한 객체는? {road.CanPlacePieceTypeProp}");
                }

                HighLightEventSystem.OnRoadHighLight?.Invoke(true); // 배치 가능한 도로칸 하이라이트 키기
            }
            else // 하이라이트가 켜져있을 때
            {
                HighLightEventSystem.OnRoadHighLight?.Invoke(false); // 배치 가능한 도로칸 하이라이트 끄기
            }
        }
    }
}
// 마지막 작성 일자: 2025.07.18