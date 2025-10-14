using InGame.MyEnum;
using InGame.MyManager;
using UnityEngine;

namespace InGame.MyUI.Card
{
    // 작성자: 조혜찬
    // 가뭄 카드
    public class DroughtUICard : UICardBase
    {
        // 카드 기능을 실제로 수행하는 함수
        public override void UseCard()
        {
            NetworkManager.Instance.Socket.Emit("debug", "가뭄");
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

                string json = JsonUtility.ToJson(droughtInfo);
                NetworkManager.Instance.Socket.Emit("makeDrought", json); // 서버에게 1을 보냄으로써 가뭄이 활성화 되었다고 전송

                base.UseCard();
            }
        }
    }
}
// 마지막 작성 일자: 2025.10.14