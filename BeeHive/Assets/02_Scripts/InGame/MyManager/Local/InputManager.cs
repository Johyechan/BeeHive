using InGame.MyEvent;
using InGame.MyInput;
using MyUtil.GameMode;
using System.Collections;
using Tutorial;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
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
        private InputAction _escapeClickAction; // 액션 맵에 있는 액션 - esc 클릭 액션

        private InputClickHandler _clickHandler; // 객체 클릭을 인식하기 위한 핸들러

        private bool _isIgnoreInput = false; // 인풋 무시 변수
        public bool IsIgnoreInput { get => _isIgnoreInput; } // 인풋 무시 변수 프로퍼티
        
        private async void Awake()
        {
            await GameReady.Gate.WaitAsync();

            _clickHandler = new InputClickHandler(this);
            
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

                _escapeClickAction = _playActionMap.FindAction("EscapeClick"); // 액션 맵에서 EscapeClick 이름을 가진 액션 탐색
                _escapeClickAction.Enable();
                _escapeClickAction.performed += TutorialManager.Instance.OnEscape;
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

        private void OnApplicationFocus(bool focus)
        {
            if(focus) // 게임 화면으로 돌아왔을 때
            {
                StartCoroutine(OneframeInputIgnoreCo());
                EventSystem.current?.SetSelectedGameObject(null); // 현재 선택 상태 초기화
                Canvas.ForceUpdateCanvases(); // UI 재계산 - 레이아웃 동기화로 UI위치와 Raycast 위치가 어긋나는 문제 대비
            }
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
                _escapeClickAction.performed -= TutorialManager.Instance.OnEscape;
            }
            _playerActionAsset.Disable(); // 인풋 에셋 비활성화
        }

        // 1 프레임 인풋 무시 코루틴
        private IEnumerator OneframeInputIgnoreCo()
        {
            _isIgnoreInput = true;
            yield return null;
            _isIgnoreInput = false;
        }
    }
}
// 마지막 작성 일자: 2026.06.24