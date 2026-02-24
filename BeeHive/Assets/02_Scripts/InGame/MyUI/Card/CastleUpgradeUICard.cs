using InGame.MyEnum;
using InGame.MyManager;
using InGame.MyManager.Global;
using InGame.MyManager.Local;
using UnityEngine;

namespace InGame.MyUI.Card
{
    // 작성자: 조혜찬
    // 성벽 강화 카드
    public class CastleUpgradeUICard : UICardBase
    {
        // 카드 기능을 실제로 수행하는 함수
        public override bool UseCard()
        {
            if(InGameContext.Current.Data.CardManager.CheckSameTypeCardWasUsed(CardType.CastleUpgrade)) // 성벽 강화 카드 일전에 사용 했었는지 확인
            {
                return false;
            }

            UsedCardData usedCardData = new UsedCardData()
            {
                roomID = SceneMgr.Instance.CurrentRoomID, // 현재 방 ID
                usedCardType = (int)_uiCardData.poolType, // 사용한 카드의 이름
            };

            string json = JsonUtility.ToJson(usedCardData); // Json 형태로 변환
            NetworkManager.Instance.Socket.Emit("usedCard", json); // 서버로 카드를 사용했다고 전송
            // 성 체력 1증가
            InGameContext.Current.Data.GameManager.MyCastle.CastleUpgrade(); // 자기 자신 최대 체력 1 증가

            CastleHpUpInfo castleHpUpInfo = new CastleHpUpInfo()
            {
                roomID = SceneMgr.Instance.CurrentRoomID, // 현재 방 ID
                changeTeamType = (int)TeamManager.Instance.CurrentTeamType, // 체력이 바뀔 성의 팀 타입
                changedHp = InGameContext.Current.Data.GameManager.MyCastle.CurrentHp, // 바뀐 체력
            };
            string castleJson = JsonUtility.ToJson(castleHpUpInfo); // Json화
            NetworkManager.Instance.Socket.Emit("castleHpUp", castleJson); // 서버에 최대 체력이 올라간 팀 타입 알려주기

            return base.UseCard();
        }
    }
}
// 마지막 작성 일자: 2026.02.24