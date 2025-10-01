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
        public override void UseCard()
        {
            // 전차의 화력 +1
            CardManager.Instance.HaveFirePowerCard = true;
        }
    }
}
// 마지막 작성 일자: 2025.10.01