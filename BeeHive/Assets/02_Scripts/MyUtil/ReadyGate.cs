using System.Threading.Tasks;
using UnityEngine;

namespace MyUtil
{
    public sealed class ReadyGate // 상속을 금지하는 클래스
    {
        private TaskCompletionSource<bool> _tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

        // 대기 함수
        public Task WaitAsync()
        {
            return _tcs.Task;
        }

        // 재사용을 위한 초기화 함수
        public void Reset()
        {
            _tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
        }

        // 대기 완료 함수
        public void Completed()
        {
            _tcs?.TrySetResult(true);
        }
    }
}
// 마지막 작성 일자: 2026.01.20