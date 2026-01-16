using InGame.MyEvent;
using System.Threading.Tasks;
using UnityEngine;

namespace InGame.MyObject.Handler
{
    // 작성자: 조혜찬
    // 도로 배치 가능 여부를 체크하는 핸들러
    public class RoadPlaceReturnCheckHandler
    {
        // 도로 배치가 불가능한지 확인하는 함수(남은 기물 개수, 가격)
        public bool IsReturn(int leftPieceCount, int cost)
        {
            if (!WarningEvent.OnCheckCurrentTurnTeam()) // 현재 턴의 팀을 확인해서 현재 턴이 내 턴이 아니라면
            {
                HighLightEvents.OnRoadPlacementHighLight?.Invoke(false); // 도로 칸 하이라이트를 끄는 매개변수로 이벤트 콜
                return true; // 반환
            }

            if (!WarningEvent.OnCheckLeftPieceCount(leftPieceCount, "남은 도로가 없어 배치할 수 없습니다")) // 남은 도로가 없다면
            {
                HighLightEvents.OnRoadPlacementHighLight?.Invoke(false); // 도로 칸 하이라이트를 끄는 매개변수로 이벤트 콜
                return true; // 반환
            }

            if (!WarningEvent.OnCanPayCost.Invoke(cost, "금괴가 부족하여 도로를 배치할 수 없습니다.")) // 비용을 지불할 수 없다면
            {
                HighLightEvents.OnRoadPlacementHighLight?.Invoke(false); // 도로 칸 하이라이트를 끄는 매개변수로 이벤트 콜
                return true; // 반환
            }

            return false;
        }
    }
}
// 마지막 작성 일자: 2026.01.16