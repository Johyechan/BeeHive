using InGame.MyEnum;
using InGame.MyManager;
using InGame.MyManager.Global;
using InGame.MyManager.Local;
using MyUtil.GameMode;
using System.Threading.Tasks;
using UnityEngine;

namespace InGame.MyUI.Card
{
    // 작성자: 조혜찬
    // 화력 카드
    public class FirePowerUICard : UICardBase
    {
        // 카드 기능을 실제로 수행하는 함수
        public override async Task<bool> UseCard()
        {
            if (InGameContext.Current.Data.CardManager.CheckSameTypeCardWasUsed(CardType.FirePower)) // 화력 카드 일전에 사용 했었는지 확인
            {
                return false;
            }

            UsedCardData usedCardData = new UsedCardData()
            {
                roomID = SceneMgr.Instance.CurrentRoomID, // 현재 방 ID
                usedCardType = (int)_uiCardData.poolType, // 사용한 카드의 이름
            };

            InGameContext.Current.Data.CardManager.UsedCardShowOver = new TaskCompletionSource<bool>(); // 사용한 카드 보여주기 끝날 때까지 대기할 tcs 발급
            string json = JsonUtility.ToJson(usedCardData); // Json 형태로 변환
            if (GameModeManager.Instance.CurrentGameMode.UseServer())
                NetworkManager.Instance.Socket.Emit("usedCard", json); // 서버로 카드를 사용했다고 전송


            await InGameContext.Current.Data.CardManager.UsedCardShowOver?.Task; // tcs 대기

            return await base.UseCard();
        }
    }
}
// 마지막 작성 일자: 2026.05.12