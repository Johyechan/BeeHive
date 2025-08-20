using InGame.MyEnum;
using MyUtil;
using System.Collections.Generic;
using UnityEngine;

namespace InGame.MyManager
{
    // 작성자: 조혜찬
    // 게임의 중요한 기능들을 관리하는 싱글톤 클래스
    public class GameManager : MonoSingleton<GameManager>
    {
        public int PlayerCount; // 현재 게임에 몇 명의 플레이어 있는지 정하는 변수

        private List<TeamType> _teamOrder = new List<TeamType>(); // 팀 순서 리스트

        private GameObject _currentMovePiece; // 현재 움직일 기물
        // 위에 변수를 외부에서 사용 및 변경하기 위한 프로퍼티
        public GameObject CurrentMovePiece
        {
            get => _currentMovePiece;
            set => _currentMovePiece = value;
        }

        private TeamType _teamType; // 현재 팀 타입
        // 위에 변수를 외부에서 사용 및 변경하기 위한 프로퍼티
        public TeamType TeamType
        {
            get => _teamType;
            set => _teamType = value;
        }
        
        protected override void Awake()
        {
            base.Awake();

            // 변수 초기화
            _currentMovePiece = null;
            SetTeamOrder(PlayerCount);
        }

        // 팀 순서를 저장하는 함수(최대 팀 수)
        private void SetTeamOrder(int maxTeamCount)
        {
            if(maxTeamCount == 2) // 2 라면
            {
                _teamOrder.Add(TeamType.Team1); // 팀1 추가
                _teamOrder.Add(TeamType.Team2); // 팀2 추가
            }
            else if(maxTeamCount == 3) // 3 이라면
            {
                _teamOrder.Add(TeamType.Team1); // 팀1 추가
                _teamOrder.Add(TeamType.Team2); // 팀2 추가
                _teamOrder.Add(TeamType.Team3); // 팀3 추가
            }
            else
            {
                Debug.Log($"{maxTeamCount} 수의 인원은 불가 합니다");
            }
        }

        // 다음 팀 순서를 반환하는 함수(현재 팀)
        public TeamType NextTeam(TeamType currentTeamType)
        {
            int count = 0; // 순서를 세는 변수

            foreach(TeamType type in _teamOrder) // 팀 순서 순회
            {
                if(currentTeamType == type) // 현재 팀과 일치하는 팀을 찾았다면
                {
                    if(count >= _teamOrder.Count - 1) // 마지막 팀 순서와 현재 센 순서가 같다면
                    {
                        return _teamOrder[0]; // 첫 팀을 반환
                    }
                    else // 아니라면
                    {
                        return _teamOrder[count]; // 현재 센 순서의 팀 반환
                    }
                }
                count++; // 순서 증가
            }

            return TeamType.None; // None을 반환하면 현재 팀이 존재하지 않는 것
        }
    }
}
// 마지막 작성 일자: 2025.08.20
