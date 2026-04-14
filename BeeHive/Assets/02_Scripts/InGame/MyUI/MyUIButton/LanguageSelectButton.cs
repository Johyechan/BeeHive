using DG.Tweening;
using InGame.MyUI.MyUIInterface;
using UnityEngine;

namespace InGame.MyUI.MyUIButton
{
    // 작성자: 조혜찬
    // 언어 선택 버튼
    public class LanguageSelectButton : MonoBehaviour, IUIClick
    {
        [SerializeField] private CanvasGroup _resolutionSelectCanvasGroup; // 해상도 선택 UI

        [SerializeField] private float _fadeDuration; // 페이드 인, 아웃에 걸리는 시간

        public void OnUIClick()
        {
            _resolutionSelectCanvasGroup.gameObject.SetActive(true); // 해상도 선택 UI 활성화
            _resolutionSelectCanvasGroup.DOFade(1, _fadeDuration); // 해상도 선택 UI 페이드 인
        }
    }
}
// 마지막 작성 일자: 2026.04.14