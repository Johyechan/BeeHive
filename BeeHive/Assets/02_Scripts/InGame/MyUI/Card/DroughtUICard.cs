using InGame.MyEnum;
using InGame.MyManager;
using InGame.MyManager.Global;
using InGame.MyManager.Local;
using MyUtil.GameMode;
using UnityEngine;

namespace InGame.MyUI.Card
{
    // 작성자: 조혜찬
    // 가뭄 카드
    public class DroughtUICard : UICardBase
    {
        // 카드 기능을 실제로 수행하는 함수
        public override bool UseCard()
        {
            if (InGameContext.Current.Data.CardManager.CheckSameTypeCardWasUsed(CardType.Drought)) // 가뭄 카드 일전에 사용 했었는지 확인
            {
                return false;
            }

            UsedCardData usedCardData = new UsedCardData()
            {
                roomID = SceneMgr.Instance.CurrentRoomID, // 현재 방 ID
                usedCardType = (int)_uiCardData.poolType, // 사용한 카드의 이름
            };

            string json = JsonUtility.ToJson(usedCardData); // Json 형태로 변환
            if (GameModeManager.Instance.CurrentGameMode.UseServer())
                NetworkManager.Instance.Socket.Emit("usedCard", json); // 서버로 카드를 사용했다고 전송

            // 상대 턴에 상대 광부 생산 불가(1턴)
            if (SceneMgr.Instance.IsTwoPlayerGame)
            {
                int target = TeamManager.Instance.CurrentTeamType == TeamType.Team1 ? 2 : 1; // 팀 1일 경우 팀 2를 할당, 팀 2일 경우 팀 1을 할당

                DroughtInfo droughtInfo = new DroughtInfo()
                {
                    roomID = SceneMgr.Instance.CurrentRoomID,
                    targetTeam = target,
                    isDrought = 1
                };

                string droughtJson = JsonUtility.ToJson(droughtInfo);
                if (GameModeManager.Instance.CurrentGameMode.UseServer())
                    NetworkManager.Instance.Socket.Emit("makeDrought", droughtJson); // 서버에게 1을 보냄으로써 가뭄이 활성화 되었다고 전송

                return base.UseCard();
            }

            return true;
        }
    }
}
// 마지막 작성 일자: 2026.03.19