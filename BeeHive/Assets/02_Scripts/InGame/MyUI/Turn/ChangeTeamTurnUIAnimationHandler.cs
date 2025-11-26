using DG.Tweening;
using InGame.MyManager;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace InGame.MyUI.Turn
{
    // 작성자: 조혜찬
    // 팀 변경할 때 나올 UI 애니메이션 클래스
    public class ChangeTeamTurnUIAnimationHandler : TurnUIAnimationHandlerBase
    {
        public ChangeTeamTurnUIAnimationHandler(CanvasGroup canvasGroup, TMP_Text tmpText, float animationDuration) : base(canvasGroup, tmpText, animationDuration)
        {
        }

        public override async Task UIAnimationPlay()
        {
            CardManager.Instance.ResetCardUse();

            await DOTween.Sequence()
                .AppendCallback(() => _tmpText.text = TurnManager.Instance.CurrentTeamType.ToString() + " 턴") // 무슨 턴인지 텍스트로 보여주기
                .AsyncWaitForCompletion(); // 이후 동일하게 실행되어야 할 기능 수행

            await base.UIAnimationPlay();
        }
    }
}
// 마지막 작성 일자: 2025.11.26