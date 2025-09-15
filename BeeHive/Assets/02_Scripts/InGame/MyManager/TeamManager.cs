using MyUtil;
using InGame.MyEnum;
using UnityEngine;

namespace InGame.MyManager
{
    // 작성자: 조혜찬
    // 서버에서 팀을 배정 받기 위한 싱글톤 클래스
    public class TeamManager : MonoSingleton<TeamManager>
    {
        private TeamType _currentTeamType; // 현재 팀 타입
        // 위에 변수 프로퍼티
        public TeamType CurrentTeamType { get => _currentTeamType; set => _currentTeamType = value; }

        protected override void Awake()
        {
            base.Awake();

            var socket = NetworkManager.Instance.Socket; // 서버와 통신하기 위한 객체 받아오기

            if(socket != null) // 서버와 통신하기 위한 객체가 null이 아닐 때
            {
                socket.On("teamType", value =>
                {
                    int teamType = value.GetValue<int>(); // int 형으로 전달 받은 값 저장
                    _currentTeamType = (TeamType)teamType; // 팀 저장
                });
            }
        }

        public Transform GetMinerTransform(TeamType type)
        {
            switch(type)
            {
                case TeamType.Team1:
                    return GameObject.Find("Player1Miners").transform;
                case TeamType.Team2:
                    return GameObject.Find("Player2Miners").transform;
                case TeamType.Team3:
                    return GameObject.Find("Player3Miners").transform;
                default:
                    return null;
            }
        }

        public Transform GetSoldierTransform(TeamType type)
        {
            switch (type)
            {
                case TeamType.Team1:
                    return GameObject.Find("Player1Soldiers").transform;
                case TeamType.Team2:
                    return GameObject.Find("Player2Soldiers").transform;
                case TeamType.Team3:
                    return GameObject.Find("Player3Soldiers").transform;
                default:
                    return null;
            }
        }

        public Transform GetTankTransform(TeamType type)
        {
            switch (type)
            {
                case TeamType.Team1:
                    return GameObject.Find("Player1Tanks").transform;
                case TeamType.Team2:
                    return GameObject.Find("Player2Tanks").transform;
                case TeamType.Team3:
                    return GameObject.Find("Player3Tanks").transform;
                default:
                    return null;
            }
        }

        public Transform GetRoadTransform(TeamType type)
        {
            switch (type)
            {
                case TeamType.Team1:
                    return GameObject.Find("Player1Road").transform;
                case TeamType.Team2:
                    return GameObject.Find("Player2Road").transform;
                case TeamType.Team3:
                    return GameObject.Find("Player3Road").transform;
                default:
                    return null;
            }
        }

        public Transform GetGoldCoinTransform(TeamType type)
        {
            switch (type)
            {
                case TeamType.Team1:
                    return GameObject.Find("Player1GoldCoins").transform;
                case TeamType.Team2:
                    return GameObject.Find("Player2GoldCoins").transform;
                case TeamType.Team3:
                    return GameObject.Find("Player3GoldCoins").transform;
                default:
                    return null;
            }
        }

        public Transform GetGoldBarTransform(TeamType type)
        {
            switch (type)
            {
                case TeamType.Team1:
                    return GameObject.Find("Player1GoldBars").transform;
                case TeamType.Team2:
                    return GameObject.Find("Player2GoldBars").transform;
                case TeamType.Team3:
                    return GameObject.Find("Player3GoldBars").transform;
                default:
                    return null;
            }
        }
    }
}
// 마지막 작성 일자: 2025.08.19