using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine;

namespace MyUtil
{
    // 작성자: 조혜찬
    // DOTween관련 확장 메서드를 가지는 클래스
    public static class DOTweenExtensions
    {
        // Task와 연동하여 사용가능한 DOTween 메서드
        public static Task AsyncWait(this Sequence sequence)
        {
            var tcs = new TaskCompletionSource<bool>(); // 외부에서 완료를 알릴 수 있는 객체 Task 생성
            sequence.OnComplete(() => tcs.SetResult(true)); // 시퀀스가 완료되었을 때 Task가 완료됨
            return tcs.Task; // Task를 반환하여 이 반환한 Task를 await 할 수 있음
        }
    }
}
// 마지막 작성 일자: 2025.08.26