using InGame.MyEnum;
using InGame.MyManager;
using InGame.MyManager.Global;
using InGame.MyManager.Local;
using InGame.MyObject.Piece.ObjectPieces;
using MyUtil.GameMode;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace InGame.MyUI.Card
{
    // 작성자: 조혜찬
    // 도로 변형 카드
    public class RoadChangeUICard : UICardBase
    {
        // 카드 기능을 실제로 수행하는 함수
        public override async Task<bool> UseCard()
        {
            if (InGameContext.Current.Data.PieceManager.CanChangeRoadList.Count <= 0) // 도로 변형이 가능한 도로가 없다면
            {
                string canNotChangeRoad = LocalizationSettings.StringDatabase.GetLocalizedString(
                    "Game",
                    "Game_UI_CanNotChangeRoad"
                );

                UIManager.Instance.WarningUIMake(canNotChangeRoad);
                return false;
            }

            if (InGameContext.Current.Data.CardManager.CheckSameTypeCardWasUsed(CardType.RoadChange)) // 도로 변형 카드 일전에 사용 했었는지 확인
            {
                return false;
            }

            InGameContext.Current.Data.CardManager.CardUsed = true; // 카드 사용

            UsedCardData usedCardData = new UsedCardData()
            {
                roomID = SceneMgr.Instance.CurrentRoomID, // 현재 방 ID
                usedCardType = (int)_uiCardData.poolType, // 사용한 카드의 이름
            };

            InGameContext.Current.Data.CardManager.UsedCardShowOver = new TaskCompletionSource<bool>(); // 사용한 카드 보여주기 끝날 때까지 대기할 tcs 발급
            string json = JsonUtility.ToJson(usedCardData); // Json 형태로 변환
            if (GameModeManager.Instance.CurrentGameMode.UseServer())
                NetworkManager.Instance.Socket.Emit("usedCard", json); // 서버로 카드를 사용했다고 전송


            await InGameContext.Current.Data.CardManager.UsedCardShowOver?.Task; // tcs 대기

            // 상대 도로 1개를 자신을 도로로 변경
            foreach (var pieceBase in InGameContext.Current.Data.PieceManager.CanChangeRoadList) // 변환 가능한 도로 리스트를 순회
            {
                Road road = pieceBase as Road; // Road 클래스로 변환
                if (road != null) // 성공적으로 변환이 되었다면
                {
                    road.ChangeMaterial(false); // 도로를 선택 가능한 상태로 변경(+ 머티리얼을 이미션 머티리얼로 변경)
                }
            }

            bool result = await base.UseCard();
            return result;
        }
    }
}
// 마지막 작성 일자: 2026.05.20