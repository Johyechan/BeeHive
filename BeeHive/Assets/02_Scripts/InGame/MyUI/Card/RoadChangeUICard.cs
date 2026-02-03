using InGame.MyEnum;
using InGame.MyEvent;
using InGame.MyManager;
using InGame.MyManager.Global;
using InGame.MyManager.Local;
using InGame.MyManager.MyPiece;
using InGame.MyObject.Piece.ObjectPieces;
using UnityEngine;

namespace InGame.MyUI.Card
{
    // 작성자: 조혜찬
    // 도로 변형 카드
    public class RoadChangeUICard : UICardBase
    {
        // 카드 기능을 실제로 수행하는 함수
        public override bool UseCard()
        {
            if (InGameContext.Current.Data.CardManager.CheckSameTypeCardWasUsed(CardType.RoadChange)) // 도로 변형 카드 일전에 사용 했었는지 확인
            {
                return false;
            }

            if (InGameContext.Current.Data.PieceManager.CanChangeRoadList.Count <= 0) // 도로 변형이 가능한 도로가 없다면
            {
                UIManager.Instance.WarningUIMake("도로 변형 가능한 도로가 없어서 사용 불가합니다.");
                return false;
            }

            InGameContext.Current.Data.CardManager.CardUsed = true; // 카드 사용

            UsedCardData usedCardData = new UsedCardData()
            {
                roomID = SceneMgr.Instance.CurrentRoomID, // 현재 방 ID
                usedCardName = _uiCardData.currentCardName, // 사용한 카드의 이름
                usedCardInformation = _uiCardData.cardInformationText, // 사용한 카드의 정보(효과)
            };

            string json = JsonUtility.ToJson(usedCardData); // Json 형태로 변환
            NetworkManager.Instance.Socket.Emit("usedCard", json); // 서버로 카드를 사용했다고 전송

            // 상대 도로 1개를 자신을 도로로 변경
            foreach (var pieceBase in InGameContext.Current.Data.PieceManager.CanChangeRoadList) // 변환 가능한 도로 리스트를 순회
            {
                Road road = pieceBase as Road; // Road 클래스로 변환
                if(road != null) // 성공적으로 변환이 되었다면
                {
                    road.ChangeMaterial(false); // 도로를 선택 가능한 상태로 변경(+ 머티리얼을 이미션 머티리얼로 변경)
                }
            }

            return base.UseCard();
        }
    }
}
// 마지막 작성 일자: 2026.02.03