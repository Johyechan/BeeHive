using InGame.MyEvent;
using InGame.MyManager;
using UnityEngine;

namespace InGame.MyUI.Card
{
    // 작성자: 조혜찬
    // 풍년 카드
    public class GoodHarvestUICard : UICardBase
    {
        // 카드 기능을 실제로 수행하는 함수
        public override void UseCard()
        {
            if (TurnManager.Instance.CurrentTeamType != TeamManager.Instance.CurrentTeamType) // 자신의 턴이 아닐 경우
                return; // 반환

            NetworkManager.Instance.Socket.Emit("debug", "금괴 4개 획득");
            // 금괴 4개 획득(가뭄 카드의 효과를 받지 않음)
            WalletEvent.OnGetGoldBar?.Invoke(4);

            base.UseCard();
        }
    }
}
// 마지막 작성 일자: 2025.10.14