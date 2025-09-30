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
        public override void UseCard()
        {
            // 상대 도로 1개를 자신을 도로로 변경
            foreach(var pieceBase in PieceManager.Instance.CanChangeRoadList) // 변환 가능한 도로 리스트를 순회
            {
                Road road = pieceBase as Road; // Road 클래스로 변환
                if(road != null) // 성공적으로 변환이 되었다면
                {
                    _ = road.ChangeMaterial(false); // 도로를 선택 가능한 상태로 변경(+ 머티리얼을 이미션 머티리얼로 변경)
                }
            }
        }
    }
}
// 마지막 작성 일자: 2025.09.30