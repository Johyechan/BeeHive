using InGame.MyEvent;
using InGame.MyInput;
using MyUtil.GameMode;
using Tutorial;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InGame.MyManager.Local
{
    // 작성자: 조혜찬
    // 인풋을 관리하는 매니저
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private InputActionAsset _playerActionAsset; // 플레이어의 인풋 에셋

        private InputActionMap _playActionMap; // 인풋 에셋의 액션 맵 - 게임에 필요한 액션들을 가지는 맵
        private InputAction _lClickAction; // 액션 맵에 있는 액션 - 좌클릭 액션
        private InputAction _rClickAction; // 액션 맵에 있는 액션 - 우클릭 액션
        private InputAction _enterClickAction; // 액션 맵에 있는 액션 - 엔터 클릭 액션

        private InputClickHandler _clickHandler; // 객체 클릭을 인식하기 위한 핸들러
        
        private async void Awake()
        {
            await GameReady.Gate.WaitAsync();

            _clickHandler = new InputClickHandler();
            
            _playActionMap = _playerActionAsset.FindActionMap("Play"); // 인풋 에셋에서 Play 이름을 가진 액션 맵 탐색
            _lClickAction = _playActionMap.FindAction("LClick"); // 액션 맵에서 LClick 이름을 가진 액션 탐색
            _rClickAction = _playActionMap.FindAction("RClick"); // 액션 맵에서 RClick 이름을 가진 액션 탐색

            _playerActionAsset.Enable(); // 인풋 에셋 활성화
            _lClickAction.Enable();
            _rClickAction.Enable();
            _lClickAction.performed += _clickHandler.MouseClick; // 클릭 액션에 클릭 시 실행될 함수 구독
            _rClickAction.performed += ctx => UIEvents.OnShowUICardInformation?.Invoke();
            if(GameModeManager.Instance.CurrentGameMode.IsTutorial()) // 튜토리얼 일 경우
            {
                _enterClickAction = _playActionMap.FindAction("EnterClick"); // 액션 맵에서 EnterClick 이름을 가진 액션 탐색
                _enterClickAction.Enable();
                _enterClickAction.performed += TutorialManager.Instance.OnConfirm;
            }
        }

        private void OnEnable()
        {
            GameOverEvent.OnGameOver += DisableInputSystem;
        }

        private void OnDisable()
        {
            GameOverEvent.OnGameOver -= DisableInputSystem;
            DisableInputSystem();
        }

        void Update()
        {
            _clickHandler.CheckIsMouseOverUI(); // 마우스 커서가 UI 위에 있는지 확인하는 함수
        }

        public void DisableInputSystem()
        {
            _lClickAction.performed -= _clickHandler.MouseClick; // 클릭 액션에 구독된 함수 해제
            if(GameModeManager.Instance.CurrentGameMode.IsTutorial())
            {
                _enterClickAction.performed -= TutorialManager.Instance.OnConfirm;
            }
            _playerActionAsset.Disable(); // 인풋 에셋 비활성화
        }
    }
}
// 마지막 작성 일자: 2026.03.19