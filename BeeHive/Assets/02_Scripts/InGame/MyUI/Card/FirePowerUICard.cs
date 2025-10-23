using InGame.MyManager;
using InGame.MyManager.MyPiece;
using UnityEngine;

namespace InGame.MyUI.Card
{
    // 작성자: 조혜찬
    // 화력 카드
    public class FirePowerUICard : UICardBase
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

            return base.UseCard();
        }
    }
}
// 마지막 작성 일자: 2025.10.20