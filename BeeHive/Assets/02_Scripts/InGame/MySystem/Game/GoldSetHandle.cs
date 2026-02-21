using InGame.MyEnum;
using InGame.MyManager;
using InGame.MyManager.Local;
using System.Threading.Tasks;
using UnityEngine;

namespace InGame.MySystem.Game
{
    // 작성자: 조혜찬
    // 금화 및 금괴 개수를 변경 이벤트 처리 핸들러

    public class GoldSetHandle
    {
        private Wallet _wallet; // 지갑 클래스 변수

        private Transform _team1GoldCoinParent; // 팀1 금화 객체 부모
        private Transform _team1GoldBarParent; // 팀1 금괴 객체 부모

        private Transform _team2GoldCoinParent; // 팀2 금화 객체 부모
        private Transform _team2GoldBarParent; // 팀2 금괴 객체 부모

        private Transform _team3GoldCoinParent; // 팀3 금화 객체 부모
        private Transform _team3GoldBarParent; // 팀3 금괴 객체 부모

        public GoldSetHandle(Wallet wallet)
        {
            _wallet = wallet;

            _team1GoldCoinParent = GameObject.Find("Player1GoldCoins").transform; // 팀1 금화 객체 부모 초기화
            _team1GoldBarParent = GameObject.Find("Player1GoldBars").transform; // 팀1 금괴 객체 부모 초기화

            _team2GoldCoinParent = GameObject.Find("Player2GoldCoins").transform; // 팀2 금화 객체 부모 초기화
            _team2GoldBarParent = GameObject.Find("Player2GoldBars").transform; // 팀2 금괴 객체 부모 초기화

            if (InGameContext.Current.Data.GameManager.PlayerCount == 3) // 플레이어 수가 3명이라면
            {
                _team3GoldCoinParent = GameObject.Find("Player3GoldCoins").transform; // 팀3 금화 객체 부모 초기화
                _team3GoldBarParent = GameObject.Find("Player3GoldBars").transform; // 팀3 금괴 객체 부모 초기화
            }
        }

        public void Setting(int team, int goldCoin, int goldBar)
        {
            TeamType type = (TeamType)team; // 팀 구하기
            switch (type)
            {
                case TeamType.Team1:
                    _wallet.WalletObjectHandle.SetObject(_team1GoldCoinParent, _team1GoldBarParent, goldCoin, goldBar, type); // 금화 및 금괴 객체 개수 세팅
                    break;
                case TeamType.Team2:
                    _wallet.WalletObjectHandle.SetObject(_team2GoldCoinParent, _team2GoldBarParent, goldCoin, goldBar, type); // 금화 및 금괴 객체 개수 세팅
                    break;
                case TeamType.Team3:
                    _wallet.WalletObjectHandle.SetObject(_team3GoldCoinParent, _team3GoldBarParent, goldCoin, goldBar, type); // 금화 및 금괴 객체 개수 세팅
                    break;
            }
        }
    }
}
// 마지막 작성 일자: 2026.02.21