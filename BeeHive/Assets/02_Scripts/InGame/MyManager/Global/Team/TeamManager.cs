using InGame.MyEnum;
using InGame.MyManager.Local;
using InGame.MyObject;
using MyUtil;
using MyUtil.GameMode;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace InGame.MyManager.Global
{
    // 작성자: 조혜찬
    // 서버에서 팀을 배정 받기 위한 싱글톤 클래스
    public class TeamManager : MonoSingleton<TeamManager>
    {
        private readonly object _teamSettingLock = new(); // 팀 세팅에 사용할 락 키

        private TeamType _currentTeamType; // 현재 팀 타입
        // 위에 변수 프로퍼티
        public TeamType CurrentTeamType { get => _currentTeamType; set => _currentTeamType = value; }

        private TaskCompletionSource<bool> _teamSetTcs; // 팀 세팅 대기 Task

        private bool _team2FirstTurn = true; // 블루팀(팀2)의 첫 번째 턴 여부
        public bool Team2FirstTurn { get => _team2FirstTurn; set => _team2FirstTurn = value; } // 블루팀(팀2)의 첫 번째 턴 여부 프로퍼티

        protected override void Awake()
        {
            base.Awake();

            var socket = NetworkManager.Instance.Socket; // 서버와 통신하기 위한 객체 받아오기

            if(socket != null) // 서버와 통신하기 위한 객체가 null이 아닐 때
            {
                socket.On("teamType", value =>
                {
                    if (NetworkManager.Instance.IsClientOver) // 클라이언트가 종료 되었다면
                        return; // 반환

                    if (GameModeManager.Instance.CurrentGameMode.UseServer()) // 게임 서버를 사용하는 경우
                    {
                        int teamType = value.GetValue<int>(); // int 형으로 전달 받은 값 저장
                        _currentTeamType = (TeamType)teamType; // 팀 저장
                        TeamSetComplete(); // 팀 세팅 완료
                    }
                });
            }

            Ready();
        }

        private void OnDisable()
        {
            NetworkManager.Instance.Socket.Off("teamType");
        }

        // 팀 세팅 대기 tcs null 체크 후 생성 반환 또는 그냥 반환하는 함수
        public Task WaitTeamSetTcsAsync()
        {
            lock(_teamSettingLock)
            {
                _teamSetTcs ??= new TaskCompletionSource<bool>(); // 만약 팀 세팅 대기 tcs가 null이라면 새로 할당
                return _teamSetTcs.Task; // 반환
            }
        }

        // 팀 세팅 완료 함수
        private void TeamSetComplete()
        {
            TaskCompletionSource<bool> tcs;

            lock(_teamSettingLock)
            {
                _teamSetTcs ??= new TaskCompletionSource<bool>(); // 팀 대기 tcs가 null이라면 새로 할당
                tcs = _teamSetTcs; // 지역 변수에 팀 대기 tcs 할당
            }

            tcs.SetResult(true); // 팀 세팅 대기 완료
        }

        // 팀 세팅 대기 tcs 초기화 함수
        public void ResetTeamSetTcs()
        {
            _teamSetTcs = null;
        }

        // 팀에 맞는 성을 반환하는 함수
        public Castle GetCastle(TeamType teamType)
        {
            switch(teamType)
            {
                case TeamType.Team1: // 팀1 일 때 팀1 성 반환
                    return GameObject.Find("Team1Castle").GetComponent<Castle>();
                case TeamType.Team2: // 팀2 일 때 팀2 성 반환
                    return GameObject.Find("Team2Castle").GetComponent<Castle>();
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
                default:
                    return null;
            }
        }
    }
}
// 마지막 작성 일자: 2026.06.19