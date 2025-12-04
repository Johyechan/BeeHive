using DG.Tweening;
using InGame.MyEvent;
using InGame.MyManager;
using UnityEngine;

namespace InGame.MyUI.Turn
{
    // 작성자: 조혜찬
    // 턴 UI 클래스
    public class TurnUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup; // TurnUI UI CanvasGroup

        [SerializeField] private float _animationDuration; // 애니메이션 지속시간

        private void OnEnable()
        {
            GameOverEvent.OnGameOver += FadeOut;
        }

        private void OnDisable()
        {
            GameOverEvent.OnGameOver -= FadeOut;
        }

        private void FadeOut()
        {
            _canvasGroup.DOFade(0, _animationDuration).SetUpdate(true);
        }
    }
}
// 마지막 작성 일자: 2025.12.04