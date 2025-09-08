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

        private string _minerParentName; // 광부 객체의 부모 객체 이름
        // 위 변수 프로퍼티
        public string MinerParentName { get => _minerParentName; }

        private string _soldierParentName; // 보병 객체의 부모 객체 이름
        // 위 변수 프로퍼티
        public string SoldierParentName { get => _soldierParentName; }

        private string _tankParentName; // 전차 객체의 부모 객체 이름
        // 위 변수 프로퍼티
        public string TankParentName { get => _tankParentName; }

        private string _roadParentName; // 도로 객체의 부모 객체 이름
        // 위 변수 프로퍼티
        public string RoadParentName { get => _roadParentName; }

        private string _goldCoinParentName; // 금화 객체의 부모 객체 이름
        // 위 변수 프로퍼티
        public string GoldCoinParentName { get => _goldCoinParentName; }

        private string _goldBarParentName; // 금괴 객체의 부모 객체 이름
        // 위 변수 프로퍼티
        public string GoldBarParentName { get => _goldBarParentName; }

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
                    switch (_currentTeamType) // 현재 타입에 따라
                    {
                        case TeamType.Team1: // 팀1일 경우
                            _minerParentName = "Player1Miners";
                            _soldierParentName = "Player1Soldiers";
                            _tankParentName = "Player1Tanks";
                            _roadParentName = "Player1Road";
                            _goldCoinParentName = "Player1GoldCoins";
                            _goldBarParentName = "Player1GoldBars";
                            break;
                        case TeamType.Team2:// 팀2일 경우
                            _minerParentName = "Player2Miners";
                            _soldierParentName = "Player2Soldiers";
                            _tankParentName = "Player2Tanks";
                            _roadParentName = "Player2Road";
                            _goldCoinParentName = "Player2GoldCoins";
                            _goldBarParentName = "Player2GoldBars";
                            break;
                        case TeamType.Team3:// 팀3일 경우
                            _minerParentName = "Player3Miners";
                            _soldierParentName = "Player3Soldiers";
                            _tankParentName = "Player3Tanks";
                            _roadParentName = "Player3Road";
                            _goldCoinParentName = "Player3GoldCoins";
                            _goldBarParentName = "Player3GoldBars";
                            break;
                    }
                });
            }
        }
    }
}
// 마지막 작성 일자: 2025.08.19