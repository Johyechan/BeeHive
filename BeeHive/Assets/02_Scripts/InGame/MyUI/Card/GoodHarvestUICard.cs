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
        public override bool UseCard()
        {
            UsedCardData usedCardData = new UsedCardData()
            {
                roomID = SceneMgr.Instance.CurrentRoomID, // 현재 방 ID
                usedCardName = _uiCardData.currentCardName, // 사용한 카드의 이름
                usedCardInformation = _uiCardData.cardInformationText, // 사용한 카드의 정보(효과)
            };

            string json = JsonUtility.ToJson(usedCardData); // Json 형태로 변환
            NetworkManager.Instance.Socket.Emit("usedCard", json); // 서버로 카드를 사용했다고 전송

            if (TurnManager.Instance.CurrentTeamType != TeamManager.Instance.CurrentTeamType) // 자신의 턴이 아닐 경우
                return false; // 반환

            // 금괴 4개 획득(가뭄 카드의 효과를 받지 않음)
            WalletEvent.OnGetGoldBar?.Invoke(4);

            return base.UseCard();
        }
    }
}
// 마지막 작성 일자: 2025.10.20