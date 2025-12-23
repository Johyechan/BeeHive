using System.Threading.Tasks;
using UnityEngine;

namespace InGame
{
    // 작성자: 조혜찬
    // 턴이 자동으로 넘어가는 시간을 재는 타이머 클래스
    public static class TurnTimer
    {
        private static TaskCompletionSource<bool> _timerEndTcs;

        // 타이머 시작 함수
        public static async void TimerStart(int timer)
        {
            await Timer(timer);
        }

        // 타이머 함수
        private static async Task Timer(int timer)
        {
            await Task.Delay(timer * 1000); // 취소가 가능해야 하는데....


        }

        // 타이머 즉시 종료 함수
        public static void TimerEndImmediately()
        {

        }
    }
}
// 마지막 작성 일자: 2025.12.23