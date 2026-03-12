using MyUtil.FSM;
using MyUtil.Interface;
using System.Collections.Generic;
using Tutorial.FSM.State;
using Tutorial.FSM.State.First;
using Tutorial.FSM.State.Fourth;
using Tutorial.FSM.State.Second;
using Tutorial.FSM.State.Third;
using Tutorial.MyEnum;

namespace Tutorial.FSM
{
    // 작성자: 조혜찬
    // 튜토리얼 FSM 관련 변수 모음 클래스
    public class TutorialFSMVariables
    {
        public TutorialState currentState; // 현재 튜토리얼 상태

        public StateMachine machine; // 상태 관리 머신

        public Dictionary<TutorialState, IState> tutorialStateMap = new (); // 튜토리얼 상태와 실행 되는 상태를 연결 짓는 맵

        public TutorialIntroState introState; // 튜토리얼 시작 상태

        public TutorialFirstTurnPlayerState firstTurnPlayerState; // 튜토리얼 첫 번째 턴(플레이어 턴) 상태

        public TutorialFirstTurnAIState firstTurnAIState; // 튜토리얼 첫 번째 턴(AI 턴) 상태

        public TutorialSecondTurnPlayerState secondTurnPlayerState; // 튜토리얼 두 번째 턴(플레이어 턴) 상태

        public TutorialSecondTurnAIState secondTurnAIState; // 튜토리얼 두 번째 턴(AI 턴) 상태

        public TutorialThirdTurnPlayerState thirdTurnPlayerState; // 튜토리얼 세 번째 턴(플레이어 턴) 상태

        public TutorialThirdTurnAIState thirdTurnAIState; // 튜토리얼 세 번째 턴(AI 턴) 상태

        public TutorialFourthTurnPlayerState fourthTurnPlayerState; // 튜토리얼 네 번째(플레이어 턴) 상태

        public TutorialEndState endState; // 튜토리얼 종료 상태

        // 초기화 함수
        public void Init()
        {
            tutorialStateMap.Add(TutorialState.Intro, introState);
            tutorialStateMap.Add(TutorialState.Turn1_Player, firstTurnPlayerState);
            tutorialStateMap.Add(TutorialState.Turn1_AI, firstTurnAIState);
            tutorialStateMap.Add(TutorialState.Turn2_Player, secondTurnPlayerState);
            tutorialStateMap.Add(TutorialState.Turn2_AI, secondTurnAIState);
            tutorialStateMap.Add(TutorialState.Turn3_Player, thirdTurnPlayerState);
            tutorialStateMap.Add(TutorialState.Turn3_AI, thirdTurnAIState);
            tutorialStateMap.Add(TutorialState.Turn4_Player, fourthTurnPlayerState);
            tutorialStateMap.Add(TutorialState.End, endState);
        }
    }
}
// 마지막 작성 일자: 2026.03.12