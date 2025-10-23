using DG.Tweening;
using InGame.MyManager;
using InGame.MyUI.Card.Variable;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.Rendering.DebugUI;

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

        public void ShowAskPanel()
        {
            _uiCardVariable.cardUseButton.UICardBase = _uiCardBase; // 실행될 카드 할당

            _uiCardVariable.cardUsePanelCanvasGroup.gameObject.SetActive(true); // UI 활성화

            Transform panel = _uiCardVariable.cardUsePanelCanvasGroup.transform.GetChild(0); // 패널 가져오기
            TMP_Text askText = panel.GetChild(0).GetComponent<TMP_Text>(); // 패널의 묻는 텍스트 가져오기

            askText.text = _uiCardBase.UICardData.currentCardName + "카드를 사용하시겠습니까?"; // 텍스트 설정
            _uiCardVariable.cardUsePanelCanvasGroup.DOFade(1, _animationDuration); // UI 페이드 인
        }
    }
}
// 마지막 작성 일자: 2025.10.23