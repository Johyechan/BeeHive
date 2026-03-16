using InGame.MyEnum;
using InGame.MyManager.Local;
using InGame.MyManager.Local.Turn;
using MyUtil.Interface;
using Tutorial.Event;
using Tutorial.MyEnum;
using UnityEngine;

namespace Tutorial.FSM.State
{
    // 작성자: 조혜찬
    // 튜토리얼 시작 상태 클래스
    public class TutorialIntroState : IState
    {
        private int _count; // 다음 설명을 보여주기 위한 카운팅에 사용할 변수

        public void Enter()
        {
            _count = 0;
        }

        public void Update()
        {
            if(TutorialManager.Instance.IsInputDelayOver()) // 인풋 딜레이가 지나고 인풋이 들어왔다면
            {
                _count++; // 카운팅
            }

            switch(_count) // 카운팅 된 수가
            {
                case 1:
                    TutorialManager.Instance.SetTutorialPanel(true, "그리고 현재 당신의 체력입니다.", 0.07f, 0.008f, new Vector4(0.443f, 0.958f), new Vector4(1f, 0.3f));
                    break;
                case 2:
                    TutorialManager.Instance.SetTutorialPanel(true, "현재 상대의 성이고,", 0.07f, 0.008f, new Vector4(0.5f, 0.78f), new Vector4(1f, 1f));
                    break;
                case 3:
                    TutorialManager.Instance.SetTutorialPanel(true, "그리고 현재 상대의 체력입니다.", 0.07f, 0.008f, new Vector4(0.565f, 0.958f), new Vector4(1.2f, 0.3f));
                    break;
                case 4:
                    TutorialManager.Instance.SetTutorialPanel(true, "당신이 바라보는 시점의 성이 당신의 팀이니 유의하세요.");
                    break;
                case 5:
                    TutorialManager.Instance.SetTutorialPanel(true, "상대를 공격하여 승리하세요!");
                    break;
                case 6:
                    TutorialEvents.OnIntroEnd?.Invoke(); // 인트로 종료 이벤트 호출
                    break;
            }
        }

        public void Exit()
        {
            _ = InGameContext.Current.Data.TurnManager.TurnChange(TurnType.ChangeTeam); // 턴 시작
        }
    }
}
// 마지막 작성 일자: 2026.03.16