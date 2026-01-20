using MyUtil;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace InGame
{
    // 작성자: 조혜찬
    // 이벤트 준비 완료 여부 대기 클래스
    public static class EventReady
    {
        private static int _pending; // 카운팅 변수

        private static TaskCompletionSource<bool> _isReady = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously); // TaskCreationOptions.RunContinuationsAsynchronously - SetResult를 호출한 함수가 완전히 return이 된 이후 await 이후 코드가 실행되도록 함

        // 대기하는 객체 수 추가 함수
        public static void Add()
        {
            Interlocked.Increment(ref _pending); // 1 증가 (Interlocked 덕분에 몇 개의 스레드에서 동시 접근하든 증가, 감소 연산이 누락되지 않고 중복 되지 않는다)
        }

        // 대기하는 객체 수 감소 함수(대기 완료)
        public static void CompletedOne()
        {
            if(Interlocked.Decrement(ref _pending) == 0) // 1 감소한 값이 0이면
            {
                _isReady?.TrySetResult(true); // 대기 종료
            }
        }

        public static void Reset()
        {
            _isReady = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
        }

        // 대기 함수
        public static Task WaitAsync()
        {
            return _isReady.Task;
        }
    }
}
// 마지막 작성 일자: 2026.01.20