using TMPro;
using UnityEngine;

namespace InGame.MySystem
{
    // 작성자: 조혜찬
    // 지갑의 UI를 변경하는 핸들러 클래스
    public class WalletUIHandle
    {
        private TMP_Text _goldCoinTmpText; // 금화 UI
        private TMP_Text _goldBarTmpText; // 금괴 UI

        // 생성자
        public WalletUIHandle(TMP_Text goldCoinTmpText, TMP_Text goldBarTmpText)
        {
            _goldCoinTmpText = goldCoinTmpText;
            _goldBarTmpText = goldBarTmpText;
        }

        // 금화 및 금괴 개수에 따라 UI 변경 함수
        public void SetUI(int goldCoinCount, int goldBarCount)
        {
            _goldCoinTmpText.text = $"금화 X {goldCoinCount}"; // 금화 개수를 UI로 표기
            _goldBarTmpText.text = $"금괴 X {goldBarCount}"; // 금괴 개수를 UI로 표기
        }
    }
}
// 마지막 작성 일자: 2025.08.20