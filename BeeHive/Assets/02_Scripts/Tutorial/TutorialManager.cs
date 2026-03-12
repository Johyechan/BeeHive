using MyUtil.GameMode;
using MyUtil.Interface;
using Tutorial.Event;
using Tutorial.FSM;
using Tutorial.MyEnum;
using Tutorial.Struct;
using UnityEngine;

namespace Tutorial
{
    // 작성자: 조혜찬
    // 튜토리얼 매니저
    public class TutorialManager : MonoBehaviour
    {
        public static TutorialManager Instance { get; private set; } // 외부에서 접근 가능한 인스턴스 프로퍼티

        private TutorialFSMVariables _fsmVariables; // 튜토리얼 fsm 관련 변수 모음 클래스

        private TutorialEventHandlerVariables _eventHandlerVariables; // 이벤트에 구독할 기능을 가지는 핸들러 변수 모음 클래스

        [SerializeField] private TutorialManagerData _tutorialManagerData; // Inspector 창에서 할당받을 변수를 가지는 구조체

        private void Awake()
        {
            Instance = this; // 자기 자신 할당

            GameModeManager.Instance.SetMode(new TutorialMode()); // 현재 게임 모드를 튜토리얼 모드로 할당
            
            _eventHandlerVariables = new TutorialEventHandlerVariables(_tutorialManagerData.tutorialOverlay, _tutorialManagerData.animationDuration);

            _fsmVariables = new TutorialFSMVariables();
            _fsmVariables.Init(); // 튜토리얼 fsm 관련 변수 초기화
        }

        private void OnEnable()
        {
            _eventHandlerVariables.Enable(); // 활성화 시 실행될 함수 실행
        }

        private void OnDisable()
        {
            _eventHandlerVariables.Disable(); // 비활성화 시 실행될 함수 실행
        }

        private void Start()
        {
            ChangeTutorialState(TutorialState.Intro); // 인트로 상태 시작
        }

        private void Update()
        {
            _fsmVariables.machine.Update(); // 현재 상태에서 지속 실행될 함수 호출
        }

        private void OnDestroy()
        {
            if (Instance == this) // 현재 인스턴스 자기 자신일 때
                Instance = null; // 초기화
        }

        private IState GetState(TutorialState state)
        {
            foreach(var states in _fsmVariables.tutorialStateMap) // 튜토리얼 상태 맵 순회
            {
                TutorialState tutorialState = states.Key; // 튜토리얼 상태 저장
                IState resultState = states.Value; // 실행되는 상태 저장

                if(tutorialState == state) // 찾는 튜토리얼 상태와 동일하다면
                {
                    return resultState; // 실행되는 상태 반환
                }
            }

            return null; // 아예 튜토리얼 상태 맵에 없다면 null 반환
        }

        // 튜토리얼 상태 변경 함수(변경 시킬 상태)
        public void ChangeTutorialState(TutorialState changeState)
        {
            _fsmVariables.currentState = changeState; // 현재 튜토리얼 상태를 변경 시킬 상태로 변경
            _fsmVariables.machine.ChangeState(GetState(changeState)); // 상태 변경
        }
    }
}
// 마지막 작성 일자: 2026.03.12