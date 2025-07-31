using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.MyUI.TurnUI
{
    // 작성자: 조혜찬
    // 카드 뽑기 결정 턴에 나올 UI 애니메이션 클래스
    public class DrawTurnUIAnimationHandler : TurnUIAnimationHandlerBase
    {
        public DrawTurnUIAnimationHandler(CanvasGroup canvasGroup, TMP_Text tmpText) : base(canvasGroup, tmpText)
        {
        }

        public override void UIAnimationPlay()
        {
            throw new System.NotImplementedException();
        }
    }
}
// 마지막 작성 일자: 2025.08.01