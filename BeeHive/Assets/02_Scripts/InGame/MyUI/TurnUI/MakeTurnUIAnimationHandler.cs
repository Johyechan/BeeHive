using DG.Tweening;
using InGame.MyEvent;
using InGame.MyManager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.MyUI.TurnUI
{
    // 작성자: 조혜찬
    // 생산 턴에 나올 UI 애니메이션 클래스
    public class MakeTurnUIAnimationHandler : TurnUIAnimationHandlerBase
    {
        public MakeTurnUIAnimationHandler(CanvasGroup canvasGroup, TMP_Text tmpText, float animationDuration) : base(canvasGroup, tmpText, animationDuration)
        {

        }

        // 애니메이션 실행 함수
        public override Sequence UIAnimationPlay()
        {
            return DOTween.Sequence()
                .AppendCallback(() => TurnChangeButtonEvent.OnSetInteractable?.Invoke(false)) // 턴 넘기기 버튼 상화작용 비활성화
                .AppendCallback(() => _tmpText.text = "생산 턴") // 무슨 턴인지 텍스트로 보여주기
                .Append(base.UIAnimationPlay()) // 이후 동일하게 실행되어야 할 기능 수행
                .AppendCallback(() => MakeTurnEvent.OnMakeTurn?.Invoke()); // 생산 턴에 실행되어야 할 기능 받은 액션 실행
        }
    }
}
// 마지막 작성 일자: 2025.08.22