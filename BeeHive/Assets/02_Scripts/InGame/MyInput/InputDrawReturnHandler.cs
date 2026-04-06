using InGame.MyEnum;
using InGame.MyEvent;
using InGame.MyManager.Global;
using InGame.MyManager.Local;
using InGame.MyObject;
using InGame.MySystem;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace InGame.MyInput
{
    // 작성자: 조혜찬
    // 드로우를 진행할 때 반환하는 경우를 가지는 핸들러
    public class InputDrawReturnHandler : MonoBehaviour
    {
        private Deck _deck;

        public InputDrawReturnHandler(Deck deck)
        {
            _deck = deck;
        }

        public bool IsReturn()
        {
            string warningEventStr = LocalizationSettings.StringDatabase.GetLocalizedString(
                "Game",
                "Game_UI_NotDrawTurn"
            );

            if (!WarningEvent.OnCheckCurrentTurn.Invoke(TurnType.DrawTurn, warningEventStr)) // 드로우 턴이 아니라면
            {
                return true; // 반환
            }

            if (InGameContext.Current.Data.TurnManager.CurrentTeamType != TeamManager.Instance.CurrentTeamType) // 내 팀의 턴이 아니라면
            {
                string str = LocalizationSettings.StringDatabase.GetLocalizedString(
                    "Game",
                    "Game_UI_NotYourTurn"
                );
                UIManager.Instance.WarningUIMake(str);
                return true; // 반환
            }

            if(_deck.transform.childCount <= 0) // 덱에 더 이상 카드가 없다면
            {
                string str = LocalizationSettings.StringDatabase.GetLocalizedString(
                    "Game",
                    "Game_UI_NoMoreCardInDeck"
                );
                InGameContext.Current.Data.DeckManager.IsEmpty = true;
                UIManager.Instance.WarningUIMake(str);
                return true; // 반환
            }

            if (!InGameContext.Current.Data.DrawManager.CanDraw) // 만약 Draw가 불가능하다면
            {
                string str = LocalizationSettings.StringDatabase.GetLocalizedString(
                    "Game",
                    "Game_UI_OnlyOneCardPerTurn"
                );
                UIManager.Instance.WarningUIMake(str);
                return true; // 반환
            }

            if (!WalletEvent.OnUseGoldBar.Invoke(2)) // 금괴 2개를 사용할 수 없다면
            {
                string str = LocalizationSettings.StringDatabase.GetLocalizedString(
                    "Game",
                    "Game_UI_NotEnoughGold",
                    new object[] { 2 }
                );
                UIManager.Instance.WarningUIMake(str);
                return true; // 반환
            }

            return false;
        }
    }
}
// 마지막 작성 일자: 2026.04.06