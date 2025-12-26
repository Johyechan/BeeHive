using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace InGame.MyManager.Turn.Handler
{
    // 작성자: 조혜찬
    // 턴이 자동으로 넘어가는 시간을 재는 타이머 클래스
    public class TurnTimerHandler
    {
        private CancellationTokenSource _cts; // 취소 스위치

        public event Action<Slider, int> OnTimerStart; // 타이머 시작 이벤트
        public event Action<Slider> OnTimerStop; // 타이머 종료 이벤트

        // 턴 타이머 시작
        public void TurnTimerStart(Slider timerSlider, int time)
        {
            if(_cts != null) // 취소 스위치가 존재할 경우
            {
                _cts.Cancel(); // 취소 신호 발생
                _cts.Dispose(); // 내부 리소스 해제
                _cts = null; // null로 객체 비우기
            }

            _cts = new CancellationTokenSource(); // 새 취소 스위치 할당
            _ = Timer(timerSlider, time, _cts.Token); // 타이머 실행
        }

        public void TurnTimerStopImmediately()
        {
            _cts?.Cancel(); // 취소 스위치가 존재할 경우 취소 신호 발생
        }

        // 턴 타이머
        private async Task Timer(Slider timerSlider, int time, CancellationToken token)
        {
            try
            {
                OnTimerStart?.Invoke(timerSlider, time); // 턴 타이머 실행 이벤트 호출
                await Task.Delay(time * 1000, token); // time초 만큼 대기 또는 token 취소 발생 시 대기 종료
                TurnTimerEnd(timerSlider); // 턴 종료
            }
            catch(OperationCanceledException) // 취소 발생 시
            {
                TurnTimerEnd(timerSlider); // 턴 종료
            }
        }

        // 턴 종료 함수
        private void TurnTimerEnd(Slider timerSlider)
        {
            if (TurnManager.Instance.CurrentTeamType == TeamManager.Instance.CurrentTeamType) // 현재 턴의 팀이 내 팀일 경우
            {
                NetworkManager.Instance.Socket.Emit("changeTurn", SceneMgr.Instance.CurrentRoomID); // 서버에 턴 변경 이벤트 전달
                TurnManager.Instance.CanChangeTurn = false; // 턴 변경 가능 여부 false로 초기화
                OnTimerStop?.Invoke(timerSlider); // 턴 타이머 종료 이벤트 호출
            }
        }
    }
}
// 마지막 작성 일자: 2025.12.26