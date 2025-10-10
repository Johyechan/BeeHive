using DG.Tweening;
using InGame.MyUI.Card.Variable;
using UnityEngine;

namespace InGame.MyUI.Card.Handler
{
    // 작성자: 조혜찬
    // 클릭 시 실행될 기능 핸들러
    public class UICardClickedHandler : MonoBehaviour
    {
        private UICardBase _uiCardBase; // ui 카드

        private UICardVariable _uiCardVariable; // 클릭 시 실행될 기능에 필요한 변수들을 가지는 클래스

        private float _animationDuration; // 애니메이션 지속시간

        public UICardClickedHandler(UICardBase uiCardbase, UICardVariable uiCardVariable, float animationDuration)
        {
            _uiCardBase = uiCardbase;
            _uiCardVariable = uiCardVariable;
            _animationDuration = animationDuration;
        }

        public void Clicked()
        {
            _uiCardVariable.cardUseButton.onClick.RemoveAllListeners(); // 클릭 이벤트 초기화
            _uiCardVariable.cardUseButton.onClick.AddListener(() => _uiCardBase.UseCard()); // 이벤트 추가 - ui 카드의 카드 사용 함수
            _uiCardVariable.cardUseButton.onClick.AddListener(() => FadeInOut(0)); // 이벤트 추가 - 카드 사용 여부 패널 페이드 아웃

            DOTween.Sequence()
                .AppendCallback(() => ReverseCardObject()) // 카드 객체 뒤집기
                .OnComplete(() => FadeInOut(1)); // 카드 사용 여부 패널 페이드 인
        }

        // 카드 사용 여부 패널 페이드 인 앤 아웃 함수
        private void FadeInOut(float value)
        {
            // 여기 패널 안뜸 로그 찍으면서 버그 해결 ㄱㄱ
            _uiCardVariable.cardUsePanelCanvasGroup.DOFade(value, _animationDuration);
        }

        // UI 카드에 맞는 카드 객체를 뒤집는 함수
        private void ReverseCardObject()
        {
            _uiCardVariable.cardObj.transform.DORotate(new Vector3(0, 0, 180), _animationDuration);
            _uiCardVariable.cardObj.transform.DOMoveY(0.0001f, _animationDuration);
        }
    }
}
// 마지막 작성 일자: 2025.10.10