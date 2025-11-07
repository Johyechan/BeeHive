using InGame.MyUI.MyUIInterface;
using UnityEngine;
using DG.Tweening;
using MyUtil;

namespace InGame.MyUI
{
    // 조혜찬
    // 패널을 띄우는 버튼 클래스
    public class ShowPanelButton : MonoBehaviour, IUIClick
    {
        [SerializeField] private CanvasGroup _targetPanel; // 보여줄 패널

        [SerializeField] private float _animationDuration; // 애니메이션 지속 시간

        // 클릭 시 실행될 함수
        public void OnUIClick()
        {
            MainThreadDispatcher.Enqueue(() =>
            {
                _targetPanel.gameObject.SetActive(true); // _targetPanel 활성화
            });
            MainThreadDispatcher.Enqueue(() =>
            {
                _targetPanel.DOFade(1, _animationDuration); // _targetPanel을 _animationDuration 동안 페이드 인
            });
        }
    }
}
// 마지막 작성 일자: 2025.11.07