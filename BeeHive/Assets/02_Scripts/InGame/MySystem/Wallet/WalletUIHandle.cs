using InGame.MyEnum;
using InGame.MyManager.Global;
using TMPro;
using UnityEngine;

namespace InGame.MySystem
{
    // 작성자: 조혜찬
    // 지갑의 UI를 변경하는 핸들러 클래스
    public class WalletUIHandle
    {
        private TMP_Text _team1GoldCoinText; // 팀 1 금화 개수 텍스트
        private TMP_Text _team1GoldBarText; // 팀 1 금괴 개수 텍스트
        private TMP_Text _team2GoldCoinText; // 팀 2 금화 개수 텍스트
        private TMP_Text _team2GoldBarText; // 팀 2 금괴 개수 텍스트

        private int _goldBarMaxCount; // 금괴 최대 개수

        private Color _team1OriginalColor; // 기본 색
        private Color _team2OriginalColor; // 기본 색

        // 생성자
        public WalletUIHandle(TMP_Text team1GoldCoinText, TMP_Text team1GoldBarText, TMP_Text team2GoldCoinText, TMP_Text team2GoldBarText, int goldBarMaxCount, Color team1OriginalColor, Color team2OriginalColor)
        {
            _team1GoldCoinText = team1GoldCoinText;
            _team1GoldBarText = team1GoldBarText;
            _team2GoldCoinText = team2GoldCoinText;
            _team2GoldBarText = team2GoldBarText;

            _goldBarMaxCount = goldBarMaxCount;

            _team1OriginalColor = team1OriginalColor;
            _team2OriginalColor = team2OriginalColor;
        }

        // 금화 및 금괴 개수에 따라 UI 변경 함수
        public void SetUI(int goldCoinCount, int goldBarCount)
        {
            switch(TeamManager.Instance.CurrentTeamType)
            {
                case TeamType.Team1:
                    ChangeText(_team1GoldCoinText, _team1GoldBarText, goldCoinCount, goldBarCount, _team1OriginalColor);
                    break;
                case TeamType.Team2:
                    ChangeText(_team2GoldCoinText, _team2GoldBarText, goldCoinCount, goldBarCount, _team2OriginalColor);
                    break;
            }
            
        }

        private void ChangeText(TMP_Text goldCoinText, TMP_Text goldBarText, int goldCoinCount, int goldBarCount, Color originalColor)
        {
            goldCoinText.text = $"x {goldCoinCount}"; // 금화 개수를 UI로 표기
            goldBarText.text = $"x {goldBarCount}"; // 금괴 개수를 UI로 표기
            if (goldBarCount >= _goldBarMaxCount) // 금괴 수가 최대 개수 이상이라면
            {
                goldBarText.color = Color.yellow; // 노란색으로 변경
            }
            else // 금괴 수가 최대 개수 미만이라면
            {
                goldBarText.color = originalColor; // 기본 색상으로 변경
            }
        }
    }
}
// 마지막 작성 일자: 2026.05.15