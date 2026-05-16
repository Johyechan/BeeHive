using DG.Tweening;
using InGame.MyEnum;
using InGame.MyManager;
using InGame.MyManager.Local;
using InGame.MyManager.Turn;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

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
            InGameContext.Current.Data.CardManager.ResetCardUse();

            string turn = LocalizationSettings.StringDatabase.GetLocalizedString(
                "Game",
                "Game_UI_Turn"
            );

            string team = "";

            switch(InGameContext.Current.Data.TurnManager.CurrentTeamType)
            {
                case TeamType.Team1:
                    team = LocalizationSettings.StringDatabase.GetLocalizedString(
                        "Game",
                        "Game_Red"
                    );
                    break;
                case TeamType.Team2:
                    team = LocalizationSettings.StringDatabase.GetLocalizedString(
                        "Game",
                        "Game_Blue"
                    );
                    break;
            }

            await DOTween.Sequence()
                .AppendCallback(() => _tmpText.text = $"{team} {turn}") // 무슨 턴인지 텍스트로 보여주기
                .AsyncWaitForCompletion(); // 이후 동일하게 실행되어야 할 기능 수행

            await base.UIAnimationPlay();
        }
    }
}
// 마지막 작성 일자: 2026.05.16