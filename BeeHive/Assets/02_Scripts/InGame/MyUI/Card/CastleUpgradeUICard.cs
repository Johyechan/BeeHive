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
            // 성 체력 1증가
            GameManager.Instance.MyCastle.HP++; // 최대 체력 1 증가
            NetworkManager.Instance.Socket.Emit("castleHpUp", (int)TeamManager.Instance.CurrentTeamType); // 서버에 최대 체력이 올라간 팀 타입 알려주기
        }
    }
}
// 마지막 작성 일자: 2025.10.01