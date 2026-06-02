using DG.Tweening;
using InGame.MyManager.Local;
using InGame.MyObject;
using InGame.MyObject.Piece;
using MyUtil.Interface;
using System.Threading.Tasks;
using Tutorial.Event;
using Tutorial.FSM;
using Tutorial.MyEnum;
using Tutorial.Struct;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization.Settings;

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

        public TutorialState CurrentTutorialState { get => _fsmVariables.currentState; }

        public bool TurnEnd { get; set; } // 턴 종료 확인 프로퍼티
        public bool InputOn { get; set; } = false; // 인풋 허용 프로퍼티
        public bool IsInputDelayOver { get; set; } = false; // 인풋 대기 시간 종료 여부

        public int TutorialRoadCreateCount { get; set; } = 0; // 도로 생성 개수

        private float _nextInputTime; // 다음 클릭 가능 시간

        private string _enterClick; // 엔터 클릭 문자열
        public string EnterClick { get => _enterClick; } // 엔터 클릭 문자열 프로퍼티

        private string _buttonClick; // 버튼 클릭 문자열
        public string ButtonClick { get => _buttonClick; } // 버튼 클릭 문자열 프로퍼티

        private string _targetClick; // 대상 클릭 문자열
        public string TargetClick { get => _targetClick; } // 대상 클릭 문자열 프로퍼티

        private void Awake()
        {
            Instance = this; // 자기 자신 할당
            
            _eventHandlerVariables = new TutorialEventHandlerVariables();
            _eventHandlerVariables.Init();

            _fsmVariables = new TutorialFSMVariables(_tutorialManagerData);
            _fsmVariables.Init(); // 튜토리얼 fsm 관련 변수 초기화

            _enterClick = LocalizationSettings.StringDatabase.GetLocalizedString(
                "Tutorial",
                "Tutorial_Click_Enter"
            );

            _buttonClick = LocalizationSettings.StringDatabase.GetLocalizedString(
                "Tutorial",
                "Tutorial_Click_Button"
            );

            _targetClick = LocalizationSettings.StringDatabase.GetLocalizedString(
                "Tutorial",
                "Tutorial_Click_Target"
            );
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
            _nextInputTime = Time.time + _tutorialManagerData.inputDelay; // 처음 inputDelay초 동안 입력 방지
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

        // AI 턴일 때 기물 배치 함수(배치 칸, 배치 기물, 이동 여부)
        public Task ObjectPlace(PlacePlaneObjectBase placePlaneObject, PieceBase piece, bool isMove)
        {
            float angle = 0; // 각도

            InGameContext.Current.Data.PlacePlaneManager.ChangePlacePlaneState(placePlaneObject, piece, isMove);

            RoadPlacePlaneObject roadPlacePlane = placePlaneObject as RoadPlacePlaneObject; // 도로 배치칸으로 형변환 시도

            if (roadPlacePlane) // 도로 배치 칸일 경우 (null이 아닐 경우)
            {
                angle = roadPlacePlane.RoadAngle; // 도로 각도 할당
            }

            return piece.MoveToPlacePlane(placePlaneObject.transform.parent, placePlaneObject.transform.localPosition, isMove, angle);
        }

        // 튜토리얼 UI 패널 세팅 함수
        public void SetTutorialPanel(bool showDimmer, string guideStr = "", string helpStr = "", float holeRadius = 0, float outlineWidth = 0, Vector4 holeCenter = default, Vector4 holeScale = default, Vector2 guideTxtPos = default)
        {
            if(showDimmer)
            {
                _tutorialManagerData.guideTxt.text = guideStr;
                _tutorialManagerData.helpTxt.text = helpStr;

                _tutorialManagerData.guideTxt.GetComponent<RectTransform>().anchoredPosition = guideTxtPos == default ? Vector2.zero : guideTxtPos;
                _tutorialManagerData.helpTxt.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, guideTxtPos.y - 100f);

                _tutorialManagerData.dimmerMat.SetFloat("_HoleRadius", holeRadius);
                _tutorialManagerData.dimmerMat.SetFloat("_OutlineWidth", outlineWidth);
                _tutorialManagerData.dimmerMat.SetVector("_HoleCenter", holeCenter == default ? Vector4.zero : holeCenter);
                _tutorialManagerData.dimmerMat.SetVector("_HoleScale", holeScale == default ? Vector4.zero : holeScale);

                if(_tutorialManagerData.tutorialBlockPanel.gameObject.activeSelf) // 클릭을 완전히 막는 패널이 활성화 상태라면
                {
                    // 클릭을 완전히 막는 패널 비활성화
                    _tutorialManagerData.tutorialBlockPanel.DOFade(0, _tutorialManagerData.animationDuration);
                    _tutorialManagerData.tutorialBlockPanel.gameObject.SetActive(false);
                }

                if(!_tutorialManagerData.tutorialDimmer.gameObject.activeSelf) // 클릭 가능한 위치를 정해주는 패널이 비활성화 상태라면
                {
                    // 클릭 가능한 위치를 정해주는 패널 활성화
                    _tutorialManagerData.tutorialDimmer.gameObject.SetActive(true);
                    _tutorialManagerData.tutorialDimmer.DOFade(1, _tutorialManagerData.animationDuration);
                }
            }
            else
            {
                if(_tutorialManagerData.tutorialDimmer.gameObject.activeSelf) // 클릭 가능한 위치를 정해주는 패널이 활성화 상태라면
                {
                    // 클릭 가능한 위치를 정해주는 패널 비활성화
                    _tutorialManagerData.tutorialDimmer.DOFade(0, _tutorialManagerData.animationDuration);
                    _tutorialManagerData.tutorialDimmer.gameObject.SetActive(false);
                }
                
                if(!_tutorialManagerData.tutorialBlockPanel.gameObject.activeSelf) // 클릭을 완전히 막는 패널이 비활성화 상태라면
                {
                    // 클릭을 완전히 막는 패널 활성화
                    _tutorialManagerData.tutorialBlockPanel.gameObject.SetActive(true);
                    _tutorialManagerData.tutorialBlockPanel.DOFade(1, _tutorialManagerData.animationDuration);
                }
            }
        }

        // 튜토리얼 상태 변경 함수(변경 시킬 상태)
        public void ChangeTutorialState(TutorialState changeState)
        {
            _fsmVariables.currentState = changeState; // 현재 튜토리얼 상태를 변경 시킬 상태로 변경
            _fsmVariables.machine.ChangeState(GetState(changeState)); // 상태 변경
        }

        public void OnEscape(InputAction.CallbackContext ctx)
        {
            Time.timeScale = 0; // 시간 멈추기
            _tutorialManagerData.tutorialEscapePanel.DOFade(1, _tutorialManagerData.animationDuration).SetUpdate(true); // 튜토리얼 종료 패널 페이드 인
            _tutorialManagerData.tutorialEscapePanel.gameObject.SetActive(true); // 활성화
        }

        // 인풋 딜레이 함수
        public void OnConfirm(InputAction.CallbackContext ctx)
        {
            switch(_fsmVariables.currentState)
            {
                case TutorialState.Intro:
                case TutorialState.Turn1_Player:
                case TutorialState.Turn1_AI:
                case TutorialState.Turn2_Player:
                case TutorialState.Turn3_AI:
                case TutorialState.Turn4_Player:
                case TutorialState.Turn5_Player:
                case TutorialState.Turn6_AI:
                    if(InputOn) // 인풋이 허용 됐을 때
                    {
                        if (Time.time >= _nextInputTime)
                        {
                            IsInputDelayOver = true;
                            _nextInputTime = Time.time + _tutorialManagerData.inputDelay; // 다음 클릭 가능 시간을 현재 시간 + 딜레이로 할당
                        }
                    }
                    break;
            }
        }
    }
}
// 마지막 작성 일자: 2026.06.02