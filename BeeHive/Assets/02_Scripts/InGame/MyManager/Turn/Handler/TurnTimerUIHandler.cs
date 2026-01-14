using DG.Tweening;
using MyUtil;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.MyManager.Turn.Handler
{
    // 작성자: 조혜찬
    // 턴 타이머 UI 관련 기능 처리 클래스
    public class TurnTimerUIHandler
    {
        private Slider _timerSlider; // 타이머 슬라이더

        private int _turnDuration; // 턴 지속 시간

        private Tween _timerSliderTween; // 타이머 슬라이더 관련 트윈

        public TurnTimerUIHandler(Slider timerSlider, int turnDuration)
        {
            _timerSlider = timerSlider;
            _turnDuration = turnDuration;
        }

        public void Init()
        {
            NetworkManager.Instance.Socket.On("playTimerSlider", (data) =>
            {
                MainThreadDispatcher.Enqueue(() => { StartSliderTimer(); });
            });

            NetworkManager.Instance.Socket.On("resetTurnTimer", (data) =>
            {
                MainThreadDispatcher.Enqueue(() =>
                {
                    TurnManager.Instance.OnTurnTimerStop?.Invoke();
                });
            });
        }

        private void StartSliderTimer()
        {
            if (_timerSlider.value != 0) // 타이머 슬라이더가 0으로 초기화가 안되어있다면
                _timerSlider.value = 0; // 0으로 초기화

            _timerSliderTween = _timerSlider.DOValue(1, _turnDuration).SetEase(Ease.Linear); // 슬라이더를 채우는 트윈 실행
        }

        // 트윈 즉시 종료
        public void SliderTimerStop()
        {
            _timerSliderTween?.Kill();
            _timerSliderTween = null;
            _timerSlider.value = 0;
        }
    }
}
// 마지막 작성 일자: 2026.01.14