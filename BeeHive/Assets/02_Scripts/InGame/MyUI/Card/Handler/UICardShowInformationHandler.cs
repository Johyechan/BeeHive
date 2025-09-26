using DG.Tweening;
using InGame.MyUI.Card.Variable;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.MyUI.Card.Handler
{
    // 작성자: 조혜찬
    // 카드 정보를 보여주는 핸들러
    public class UICardShowInformationHandler : MonoBehaviour
    {
        private UICardVariable _uiCardVariable; // 카드 정보를 보여줄 때 필요한 변수들을 가지는 클래스

        private Image _currentCardImage; // 현재 카드의 이미지

        private string _cardInformationText; // 현재 카드의 기능 설명

        private float _animationDuration; // 애니메이션 지속시간

        public UICardShowInformationHandler(UICardVariable uiCardVariable, Image currentCardImage, string cardInformationText, float animationDuration)
        {
            _uiCardVariable = uiCardVariable;
            _currentCardImage = currentCardImage;
            _cardInformationText = cardInformationText;
            _animationDuration = animationDuration;
        }

        public void ShowInfomation()
        {
            _uiCardVariable.cardInformationImage = _currentCardImage; // 선택한 현재 카드 이미지 할당
            _uiCardVariable.cardInformationTmpText.text = _cardInformationText; // 선택한 카드의 기능 설명 할당

            _uiCardVariable.cardInformationCanvasGroup.gameObject.SetActive(true); // 카드 정보 패널 활성화
            _uiCardVariable.cardInformationCanvasGroup.DOFade(1, _animationDuration); // 카드 정보 패널 페이드 인
        }
    }
}
// 마지막 작성 일자: 2025.09.26