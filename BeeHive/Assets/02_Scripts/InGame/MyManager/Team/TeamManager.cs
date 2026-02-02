using MyUtil;
using InGame.MyEnum;
using UnityEngine;
using InGame.MyObject;
using System.Threading.Tasks;

namespace InGame.MyManager
{
    // 작성자: 조혜찬
    // 서버에서 팀을 배정 받기 위한 싱글톤 클래스
    public class TeamManager : MonoSingleton<TeamManager>
    {
        private TeamType _currentTeamType; // 현재 팀 타입
        // 위에 변수 프로퍼티
        public TeamType CurrentTeamType { get => _currentTeamType; set => _currentTeamType = value; }

        private TaskCompletionSource<bool> _teamSetTcs; // 팀 세팅 대기 Task
        public TaskCompletionSource<bool> TeamSetTcs
        {
            get
            {
                if(_teamSetTcs == null) // 팀 세팅 대기 task가 비어 있다면
                {
                    _teamSetTcs = new TaskCompletionSource<bool>(); // 새로 생성
                }
                return _teamSetTcs;
            }
        }// 팀 세팅 대기 Task 프로퍼티

        private bool _firstTurn = true; // 팀의 첫 번째 턴 여부
        public bool FirstTurn { get => _firstTurn; set => _firstTurn = value; } // 팀의 첫 번째 턴 여부 프로퍼티

        protected override void Awake()
        {
            base.Awake();

            NetworkManager.Instance.Socket.Emit("debug", $"TeamManager Instance Awake: {gameObject.GetInstanceID()}");

            var socket = NetworkManager.Instance.Socket; // 서버와 통신하기 위한 객체 받아오기

            if(socket != null) // 서버와 통신하기 위한 객체가 null이 아닐 때
            {
                socket.On("teamType", value =>
                {
                    if (NetworkManager.Instance.IsClientOver) // 클라이언트가 종료 되었다면
                        return; // 반환

                    int teamType = value.GetValue<int>(); // int 형으로 전달 받은 값 저장
                    socket.Emit("debug", $"TeamManager 이벤트 받아서 형변환 전 현재 팀 타입: {teamType}");
                    _currentTeamType = (TeamType)teamType; // 팀 저장
                    socket.Emit("debug", $"TeamManager 이벤트 받아서 저장하는 현재 팀 타입: {_currentTeamType}");
                    _teamSetTcs?.TrySetResult(true); // 팀 세팅 완료
                });
            }
        }

        private void OnDisable()
        {
            NetworkManager.Instance.Socket.Emit("debug", "TeamManager 비활성화 불림");
            NetworkManager.Instance.Socket.Emit("debug", $"TeamManager Instance Disable: {gameObject.GetInstanceID()}");
            NetworkManager.Instance.Socket.Off("teamType");
        }

        // 팀에 맞는 성을 반환하는 함수
        public Castle GetCastle(TeamType teamType)
        {
            switch(teamType)
            {
                case TeamType.Team1: // 팀1 일 때 팀1 성 반환
                    return GameManager.Instance.MyCastle = GameObject.Find("Team1Castle").GetComponent<Castle>();
                case TeamType.Team2: // 팀2 일 때 팀2 성 반환
                    return GameManager.Instance.MyCastle = GameObject.Find("Team2Castle").GetComponent<Castle>();
                case TeamType.Team3: // 팀3 일 때 팀3 성 반환
                    return GameManager.Instance.MyCastle = GameObject.Find("Team3Castle").GetComponent<Castle>();
                default:
                    return null;
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
// 마지막 작성 일자: 2026.01.30