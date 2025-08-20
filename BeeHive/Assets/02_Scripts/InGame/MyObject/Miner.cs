using InGame.MyEnum;
using InGame.MyEvent;
using InGame.MyManager;
using InGame.MyManager.MyPlacePlane;
using UnityEngine;

namespace InGame.MyObject
{
    // 작성자: 조혜찬
    // 광부 기물 클래스
    public class Miner : PieceBase
    {
        private void Awake()
        {
            ParentSet();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            MakeTurnEvent.OnMakeTurn += Dig; // 생산 턴에 광부가 금화를 얻는 기능 구독
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            MakeTurnEvent.OnMakeTurn -= Dig; // 생산 턴에 광부가 금화를 얻는 기능 구독 해제
        }

        // 금화를 얻는 함수
        private void Dig()
        {
            if (TurnManager.Instance.CurrentTeamType != TeamManager.Instance.CurrentTeamType) // 현재 턴 팀과 나의 팀이 다르다면
                return; // 반환

            if (_currentPlacePlane == null) // 자기가 배치된 판이 없다면
                return; // 반환

            switch(TeamManager.Instance.CurrentTeamType)
            {
                case TeamType.Team1: // 플레이어의 팀이 Team1일 경우
                    WalletEvent.OnGetGoldCoin?.Invoke(_currentPlacePlane.team1GoldCoin); // 현재 칸에서 team1이 얻는 금화만큼 얻기
                    break;
                case TeamType.Team2: // 플레이어의 팀이 Team2일 경우
                    WalletEvent.OnGetGoldCoin?.Invoke(_currentPlacePlane.team2GoldCoin); // 현재 칸에서 team2가 얻는 금화만큼 얻기
                    break;
                case TeamType.Team3: // 플레이어의 팀이 Team3일 경우
                    WalletEvent.OnGetGoldCoin?.Invoke(_currentPlacePlane.team3GoldCoin); // 현재 칸에서 team3이 얻는 금화만큼 얻기
                    break;
            }
        }

        // 부모 초기화 함수
        private void ParentSet()
        {
            _parent = GameObject.Find(TeamManager.Instance.MinerParentName).transform; // 보병 객체의 부모 할당
        }
    }
}
// 마지막 작성 일자: 2025.08.20