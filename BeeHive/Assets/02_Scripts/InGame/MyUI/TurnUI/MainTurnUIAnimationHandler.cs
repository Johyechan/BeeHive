using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.MyUI.TurnUI
{
    // 작성자: 조혜찬
    // 메인(생성 및 이동) 턴에 나올 UI 애니메이션 클래스
    public class MainTurnUIAnimationHandler : TurnUIAnimationHandlerBase
    {
        public MainTurnUIAnimationHandler(CanvasGroup canvasGroup, TMP_Text tmpText, float animationDuration) : base(canvasGroup, tmpText, animationDuration)
        {
        }

        public override Sequence UIAnimationPlay()
        {
            return DOTween.Sequence()
                .AppendCallback(() => _tmpText.text = "메인 턴") // 무슨 턴인지 텍스트로 보여주기
                .Append(base.UIAnimationPlay()); // 이후 동일하게 실행되어야 할 기능 수행
        }
    }
}
// 마지막 작성 일자: 2025.08.01