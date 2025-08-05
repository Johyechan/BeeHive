using UnityEngine;

namespace InGame.MyUI.MyUIInterface
{
    // 작성자: 조혜찬
    // UI에서 클릭의 공통 기능 구현을 강제하기 위한 인터페이스
    public interface IUIClick
    {
        // 클릭 시 실행될 함수
        public void OnUIClick();
    }
}
// 마지막 작성 일자: 2025.07.01