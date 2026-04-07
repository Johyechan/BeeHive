using InGame.MyEvent;
using InGame.MyManager.Global;
using MyUtil.Interface;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Tutorial.FSM.State
{
    // 작성자: 조혜찬
    // 튜토리얼 종료 상태 클래스
    public class TutorialEndState : IState
    {
        public void Enter()
        {
            string end = LocalizationSettings.StringDatabase.GetLocalizedString(
                "Tutorial",
                "Tutorial_End"
            );
            TutorialManager.Instance.SetTutorialPanel(true, end, TutorialManager.Instance.ButtonClick, 0.2f, 0.008f, new Vector4(0.5f, 0.247f), new Vector4(1f, 0.3f), new Vector2(0, 400f));
            GameOverEvent.OnGameOver?.Invoke();
        }

        public void Exit()
        {
            
        }

        public void Update()
        {
            
        }
    }
}
// 마지막 작성 일자: 2026.04.07