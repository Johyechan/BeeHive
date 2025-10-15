using InGame.MyManager;
using UnityEngine;

namespace InGame.MyUI.Card
{
    // 작성자: 조혜찬
    // 성벽 강화 카드
    public class CastleUpgradeUICard : UICardBase
    {
        // 카드 기능을 실제로 수행하는 함수
        public override void UseCard()
        {
            UsedCardData usedCardData = new UsedCardData()
            {
                roomID = SceneMgr.Instance.CurrentRoomID, // 현재 방 ID
                usedCardName = _uiCardData.currentCardName, // 사용한 카드의 이름
                usedCardInformation = _uiCardData.cardInformationText, // 사용한 카드의 정보(효과)
            };

            string json = JsonUtility.ToJson(usedCardData); // Json 형태로 변환
            NetworkManager.Instance.Socket.Emit("usedCard", json); // 서버로 카드를 사용했다고 전송

            NetworkManager.Instance.Socket.Emit("debug", "체력 1 증가");
            // 성 체력 1증가
            GameManager.Instance.MyCastle.HP++; // 최대 체력 1 증가
            NetworkManager.Instance.Socket.Emit("castleHpUp", (int)TeamManager.Instance.CurrentTeamType); // 서버에 최대 체력이 올라간 팀 타입 알려주기

            base.UseCard();
        }
    }
}
// 마지막 작성 일자: 2025.10.15