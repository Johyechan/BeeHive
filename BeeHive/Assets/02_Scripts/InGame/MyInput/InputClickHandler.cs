using InGame.MyManager.Global;
using InGame.MyManager.Local;
using InGame.MyObject.MyObjectInterface;
using MyUtil;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace InGame.MyInput
{
    // 작성자: 조혜찬
    // 클릭 기능을 제공하는 핸들러 클래스
    public class InputClickHandler
    {
        private InputManager _inputManager; // 인풋 매니저

        private RaycastHit _mouseRaycastHit; // 마우스 레이를 쏴서 닿았을 때 정보를 저장할 변수

        private bool _isOverUI; // UI위에 마우스 커서가 있는지 확인할 변수

        public InputClickHandler(InputManager inputManager)
        {
            _inputManager = inputManager;
        }

        // 마우스 클릭 시 호출되는 함수
        private GameObject OnMouseClick()
        {
            if (!_isOverUI) // 마우스 커서가 UI위에 있지 않다면
            {
                // 마우스 위치로 레이 캐스트 수행(hit, 레이 캐스트 거리, 레이어 마스크(ClickObj layer만 인식))
                GameObject clickedObj = RaycastUtil.MouseRaycast(out _mouseRaycastHit, 100, LayerMask.GetMask("ClickObj"));
                if (clickedObj != null) // 레이에 닿은 객체가 있다면
                {
                    return _mouseRaycastHit.collider.gameObject;
                }
            }

            return null;
        }

        public void MouseClick(InputAction.CallbackContext context)
        {
            if(_inputManager.IsIgnoreInput) // 인풋 무시 상태라면
            {
                return; // 반환
            }

            GameObject hitObj = OnMouseClick(); // 마우스 클릭 시 레이 캐스트를 쏘아 닿은 객체를 반환
            if (hitObj != null) // 레이캐스트에 닿은 객체가 있을 경우
            {
                IClickObject clickObj = hitObj.GetComponent<IClickObject>(); // 클릭 가능한 오브젝트들이 가지는 인터페이스 가져오기
                clickObj.ObjectClicked(); // 레이 캐스트에 닿은 객체에게 클릭되었다고 함수 호출
            }
        }

        public void CheckIsMouseOverUI()
        {
            _isOverUI = EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(); // 현재 EventSystem이 존재하고 마우스 커서가 UI위에 있다면 true 할당, 둘 중 하나라도 false라면 false 할당
        }
    }
}
// 마지막 작성 일자: 2026.06.24