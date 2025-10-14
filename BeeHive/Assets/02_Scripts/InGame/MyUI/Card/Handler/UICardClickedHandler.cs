using DG.Tweening;
using InGame.MyManager;
using InGame.MyUI.Card.Variable;
using System;
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

        public void Clicked()
        {
            _uiCardVariable.cardUseButton.UICardBase = _uiCardBase; // 실행될 카드 할당

            _uiCardVariable.cardUsePanelCanvasGroup.gameObject.SetActive(true); // UI 활성화

            DOTween.Sequence()
                .AppendCallback(() => ReverseCardObject()) // 카드 객체 뒤집기
                .Append(_uiCardVariable.cardUsePanelCanvasGroup.DOFade(1, _animationDuration)); // UI 페이드 인
        }

        // UI 카드에 맞는 카드 객체를 뒤집는 함수
        private void ReverseCardObject()
        {
            _uiCardVariable.cardObj.transform.DORotate(new Vector3(0, _uiCardVariable.cardObj.transform.eulerAngles.y, 180), _animationDuration); // y축은 Team1의 경우 플레이어의 시야를 고려하여 180도 돌아가 있기 때문에 카드의 y값으로 그대로 적용, z축으로 180도 회전
            _uiCardVariable.cardObj.transform.DOMoveY(0.0001f, _animationDuration); // y축을 조금 올리는 이유는 안 올릴 경우 바닥을 뚫는 문제 발생
        }
    }
}
// 마지막 작성 일자: 2025.10.14