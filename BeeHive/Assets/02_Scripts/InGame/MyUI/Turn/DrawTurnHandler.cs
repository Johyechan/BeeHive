using DG.Tweening;
using InGame.MyEvent;
using InGame.MyManager;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace InGame.MyUI.Turn
{
    // 작성자: 조혜찬
    // 드로우(카드 뽑기 결정) 턴에 나올 UI 애니메이션 클래스
    public class DrawTurnHandler : TurnUIAnimationHandlerBase
    {
        public DrawTurnHandler(CanvasGroup canvasGroup, TMP_Text tmpText, float animationDuration) : base(canvasGroup, tmpText, animationDuration)
        {
        }

        public override async Task UIAnimationPlay()
        {
            await DOTween.Sequence()
                .AppendCallback(() => _tmpText.text = "드로우 턴") // 무슨 턴인지 텍스트로 보여주기
                .AsyncWaitForCompletion(); // 이후 동일하게 실행되어야 할 기능 수행

            await base.UIAnimationPlay();

            await DOTween.Sequence()
                .AppendCallback(() =>
                {
                    if (TurnManager.Instance.CurrentTeamType == TeamManager.Instance.CurrentTeamType) // 내 팀 차례일때
                    {
                        TurnManager.Instance.CanChangeTurn = true; // 턴 변경 버튼으로 넘기기 가능
                        TurnEvents.OnSetInteractable?.Invoke(true); // 턴 넘기기 버튼 상화작용 활성화
                    }
                }).AsyncWaitForCompletion();

            
        }
    }
}
// 마지막 작성 일자: 2025.09.23