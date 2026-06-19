using UnityEngine;

namespace InGame.MyUI
{
    // 작성자: 조혜찬
    // 타이머 UI를 사용하는 부모 객체
    public class TimerUIParent : MonoBehaviour
    {
        [SerializeField] private TimerUI _timerUI; // 사용할 타이머 UI

        // 타이머 UI 사용 함수
        public void UseTimerUI(float waitTime)
        {
            _timerUI.gameObject.SetActive(true); // 객체 활성화
            _timerUI.TimerAnimationStart(waitTime);
        }

        // 타이머 UI 숨기기 함수
        public void TimerUIHide()
        {
            _timerUI.gameObject.SetActive(false);
        }
    }
}
// 마지막 작성 일자: 2026.06.19