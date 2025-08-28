using InGame.MyEnum;
using InGame.MyManager;
using UnityEngine;

namespace InGame.MySystem.Game
{
    // 작성자: 조혜찬
    // 금화 및 금괴 개수를 변경 이벤트 처리 핸들러

    public class GoldSetHandle
    {
        private PlayerData[] _players; // 현재 방에 존재하는 플레이어들을 저장한 배열

        private Wallet _wallet; // 지갑 클래스 변수

        private Transform _team1GoldCoinParent; // 팀1 금화 객체 부모
        private Transform _team1GoldBarParent; // 팀1 금괴 객체 부모

        private Transform _team2GoldCoinParent; // 팀2 금화 객체 부모
        private Transform _team2GoldBarParent; // 팀2 금괴 객체 부모

        private Transform _team3GoldCoinParent; // 팀3 금화 객체 부모
        private Transform _team3GoldBarParent; // 팀3 금괴 객체 부모

        public GoldSetHandle(PlayerData[] players, Wallet wallet)
        {
            _players = players;

            _wallet = wallet;

            _team1GoldCoinParent = GameObject.Find("Player1GoldCoins").transform; // 팀1 금화 객체 부모 초기화
            _team1GoldBarParent = GameObject.Find("Player1GoldBars").transform; // 팀1 금괴 객체 부모 초기화

            _team2GoldCoinParent = GameObject.Find("Player2GoldCoins").transform; // 팀2 금화 객체 부모 초기화
            _team2GoldBarParent = GameObject.Find("Player2GoldBars").transform; // 팀2 금괴 객체 부모 초기화

            if (GameManager.Instance.PlayerCount == 3) // 플레이어 수가 3명이라면
            {
                _team3GoldCoinParent = GameObject.Find("Player3GoldCoins").transform; // 팀3 금화 객체 부모 초기화
                _team3GoldBarParent = GameObject.Find("Player3GoldBars").transform; // 팀3 금괴 객체 부모 초기화
            }
        }

        public void Setting()
        {
            for (int i = 0; i < _players.Length; i++) // 플레이어 순회
            {
                TeamType type = (TeamType)_players[i].team; // 플레이어의 팀 구하기
                switch(type)
                {
                    case TeamType.Team1:
                        Debug.Log("팀1");
                        _wallet.WalletObjectHandle.SetObject(_team1GoldCoinParent, _team1GoldBarParent, _players[i].goldCoin, _players[i].goldBar); // 금화 및 금괴 객체 개수 세팅
                        break;
                    case TeamType.Team2:
                        Debug.Log("팀2");
                        _wallet.WalletObjectHandle.SetObject(_team2GoldCoinParent, _team2GoldBarParent, _players[i].goldCoin, _players[i].goldBar); // 금화 및 금괴 객체 개수 세팅
                        break;
                    case TeamType.Team3:
                        _wallet.WalletObjectHandle.SetObject(_team3GoldCoinParent, _team3GoldBarParent, _players[i].goldCoin, _players[i].goldBar); // 금화 및 금괴 객체 개수 세팅
                        break;
                }
            }
        }
    }
}
// 마지막 작성 일자: 2025.08.21