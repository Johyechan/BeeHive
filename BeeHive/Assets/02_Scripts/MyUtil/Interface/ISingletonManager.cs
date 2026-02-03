using UnityEngine;

namespace MyUtil.Interface
{
    // 작성자: 조혜찬
    // 싱글톤 매니저 인터페이스
    public interface ISingletonManager
    {
        public bool IsReady { get; } // 준비 완료 확인 변수

        public void Ready(); // 준비 완료 함수
    }
}
// 마지막 작성 일자: 2026.02.03