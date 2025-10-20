using InGame.MyEnum;
using InGame.MyObject;
using MyUtil;
using System.Collections.Generic;
using System.Linq;
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

        private bool _canMakePiece; // 기물 생성 가능 여부
        public bool CanMakePiece // 위 변수 프로퍼티
        {
            get => _canMakePiece;
            set => _canMakePiece = value;
        }

        private Dictionary<ObjectType, bool> _pieceCanMoveMap = new Dictionary<ObjectType, bool>(); // 각 기물마다 이동 가능 여부를 가지는 맵
        public Dictionary<ObjectType, bool> PieceCanMoveMap { get => _pieceCanMoveMap; }

        private Castle _myCastle; // 플레이어 성
        public Castle MyCastle { get => _myCastle; set => _myCastle = value; }

        protected override void Awake()
        {
            base.Awake();

            // 변수 초기화
            _currentMovePiece = null;
            SetTeamOrder(PlayerCount);

            // 초기화
            _canMakePiece = true; // 생성 가능 여부
            _pieceCanMoveMap.Add(ObjectType.Miner, true); // 광부 기물 이동 가능 여부
            _pieceCanMoveMap.Add(ObjectType.Soldier, true); // 보병 기물 이동 가능 여부
            _pieceCanMoveMap.Add(ObjectType.Tank, true); // 전차 기물 이동 가능 여부

            switch(TeamManager.Instance.CurrentTeamType)
            {
                case TeamType.Team1: // 팀1 일 때 팀1 성 반환
                    MyCastle = GameObject.Find("Team1Castle").GetComponent<Castle>();
                    break;
                case TeamType.Team2: // 팀2 일 때 팀2 성 반환
                    MyCastle = GameObject.Find("Team2Castle").GetComponent<Castle>();
                    break;
                case TeamType.Team3: // 팀3 일 때 팀3 성 반환
                    MyCastle = GameObject.Find("Team3Castle").GetComponent<Castle>();
                    break;
            }
        }

        // 기물 생성 및 이동 가능 여부 초기화
        private void TurnStart()
        {
            _canMakePiece = true;
            var keys = _pieceCanMoveMap.Keys.ToList();
            for(int i = 0; i < keys.Count; i++)
            {
                _pieceCanMoveMap[keys[i]] = true;
            }
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
                count++; // 순서 증가
                if (currentTeamType == type) // 현재 팀과 일치하는 팀을 찾았다면
                {
                    TurnStart();
                    if(count >= _teamOrder.Count) // 마지막 팀 순서와 현재 센 순서가 같다면
                    {
                        return _teamOrder[0]; // 첫 팀을 반환
                    }
                    else // 아니라면
                    {
                        return _teamOrder[count]; // 현재 센 순서의 팀 반환
                    }
                }
            }

            return TeamType.None; // None을 반환하면 현재 팀이 존재하지 않는 것
        }
    }
}
// 마지막 작성 일자: 2025.10.01
