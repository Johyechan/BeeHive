using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.MyUI.TurnUI
{
    // 작성자: 조혜찬
    // 턴 종료에 나올 UI 애니메이션 클래스
    public class TurnEndUIAnimationHandler : TurnUIAnimationHandlerBase
    {
        public TurnEndUIAnimationHandler(CanvasGroup canvasGroup, TMP_Text tmpText) : base(canvasGroup, tmpText)
        {
        }

        public override void UIAnimationPlay()
        {
            throw new System.NotImplementedException();
        }
    }
}
// 마지막 작성 일자: 2025.08.01