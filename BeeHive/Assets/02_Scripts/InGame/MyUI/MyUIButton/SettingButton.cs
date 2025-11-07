using DG.Tweening;
using InGame.MyUI.MyUIInterface;
using MyUtil;
using UnityEngine;

namespace InGame.MyUI.MyUIButton
{
    // 작성자: 조혜찬
    // 세팅 버튼
    public class SettingButton : MonoBehaviour, IUIClick
    {
        [SerializeField] private CanvasGroup _canvasGroup; // UI 페이드 인, 아웃 기능을 수행할 CanvasGroup 변수

        [SerializeField] private float _animationDuration; // 애니메이션(페이드 인, 아웃) 지속 시간

        // 클릭 시 실행될 함수
        public void OnUIClick()
        {
            MainThreadDispatcher.Enqueue(() =>
            {
                _canvasGroup.gameObject.SetActive(true); // 세팅 UI 객체의 부모 활성화
            });

            MainThreadDispatcher.Enqueue(() =>
            {
                _canvasGroup.DOFade(1, _animationDuration); // 페이드 인
            });
        }
    }
}
// 마지막 작성 일자: 2025.11.07