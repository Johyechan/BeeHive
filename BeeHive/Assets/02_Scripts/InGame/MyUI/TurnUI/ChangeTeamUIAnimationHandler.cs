using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.MyUI.TurnUI
{
    // 작성자: 조혜찬
    // 팀 변경할 때 나올 UI 애니메이션 클래스
    public class ChangeTeamUIAnimationHandler : TurnUIAnimationHandlerBase
    {
        public ChangeTeamUIAnimationHandler(CanvasGroup canvasGroup, TMP_Text tmpText) : base(canvasGroup, tmpText)
        {
        }

        public override void UIAnimationPlay()
        {
            throw new System.NotImplementedException();
        }
    }
}
// 마지막 작성 일자: 2025.08.01