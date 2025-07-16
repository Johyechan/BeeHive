using InGame.MyObject;
using MyUtil;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace InGame.MyManager
{
    // 작성자: 조혜찬
    // 인풋 관련 매니저
    public class InputManager : MonoSingleton<InputManager>
    {
        [SerializeField] private InputActionAsset _playerActionAsset; // 플레이어 인풋 액션 에셋

        private InputActionMap _playActionMap; // Play 명을 가진 플레이 할 때 필요한 액션들을 가진 맵 변수
        private InputAction _clickAction; // click 명을 가진 클릭 액션 변수

        private InputClickHandler _clickHandler;

        protected override void Awake()
        {
            base.Awake();

            _clickHandler = new InputClickHandler(); // 인풋 클릭 클래스 가져오기

            _playActionMap = _playerActionAsset.FindActionMap("Play"); // 인풋 액션 에셋에서 Play 명을 가진 맵 탐색
            _clickAction = _playActionMap.FindAction("Click"); // 인풋 액션 에셋 맵에서 Click 명을 가진 액션 탐색
        }

        private void OnEnable()
        {
            _playerActionAsset.Enable(); // 플레이어 인풋 액션 에셋 활성화 - 맵과 액션까지 전부 활성화
            _clickAction.performed += _clickHandler.OnMouseClick; // 클릭 액션에 클릭 시 실행될 함수 구독
        }

        private void OnDisable()
        {
            _clickAction.performed -= _clickHandler.OnMouseClick; // 클릭 액션에 구독되어 있는 함수 구독 해제
            _playerActionAsset.Disable(); // 플레이어 인풋 액션 에셋 비활성화 - 맵과 액션까지 전부 비활성화
        }

        void Update()
        {
            _clickHandler.IsOverUI = EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(); // 현재 이벤트 시스템이 존재하면서 마우스 커서가 UI 위에 있다면 true 하나라도 충족하지 못한다면 false
        }
    }
}
// 마지막 작성 일자: 2025.07.16