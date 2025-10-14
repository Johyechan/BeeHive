using DG.Tweening;
using InGame.MyUI.Card;
using InGame.MyUI.MyUIInterface;
using UnityEngine;

namespace InGame.MyUI.MyUIButton
{
    // 작성자: 조혜찬
    // 카드 사용 버튼 클래스
    public class CardUseButton : MonoBehaviour, IUIClick
    {
        [SerializeField] private CanvasGroup _canvasGroup; // 버튼의 부모 패널의 CanvasGroup

        [SerializeField] private float _animationDuration; // 애니메이션 지속 시간

        private UICardBase _uiCardBase; // 사용될 카드의 베이스 클래스
        public UICardBase UICardBase { get => _uiCardBase; set => _uiCardBase = value; } // 외부에서 할당하기 위한 프로퍼티

        // 클릭 시 실행될 함수
        public void OnUIClick()
        {
            _uiCardBase.UseCard(); // 카드 기능 실행

            DOTween.Sequence()
                .Append(_canvasGroup.DOFade(0, _animationDuration)) // 페이드 아웃
                .OnComplete(() => _canvasGroup.gameObject.SetActive(false)); // 객체 비활성화
            
        }
    }
}
// 마지막 작성 일자: 2025.10.14