using InGame.MyEvent;
using InGame.MyManager;
using System.Threading.Tasks;
using UnityEngine;

namespace InGame.MySystem.Game
{
    // 작성자: 조혜찬
    // 생산 턴에 필요한 객체를 추가하는 기능들을 관리하는 클래스
    public class MakeTurnAddSystem
    {
        // 초기화 함수
        public void Init()
        {
            TurnEvents.OnMakeTurn.Add(GetGoldBar); // 생산 이벤트에 금괴 획득 함수 큐에 추가
            TurnEvents.OnMakeTurn.Add(GetRoad); // 생산 이벤트에 도로 추가 함수 큐에 추가
        }

        // 금괴 획득 함수
        private async Task GetGoldBar()
        {
            if (IsReturn()) return; // 반환해야할 조건을 충족했을 경우 반환

            WalletEvent.OnGetGoldBar?.Invoke(2); // 금괴 2개 획득

            await Task.CompletedTask; // Task 완료 반환
        }

        private async Task GetRoad()
        {
            if (IsReturn()) return; // 반환해야할 조건을 충족했을 경우 반환

            PieceEvents.OnGetRoad?.Invoke(2); // 도로 2개 획득

            await Task.CompletedTask; // Task 완료 반환
        }

        // 반환 여부 확인 함수
        private bool IsReturn()
        {
            if (TurnManager.Instance.CurrentTeamType != TeamManager.Instance.CurrentTeamType) // 현재 턴의 팀과 내 팀이 다르다면
                return true; // 반환

            return false;
        }
    }
}
// 마지막 작성 일자: 2025.09.05