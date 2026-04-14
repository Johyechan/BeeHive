using UnityEngine;

namespace InGame.MyUI.MyUIInterface
{
    // 작성자: 조혜찬
    // 토글 이벤트 인터페이스
    public interface IToggleChanged
    {
        public void OnToggleChanged(bool isOn);
    }
}
// 마지막 작성 일자: 2026.04.14