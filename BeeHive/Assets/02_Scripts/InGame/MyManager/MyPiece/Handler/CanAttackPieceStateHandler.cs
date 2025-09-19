using InGame.MyEnum;
using InGame.MyObject.Piece;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InGame.MyManager.MyPiece.Handler
{
    // 작성자: 조혜찬
    // 공격 가능한 기물의 상태를 변화시키는 기능을 처리하는 핸들러
    public class CanAttackPieceStateHandler
    {
        // 공격 가능한 기물들을 보여주는 함수(보여줄 기물의 객체 타입, (Key)객체의 타입에 따라 (Value)기물 리스트를 가지는 딕셔너리)
        public async Task ShowCanAttackPieces(ObjectType type, Dictionary<ObjectType, List<PieceBase>> canAttackPieceMap)
        {
            foreach (var piece in canAttackPieceMap) // 공격 가능 기물들 저장 맵 순회
            {
                if (piece.Key == type) // 매개 변수로 받은 공격 가능 기물의 타입과 현재 순서의 타입이 같다면
                {
                    foreach (var pieceBase in piece.Value) // 해당 타입에 맞는 기물들을 저장한 리스트 순회
                    {
                        switch (pieceBase.CurrentTeamType) // 해당 기물의 팀 타입에 따라
                        {
                            case TeamType.Team1:
                                await pieceBase.ChangeMaterial(false); // 머티리얼 변경
                                break;
                            case TeamType.Team2:
                                await pieceBase.ChangeMaterial(false); // 머티리얼 변경
                                break;
                            case TeamType.Team3:
                                await pieceBase.ChangeMaterial(false); // 머티리얼 변경
                                break;
                        }
                    }
                    break;
                }
            }
        }

        // 공격 가능한 기물을 숨기는 함수((Key)객체의 타입에 따라 (Value)기물 리스트를 가지는 딕셔너리)
        public async Task HideCanAttackPieces(Dictionary<ObjectType, List<PieceBase>> canAttackPieceMap)
        {
            foreach (var piece in canAttackPieceMap) // 공격 가능 기물들 저장 맵 순회
            {
                foreach (var pieceBase in piece.Value) // 해당 타입에 맞는 기물들을 저장한 리스트 순회
                {
                    await pieceBase.ChangeMaterial(true);
                }
            }
        }
    }
}
// 마지막 작성 일자: 2025.09.19