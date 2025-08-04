using UnityEngine;
using MyUtil;
using TMPro;
using InGame.MyEnum;

namespace InGame.MyManager
{
    // 작성자: 조혜찬
    // 서버에서 팀을 배정 받기 위한 싱글톤 클래스
    public class TeamManager : MonoSingleton<TeamManager>
    {
        private TeamType _currentTeamType; // 현재 팀 타입
        // 위에 변수 프로퍼티
        public TeamType currentTeamType { get => _currentTeamType; set => _currentTeamType = value; }

        private int _maxTeam; // 최대 팀 수
        private int _currentTeam = 1; // 현재 팀

        private void Start()
        {
            _maxTeam = GameManager.Instance.PlayerCount; // 최대 플레이어 수를 최대 팀 수로 저장
        }

        public void SetTeam()
        {
            _currentTeamType = (TeamType)_currentTeam; // 현재 팀 값 저장

            _currentTeam++; // 현재 팀 증가
            if (_currentTeam > _maxTeam) // 현재 팀이 최대 팀보다 크다면
            {
                _currentTeam = 1; // 현재 팀 초기화
            }
        }
    }
}
// 마지막 작성 일자: 2025.07.29