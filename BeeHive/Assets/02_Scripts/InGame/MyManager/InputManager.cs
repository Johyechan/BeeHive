using InGame.MyEvent;
using InGame.MyInput;
using InGame.MyInput.Struct;
using InGame.MyObject;
using MyUtil;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InGame.MyManager
{
    // 작성자: 조혜찬
    // 인풋을 관리하는 싱글톤 매니저
    public class InputManager : MonoSingleton<InputManager>
    {
        [SerializeField] private Deck _deck; // 드로우에 필요한 객체를 가지는 클래스
        [SerializeField] private InputActionAsset _playerActionAsset; // 플레이어의 인풋 에셋

        [SerializeField] private int _drawMillisecondDelay; // 드로우 딜레이 시간

        private InputActionMap _playActionMap; // 인풋 에셋의 액션 맵 - 게임에 필요한 액션들을 가지는 맵
        private InputAction _lClickAction; // 액션 맵에 있는 액션 - 좌클릭 액션
        private InputAction _drawAction; // 액션 맵에 있는 액션 - 드로우 액션
        private InputAction _rClickAction; // 액션 맵에 있는 액션 - 우클릭 액션

        private InputClickHandler _clickHandler; // 객체 클릭을 인식하기 위한 핸들러
        private InputDrawHandler _drawHandler; // 드로우를 위한 핸들러

        private InputDrawHandlerData _inputDrawHandlerData; // 드로우 핸들러에 필요한 핸들러들을 가지는 구조체

        protected override void Awake()
        {
            base.Awake();

            _inputDrawHandlerData = new InputDrawHandlerData()
            {
                returnHandler = new InputDrawReturnHandler(_deck),
                socketEventHandler = new InputDrawSocketEventHandler(),
                functionHandler = new InputDrawFunctionHandler(),
            };

            _clickHandler = new InputClickHandler();
            _drawHandler = new InputDrawHandler(_deck, _drawMillisecondDelay, _inputDrawHandlerData);

            _playActionMap = _playerActionAsset.FindActionMap("Play"); // 인풋 에셋에서 Play 이름을 가진 액션 맵 탐색
            _lClickAction = _playActionMap.FindAction("LClick"); // 액션 맵에서 LClick 이름을 가진 액션 탐색
            _drawAction = _playActionMap.FindAction("Draw"); // 액션 맵에서 Draw 이름을 가진 액션 탐색
            _rClickAction = _playActionMap.FindAction("RClick"); // 액션 맵에서 RClick 이름을 가진 액션 탐색

            _playerActionAsset.Enable(); // 인풋 에셋 활성화
            _lClickAction.Enable();
            _drawAction.Enable();
            _rClickAction.Enable();
            _lClickAction.performed += _clickHandler.MouseClick; // 클릭 액션에 클릭 시 실행될 함수 구독
            _drawAction.performed += _drawHandler.Draw; // 드로우 액션에 드로우 인풋 시 실행될 함수 구독
            _rClickAction.performed += ctx => UIEvents.OnShowUICardInformation?.Invoke();
        }

        private void OnDisable()
        {
            _lClickAction.performed -= _clickHandler.MouseClick; // 클릭 액션에 구독된 함수 해제
            _drawAction.performed -= _drawHandler.Draw; // 드로우 액션에 구독된 함수 해제
            _playerActionAsset.Disable(); // 인풋 에셋 비활성화
        }

        void Update()
        {
            _clickHandler.CheckIsMouseOverUI(); // 마우스 커서가 UI 위에 있는지 확인하는 함수
        }
    }
}
// 마지막 작성 일자: 2025.11.13