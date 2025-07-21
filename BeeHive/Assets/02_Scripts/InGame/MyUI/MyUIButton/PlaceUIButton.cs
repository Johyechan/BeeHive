using InGame.MyEvent;
using InGame.MyManager;
using InGame.MyObject.MyObjectEnum;
using InGame.MyUI.MyUIInterface;
using UnityEngine;

namespace InGame.MyUI.MyUIButton
{
    // 작성자: 조혜찬
    // 배치할 기물을 선택하는 버튼의 부모 클래스
    public abstract class PlaceUIButton : MonoBehaviour, IUIButton
    {
        [SerializeField] protected ObjectType _canPlaceType; // 배치 가능한 객체 타입 변수

        protected bool _isHighLightOn; // 하이라이트가 켜졌는지 확인하는 변수

        private void Awake()
        {
            _isHighLightOn = false; // 하이라이트 꺼짐 상태로 초기화
        }

        private void OnEnable()
        {
            HighLightEventSystem.OnPieceHighLight += HightLightOff; // 기물 전용 이벤트 구독
            HighLightEventSystem.OnRoadHighLight += HightLightOff; // 도로 전용 이벤트 구독
        }

        private void OnDisable()
        {
            HighLightEventSystem.OnPieceHighLight -= HightLightOff; // 기물 전용 이벤트 구독 해제
            HighLightEventSystem.OnRoadHighLight -= HightLightOff; // 도로 전용 이벤트 구독 해제
        }

        // 하이라이트가 꺼질 때 현재 하이라이트 활성화 여부를 끄는 함수
        private void HightLightOff(bool isOn)
        {
            if (!isOn) // 꺼져있는 상태라면
            {
                _isHighLightOn = isOn; // 현재 하이라이트 활성화 여부를 꺼져있는 상태로 할당
            }
        }

        public abstract void OnUIButtonClick();
    }
}
// 마지막 작성 일자: 2025.07.21