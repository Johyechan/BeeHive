using DG.Tweening;
using InGame.MyManager;
using InGame.MyManager.Global;
using InGame.MyManager.Local;
using InGame.MyManager.Turn;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace InGame.MyUI.Turn
{
    // 작성자: 조혜찬
    // 메인(생성 및 이동) 턴에 나올 UI 애니메이션 클래스
    public class MainTurnHandler : TurnUIAnimationHandlerBase
    {
        public MainTurnHandler(CanvasGroup canvasGroup, TMP_Text tmpText, float animationDuration) : base(canvasGroup, tmpText, animationDuration)
        {
        }

        public override async Task UIAnimationPlay()
        {
            await DOTween.Sequence()
                .AppendCallback(() => _tmpText.text = "메인 턴") // 무슨 턴인지 텍스트로 보여주기
                .AsyncWaitForCompletion(); // 이후 동일하게 실행되어야 할 기능 수행

            await base.UIAnimationPlay();

            await DOTween.Sequence()
                .AppendCallback(() =>
                {
                    if (InGameContext.Current.Data.TurnManager.CurrentTeamType == TeamManager.Instance.CurrentTeamType) // 내 팀 차례일때
                    {
                        InGameContext.Current.Data.TurnManager.CanChangeTurn = true; // 턴 변경 버튼으로 넘기기 가능
                    }
                }).AsyncWaitForCompletion();
        }
    }
}
// 마지막 작성 일자: 2026.02.03