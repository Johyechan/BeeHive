using InGame.MyEnum;
using InGame.MyEvent;
using InGame.MyManager;
using InGame.MyManager.MyCard;
using InGame.MyManager.Turn;
using InGame.MyObject;
using System.Threading.Tasks;
using UnityEngine;

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

        public async Task<bool> IsReturn()
        {
            if(!await WarningEvent.OnCheckCurrentTurn?.Invoke(TurnType.DrawTurn, "드로우 턴이 아닙니다.")) // 드로우 턴이 아니라면
            {
                return true; // 반환
            }

            if (TurnManager.Instance.CurrentTeamType != TeamManager.Instance.CurrentTeamType) // 내 팀의 턴이 아니라면
            {
                return true; // 반환
            }

            if(_deck.transform.childCount <= 0) // 덱에 더 이상 카드가 없다면
            {
                DeckManager.Instance.IsEmpty = true;
                return true; // 반환
            }

            if (!DrawManager.Instance.IsCanDraw) // 만약 Draw가 불가능하다면
            {
                return true; // 반환
            }

            if (!await WalletEvent.OnUseGoldBar.Invoke(2)) // 금괴 2개를 사용할 수 없다면
            {
                return true; // 반환
            }

            return false;
        }
    }
}
// 마지막 작성 일자: 2025.11.13