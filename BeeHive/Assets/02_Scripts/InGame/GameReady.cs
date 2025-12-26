using System.Threading.Tasks;
using UnityEngine;

namespace InGame
{
    // 작성자: 조혜찬
    // 게임이 준비 여부 관리 정적 클래스
    public static class GameReady
    {
        private static TaskCompletionSource<bool> _isReady;

        public static Task WaitAsync()
        {
            _isReady = new TaskCompletionSource<bool>();
            return _isReady.Task; // 외부에서 사용할 게임 준비를 대기하는 함수
        }

        public static bool IsReady => _isReady.Task.IsCompleted; // 게임 준비 완료 여부 프로퍼티

        // 테스크 완료 함수
        public static void Completed()
        {
            _isReady?.TrySetResult(true);
        }
    }
}
// 마지막 작성 일자: 2025.12.26