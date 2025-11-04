using InGame.MyObject;
using InGame.MyUI.Card.Handler;
using InGame.MyUI.MyUIButton;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.MyUI.Card.Variable
{
    // 작성자: 조혜찬
    // UI 카드에 필요한 변수들을 가지는 클래스
    public class UICardVariable
    {
        public CanvasGroup cardUsePanelCanvasGroup; // 카드 사용 여부를 묻는 패널의 캔버스 그룹 변수
        public CanvasGroup cardInformationCanvasGroup; // 카드 정보를 보여주는 패널의 캔버스 그룹 변수

        public Image cardInformationImage; // 카드 정보에 필요한 이미지 변수

        public TMP_Text cardInformationTmpText; // 카드 정보에 필요한 텍스트 변수

        public CardUseButton cardUseButton; // 카드 사용 버튼 변수

        public UICardInitializeHandler initializeHandler; // 초기화 핸들러

        public UICardShowInformationHandler showInformationHandler; // 카드 정보 보여주는 핸들러

        public UICardClickedHandler clickedHandler; // 클릭 시 실행될 기능을 가지는 핸들러

        public GameObject cardObj; // 현재 UI 카드에 맞는 객체

        public bool isMouseCursorOn;
        public bool isAnimationEnd = true; // 애니메이션 종료 여부 - 기본 상태: true

        public RectTransform rect;

        public UsedDeck usedCardDeck; // 사용한 카드들을 모아두는 덱

        public float originYPos; // 기본 Y 위치

        public int originIndex; // 자기자신의 기본 인덱스
    }
}
// 마지막 작성 일자: 2025.11.04