using UnityEngine;

namespace InGame.MyObject.Handler
{
    // 작성자: 조혜찬
    // 사용한 카드 UI 관련 기능 핸들러
    public class UsedDeckUIHandler
    {
        private UsedDeckUIData _usedDeckUIData;

        public UsedDeckUIHandler(UsedDeckUIData usedDeckUIData)
        {
            _usedDeckUIData = usedDeckUIData;
        }

        // 초기화 함수(성 강화 카드 개수 초기화가 필요한지 확인하는 매개변수)
        public void Init(bool needCastleUpgradeCardReset = true)
        {
            if(needCastleUpgradeCardReset)
                _usedDeckUIData.castleUpgradeCardCount.text = "x 0";

            _usedDeckUIData.roadChangeCardCount.text = "x 0";
            _usedDeckUIData.goodHarvestCardCount.text = "x 0";
            _usedDeckUIData.droughtCardCount.text = "x 0";
            _usedDeckUIData.firePowerCardCount.text = "x 0";
        }
    }
}
// 마지막 작성 일자: 2026.07.01