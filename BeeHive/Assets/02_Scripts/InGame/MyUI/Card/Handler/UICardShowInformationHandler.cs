using DG.Tweening;
using InGame.MyManager.Local;
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

        private float _animationDuration; // 애니메이션 지속시간

        public UICardShowInformationHandler(UICardVariable uiCardVariable, float animationDuration)
        {
            _uiCardVariable = uiCardVariable;
            _animationDuration = animationDuration;
        }

        public void ShowInfomation()
        {
            _uiCardVariable.cardInformationImage.sprite = InGameContext.Current.Data.CardManager.CurrentUICard.UICardData.currentCardSprite; // 선택한 현재 카드 이미지 할당
            _uiCardVariable.cardTitle.text = InGameContext.Current.Data.CardManager.CurrentUICard.UICardData.currentCardName; // 선택한 현재 카드 이름
            _uiCardVariable.cardExplain.text = InGameContext.Current.Data.CardManager.CurrentUICard.UICardData.currentCardExplain; // 선택한 현재 카드 설명
            _uiCardVariable.cardInformation.text = InGameContext.Current.Data.CardManager.CurrentUICard.UICardData.currentCardExplain; // 선택한 현재 카드 설명

            _uiCardVariable.cardInformationCanvasGroup.gameObject.SetActive(true); // 카드 정보 패널 활성화
            _uiCardVariable.cardInformationCanvasGroup.DOFade(1, _animationDuration); // 카드 정보 패널 페이드 인
        }
    }
}
// 마지막 작성 일자: 2026.04.02