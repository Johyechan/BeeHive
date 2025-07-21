using MyUtil;
using UnityEngine;

namespace InGame.MyManager
{
    // 작성자: 조혜찬
    // UI 관련 싱글톤 매니저 클래스
    public class UIManager : MonoSingleton<UIManager>
    {
        private bool _canInteractionUI; // UI 상호작용 가능 여부 변수
        public bool CanInteractionUI { get { return _canInteractionUI; } set { _canInteractionUI = value; } } // UI 상호작용 가능 여부 프로퍼티

        protected override void Awake()
        {
            _canInteractionUI = true; // 처음에는 UI 상호작용 가능하도록 초기화
        }
    }
}
// 마지막 작성 일자: 2025.07.21