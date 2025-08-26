using DG.Tweening;
using InGame.MyEvent;
using InGame.MyManager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.MyUI.Turn
{
    // 작성자: 조혜찬
    // 팀 변경할 때 나올 UI 애니메이션 클래스
    public class ChangeTeamHandler : TurnUIAnimationHandlerBase
    {
        public ChangeTeamHandler(CanvasGroup canvasGroup, TMP_Text tmpText, float animationDuration) : base(canvasGroup, tmpText, animationDuration)
        {
        }

        public override Sequence UIAnimationPlay()
        {
            return DOTween.Sequence()
                .AppendCallback(() => TurnEvents.OnSetInteractable?.Invoke(false)) // 턴 넘기기 버튼 상화작용 비활성화
                .AppendCallback(() => _tmpText.text = TurnManager.Instance.CurrentTeamType.ToString() + " 턴") // 무슨 턴인지 텍스트로 보여주기
                .Append(base.UIAnimationPlay()); // 이후 동일하게 실행되어야 할 기능 수행
        }
    }
}
// 마지막 작성 일자: 2025.08.26