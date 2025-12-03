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

        // 초기화 함수
        public void Init()
        {
            _usedDeckUIData.castleUpgradeCardCount.text = "x 0";
            _usedDeckUIData.roadChangeCardCount.text = "x 0";
            _usedDeckUIData.goodHarvestCardCount.text = "x 0";
            _usedDeckUIData.droughtCardCount.text = "x 0";
            _usedDeckUIData.firePowerCardCount.text = "x 0";
        }
    }
}
// 마지막 작성 일자: 2025.12.03