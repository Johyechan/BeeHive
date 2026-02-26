using InGame.MyUI.Card.Variable;
using InGame.MyUI.MyUIButton;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.MyUI.Card.Handler
{
    // 작성자: 조혜찬
    // ui 카드 초기화 핸들러
    public class UICardInitializeHandler
    {
        private UICardVariable _uiCardVariable; // ui카드 초기화에 필요한 변수들을 가지는 클래스

        public UICardInitializeHandler(UICardVariable uiCardVariable)
        {
            _uiCardVariable = uiCardVariable;
        }

        public void Init(CanvasGroup cardUsePanelCanvaseGroup, CanvasGroup cardInformationCanvasGroup)
        {
            _uiCardVariable.cardUsePanelCanvasGroup = cardUsePanelCanvaseGroup; // 매개 변수로 받은 카드 사용 여부를 묻는 패널의 캔버스 그룹 할당
            _uiCardVariable.cardInformationCanvasGroup = cardInformationCanvasGroup; // 매개 변수로 받은 카드 정보 패널의 캔버스 그룹 할당

            Transform cardInfoPanelTransform = _uiCardVariable.cardInformationCanvasGroup.transform.GetChild(1); // 카드 정보 패널의 두 번째 자식 - 패널
            Transform uiTransform = cardInfoPanelTransform.GetChild(0); // 패널의 첫 번째 자식 - UI를 모아둔 빈 객체
            Transform imageTransform = uiTransform.GetChild(0); // UI를 모아둔 빈 객체의 첫 번째 자식 - 이미지

            _uiCardVariable.cardInformationImage = imageTransform.GetComponent<Image>(); // 이미지 할당

            Transform cardUsePanelTransform = _uiCardVariable.cardUsePanelCanvasGroup.transform.GetChild(1); // 카드 사용 패널의 두 번째 자식 - 패널
            Transform buttonsTransform = cardUsePanelTransform.GetChild(1); // 패널의 첫 번째 자식 - 버튼을 모아둔 빈 객체
            Transform cardUseButtonTransform = buttonsTransform.GetChild(0); // 버튼을 모아둔 빈 객체의 첫 번째 자식 - 카드 사용 버튼

            _uiCardVariable.cardUseButton = cardUseButtonTransform.GetComponent<CardUseButton>(); // 버튼 할당
        }
    }
}
// 마지막 작성 일자: 2026.02.26