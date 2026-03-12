using UnityEngine;

namespace MyUtil.Interface
{
    // 작성자: 조혜찬
    // 이벤트 기능 핸들러 인터페이스
    public interface IEventHandler
    {
        public void Enable(); // 활성화 시 실행될 함수

        public void Disable(); // 비활성화 시 실행될 함수

        public void Function(); // 실제 기능 함수
    }
}
// 마지막 작성 일자: 2026.03.12