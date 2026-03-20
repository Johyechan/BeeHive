using InGame.MyManager.Global;
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

        private int _goldBarMaxCount; // 금괴 최대 개수

        private Color _originColor; // 기본 색

        // 생성자
        public WalletUIHandle(TMP_Text goldCoinTmpText, TMP_Text goldBarTmpText, int goldBarMaxCount, Color originColor)
        {
            _goldCoinTmpText = goldCoinTmpText;
            _goldBarTmpText = goldBarTmpText;
            _goldBarMaxCount = goldBarMaxCount;
            _originColor = originColor;
        }

        // 금화 및 금괴 개수에 따라 UI 변경 함수
        public void SetUI(int goldCoinCount, int goldBarCount)
        {
            _goldCoinTmpText.text = $"x {goldCoinCount}"; // 금화 개수를 UI로 표기
            _goldBarTmpText.text = $"x {goldBarCount}"; // 금괴 개수를 UI로 표기
            if(goldBarCount >= _goldBarMaxCount) // 금괴 수가 최대 개수 이상이라면
            {
                _goldBarTmpText.color = Color.red; // 빨간색으로 변경
            }
            else // 금괴 수가 최대 개수 미만이라면
            {
                _goldBarTmpText.color = _originColor; // 기본 색상으로 변경
            }
        }
    }
}
// 마지막 작성 일자: 2026.02.25