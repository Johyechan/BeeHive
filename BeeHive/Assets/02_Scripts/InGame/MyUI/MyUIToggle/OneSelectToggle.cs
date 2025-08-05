using InGame.MyUI.MyUIInterface;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace InGame.MyUI.MyUIToggle
{
    // 작성자: 조혜찬
    // 하나만 선택되는 Toggle 클래스
    public class OneSelectToggle : MonoBehaviour, IUIClick, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Toggle _otherToggle; // 다른 토글

        private Toggle _currentToggle; // 현재 토글

        private bool _isMouseOn; // 마우스가 올려져 있는지 확인해서 무한으로 서로 왔다갔다 하는 상황을 막기

        private void Awake()
        {
            _currentToggle = GetComponent<Toggle>(); // 현재 객체의 토글 가져오기
            _isMouseOn = false;
        }

        // 클릭되었을 때 실행될 함수
        public void OnUIClick()
        {
            if(_isMouseOn)
            {
                _currentToggle.isOn = true; // 현재 토글(내 토글) 키기
                _otherToggle.isOn = false; // 다른 토글 끄기
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _isMouseOn = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isMouseOn = false;
        }
    }
}
// 마지막 작성 일자: 2025.08.05