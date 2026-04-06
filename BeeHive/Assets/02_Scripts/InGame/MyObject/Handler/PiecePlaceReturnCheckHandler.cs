using InGame.MyEvent;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization.Settings;

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

        public bool IsReturn(int leftPieceCount, int cost)
        {
            if (!WarningEvent.OnCheckCurrentTurnTeam()) // 현재 턴이 자신의 턴이 아닐 경우
            {
                _piecePlacePlaneObject.HighLightOffEvent(); // 하이라이트 끄기
                return true; // 반환
            }

            string noLeftPiece = LocalizationSettings.StringDatabase.GetLocalizedString(
                "Game",
                "Game_UI_NoLeftPiece"
            );

            if (!WarningEvent.OnCheckLeftPieceCount(leftPieceCount, noLeftPiece)) // 남은 도로가 없다면
            {
                _piecePlacePlaneObject.HighLightOffEvent(); // 하이라이트 끄기
                return true; // 반환
            }

            string noGold = LocalizationSettings.StringDatabase.GetLocalizedString(
                "Game",
                "Game_UI_NoGold"
            );

            if (!WarningEvent.OnCanPayCost.Invoke(cost, noGold)) // 비용을 지불할 수 없다면
            {
                _piecePlacePlaneObject.HighLightOffEvent(); // 하이라이트 끄기
                return true; // 반환
            }

            return false;
        }
    }
}
// 마지막 작성 일자: 2026.04.06