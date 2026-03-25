using MyUtil.FSM;
using MyUtil.Interface;
using System.Collections.Generic;
using Tutorial.FSM.State;
using Tutorial.FSM.State.Eighth;
using Tutorial.FSM.State.Fifth;
using Tutorial.FSM.State.First;
using Tutorial.FSM.State.Fourth;
using Tutorial.FSM.State.Second;
using Tutorial.FSM.State.Seventh;
using Tutorial.FSM.State.Sixth;
using Tutorial.FSM.State.Third;
using Tutorial.MyEnum;
using Tutorial.Struct;

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

        public TutorialFourthTurnAIState fourthTurnAIState; // 튜토리얼 네 번째(AI 턴) 상태

        public TutorialFifthTurnPlayerState fifthTurnPlayerState; // 튜토리얼 다섯 번째(플레이어 턴) 상태

        public TutorialFifthTurnAIState fifthTurnAIState; // 튜토리얼 다섯 번째(AI 턴) 상태

        public TutorialSixthTurnPlayerState sixthTurnPlayerState; // 튜토리얼 여섯 번째(플레이어 턴) 상태

        public TutorialSixthTurnAIState sixthTurnAIState; // 튜토리얼 여섯 번째(AI 턴) 상태

        public TutorialSeventhTurnPlayerState seventhTurnPlayerState; // 튜토리얼 일곱 번째(플레이어 턴) 상태

        public TutorialSeventhTurnAIState seventhTurnAIState; // 튜토리얼 일곱 번째(AI 턴) 상태

        public TutorialEighthTurnPlayerState eighthTurnPlayerState; // 튜토리얼 여덟 번째(플레이어 턴) 상태

        public TutorialEndState endState; // 튜토리얼 종료 상태

        public TutorialFSMVariables(TutorialManagerData tutorialManagerData)
        {
            machine = new StateMachine();
            introState = new TutorialIntroState();
            firstTurnPlayerState = new TutorialFirstTurnPlayerState();
            firstTurnAIState = new TutorialFirstTurnAIState(tutorialManagerData.firstTurnAIuseSoldier, tutorialManagerData.firstTurnAISoldierCreatePlace, tutorialManagerData.firstTurnAISoldierMovePlace, tutorialManagerData.roadParent, tutorialManagerData.firstTurnAIFirstRoadPlacePlane, tutorialManagerData.firstTurnAISecondRoadPlacePlane);
            secondTurnPlayerState = new TutorialSecondTurnPlayerState(tutorialManagerData.goldCoin1PlacePlanes, tutorialManagerData.goldCoin3PlacePlanes, tutorialManagerData.goldCoin5PlacePlanes);
            secondTurnAIState = new TutorialSecondTurnAIState(tutorialManagerData.secondTurnAIuseSoldier, tutorialManagerData.secondTurnAISoldierMovePlace, tutorialManagerData.roadParent, tutorialManagerData.secondTurnAIFirstRoadPlacePlane, tutorialManagerData.secondTurnAISecondRoadPlacePlane);
            thirdTurnPlayerState = new TutorialThirdTurnPlayerState();
            thirdTurnAIState = new TutorialThirdTurnAIState(tutorialManagerData.thirdTurnAIuseSoldier, tutorialManagerData.thirdTurnAISoldierMovePlace);
            fourthTurnPlayerState = new TutorialFourthTurnPlayerState();
            fourthTurnAIState = new TutorialFourthTurnAIState(tutorialManagerData.fourthTurnAIuseTank, tutorialManagerData.fourthTurnAITankCreatePlace, tutorialManagerData.fourthTurnAITankMovePlace, tutorialManagerData.confirmUI);
            fifthTurnPlayerState = new TutorialFifthTurnPlayerState();
            fifthTurnAIState = new TutorialFifthTurnAIState(tutorialManagerData.fifthTurnAIuseMiner, tutorialManagerData.fifthTurnAIMinerCreatePlace, tutorialManagerData.fifthTurnAIMinerMovePlace, tutorialManagerData.roadParent, tutorialManagerData.fifthTurnAIFirstRoadPlacePlane, tutorialManagerData.fifthTurnAISecondRoadPlacePlane);
            sixthTurnPlayerState = new TutorialSixthTurnPlayerState();
            sixthTurnAIState = new TutorialSixthTurnAIState(tutorialManagerData.sixthTurnAIuseSoldier, tutorialManagerData.sixthTurnAISoldierCreatePlace, tutorialManagerData.sixthTurnAISoldierMovePlace);
            seventhTurnPlayerState = new TutorialSeventhTurnPlayerState();
            seventhTurnAIState = new TutorialSeventhTurnAIState();
            eighthTurnPlayerState = new TutorialEighthTurnPlayerState();
            endState = new TutorialEndState();
        }

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
// 마지막 작성 일자: 2026.03.25