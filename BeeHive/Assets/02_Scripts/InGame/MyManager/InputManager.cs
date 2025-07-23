using InGame.MyInput;
using InGame.MyObject;
using InGame.MyObject.MyObjectInterface;
using MyUtil;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace InGame.MyManager
{
    // 작성자: 조혜찬
    // 인풋을 관리하는 싱글톤 매니저
    public class InputManager : MonoSingleton<InputManager>
    {
        [SerializeField] private InputActionAsset _playerActionAsset; // 플레이어의 인풋 에셋

        private InputActionMap _playActionMap; // 인풋 에셋의 액션 맵 - 게임에 필요한 액션들을 가지는 맵
        private InputAction _clickAction; // 액션 맵에 있는 액션 - 클릭 액션

        private InputClickHandler _clickHandler; // 객체 클릭을 인식하기 위한 핸들러

        protected override void Awake()
        {
            base.Awake();

            _clickHandler = new InputClickHandler();

            _playActionMap = _playerActionAsset.FindActionMap("Play"); // 인풋 에셋에서 Play 이름을 가진 액션 맵 탐색
            _clickAction = _playActionMap.FindAction("Click"); // 액션 맵에서 Click이름을 가진 액션 탐색
        }

        private void OnEnable()
        {
            _playerActionAsset.Enable(); // 인풋 에셋 활성화
            _clickAction.performed += MouseClick; // 클릭 액션에 클릭 시 실행될 함수 구독
        }

        private void OnDisable()
        {
            _clickAction.performed -= MouseClick; // 클릭 액션에 구독된 함수 해제
            _playerActionAsset.Disable(); // 인풋 에셋 비활성화
        }

        private void MouseClick(InputAction.CallbackContext context)
        {
            GameObject hitObj = _clickHandler.OnMouseClick(); // 마우스 클릭 시 레이 캐스트를 쏘아 닿은 객체를 반환
            if (hitObj != null) // 레이캐스트에 닿은 객체가 있을 경우
            {
                IClickObject clickObj = hitObj.GetComponent<IClickObject>(); // 클릭 가능한 오브젝트들이 가지는 인터페이스 가져오기
                clickObj.ObjectClicked(); // 레이 캐스트에 닿은 객체에게 클릭되었다고 함수 호출
            }
            
        }

        void Update()
        {
            _clickHandler.IsOverUI = EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(); // 현재 EventSystem이 존재하고 마우스 커서가 UI위에 있다면 true 할당, 둘 중 하나라도 false라면 false 할당
        }
    }
}
// 마지막 작성 일자: 2025.07.23