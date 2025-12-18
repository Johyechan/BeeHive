using System.Threading.Tasks;
using UnityEngine;

namespace InGame
{
    // 작성자: 조혜찬
    // 방 준비 완료 여부 관리 클래스
    public static class RoomReady
    {
        private static TaskCompletionSource<bool> _isReady = new TaskCompletionSource<bool>();

        public static Task WaitAsync() => _isReady.Task;

        public static void Completed()
        {
            _isReady?.TrySetResult(true);
        }
    }
}
// 마지막 작성 일자: 2025.12.18