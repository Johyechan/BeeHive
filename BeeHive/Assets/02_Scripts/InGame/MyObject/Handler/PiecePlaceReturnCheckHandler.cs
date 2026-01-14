using InGame.MyEvent;
using System.Threading.Tasks;
using UnityEngine;

namespace InGame.MyObject.Handler
{
    // 작성자: 조혜찬
    // 기물 배치 가능 여부를 체크하는 핸들러
    public class PiecePlaceReturnCheckHandler
    {
        private PiecePlacePlaneObject _piecePlacePlaneObject;

        public PiecePlaceReturnCheckHandler(PiecePlacePlaneObject piecePlacePlaneObject)
        {
            _piecePlacePlaneObject = piecePlacePlaneObject;
        }

        public async Task<bool> IsReturn(int leftPieceCount, int cost)
        {
            if (!await WarningEvent.OnCheckCurrentTurnTeam()) // 현재 턴이 자신의 턴이 아닐 경우
            {
                _piecePlacePlaneObject.HighLightOffEvent(); // 하이라이트 끄기
                return true; // 반환
            }

            if (!await WarningEvent.OnCheckLeftPieceCount(leftPieceCount, "남은 기물이 없어 배치할 수 없습니다")) // 남은 도로가 없다면
            {
                _piecePlacePlaneObject.HighLightOffEvent(); // 하이라이트 끄기
                return true; // 반환
            }

            if (!await WarningEvent.OnCanPayCost.Invoke(cost, "금괴가 부족하여 기물을 배치할 수 없습니다.")) // 비용을 지불할 수 없다면
            {
                _piecePlacePlaneObject.HighLightOffEvent(); // 하이라이트 끄기
                return true; // 반환
            }

            return false;
        }
    }
}
// 마지막 작성 일자: 2026.01.14