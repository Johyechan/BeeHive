using InGame.MyEnum;
using InGame.MyManager;
using InGame.MyManager.Global;
using InGame.MyManager.Local;
using InGame.MyObject;
using MyUtil.GameMode;
using System.Threading.Tasks;
using UnityEngine;

namespace InGame.MyUI.Card
{
    // 작성자: 조혜찬
    // 성벽 강화 카드
    public class CastleUpgradeUICard : UICardBase
    {
        // 카드 기능을 실제로 수행하는 함수
        public override async Task<bool> UseCard()
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

            InGameContext.Current.Data.CardManager.UsedCardShowOver = new TaskCompletionSource<bool>(); // 사용한 카드 보여주기 끝날 때까지 대기할 tcs 발급
            string json = JsonUtility.ToJson(usedCardData); // Json 형태로 변환
            if (GameModeManager.Instance.CurrentGameMode.UseServer())
                NetworkManager.Instance.Socket.Emit("usedCard", json); // 서버로 카드를 사용했다고 전송


            await InGameContext.Current.Data.CardManager.UsedCardShowOver?.Task; // tcs 대기

            Castle currentCastle = TeamManager.Instance.GetCastle(TeamManager.Instance.CurrentTeamType); // 현재 팀의 성 가져오기
            currentCastle.CastleUpgrade(); // 성 체력 1증가

            CastleHpUpInfo castleHpUpInfo = new CastleHpUpInfo()
            {
                roomID = SceneMgr.Instance.CurrentRoomID, // 현재 방 ID
                changeTeamType = (int)TeamManager.Instance.CurrentTeamType, // 체력이 바뀔 성의 팀 타입
                changedHp = currentCastle.CurrentHp, // 바뀐 체력
            };
            string castleJson = JsonUtility.ToJson(castleHpUpInfo); // Json화
            if (GameModeManager.Instance.CurrentGameMode.UseServer())
                NetworkManager.Instance.Socket.Emit("castleHpUp", castleJson); // 서버에 최대 체력이 올라간 팀 타입 알려주기

            bool result = await base.UseCard();

            return result;
        }
    }
}
// 마지막 작성 일자: 2026.06.19