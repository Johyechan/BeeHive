using InGame.MyEnum;
using InGame.MyEvent;
using InGame.MyManager;
using InGame.MyManager.MyPlacePlane;
using InGame.MyObject.Piece.Data;
using System.Threading.Tasks;

namespace InGame.MyObject.Piece.Handler
{
    // 작성자: 조혜찬
    public class PieceSelectHandler
    {
        private PieceBase _pieceBase; // 기물 클래스

        private PieceData _pieceData; // 불변 변수를 가지는 구조체

        // 생성자(불변 변수를 가지는 구조체)
        public PieceSelectHandler(PieceBase pieceBase, PieceData pieceData)
        {
            _pieceBase = pieceBase;
            _pieceData = pieceData;
        }

        public async Task PieceSelect()
        {
            HighLightEvents.OnPieceMovementHighLight?.Invoke(false, false); // 하이라이트 끄기, 이동 가능 배치 칸 대상
            HighLightEvents.OnRoadPlacementHighLight?.Invoke(false); // 도로 배치 칸 하이라이트 끄기
            HighLightEvents.OnPiecePlacementHighLight?.Invoke(false, true); // 기물 배치 칸 하이라이트 끄기, 배치 가능 배치 판 대상
            await PieceEvents.OnHideCanAttackPieces?.Invoke(); // 공격 가능한 기물들 하이라이트 끄기

            if (_pieceData.currentObjectType != ObjectType.Tank) // 전차가 아닐 경우
            {
                await PlacePlaneManager.Instance.Variable.findCanPlacePlaneSystem.FindCanMovePlacePlane(_pieceBase.PieceVariable.currentPlacePlane, TeamManager.Instance.CurrentTeamType, _pieceData.currentObjectType); // 한 칸 이동 가능한 칸 찾기
            }
            else // 전차일 경우
            {
                if(CardManager.Instance.HaveFirePowerCard) // 화력 카드를 가지고 있을 때
                {
                    await PlacePlaneManager.Instance.Variable.findCanPlacePlaneSystem.FindCanFirePowerAttackPiece(_pieceBase.CurrentTeamType, _pieceBase.PieceVariable.currentPlacePlane); // 한 칸 떨어진 기물들을 공격 가능 대상으로 지정
                }
            }

                GameManager.Instance.CurrentMovePiece = _pieceBase.gameObject; // 현재 객체를 현재 이동하려는 기물로 할당
            HighLightEvents.SelectedPlacementType = ObjectType.None; // 배치 하는 것이 아닌 이동의 여부이기에 None으로 설정

            foreach (var piece in PlacePlaneManager.Instance.Variable.highLightHandler.CanPieceMovePlanes) // 배치 가능한 도로 칸들 순회
            {
                piece.CanPlacePieceType = _pieceData.currentObjectType; // 배치 가능한 타입을 할당
            }

            HighLightEvents.OnPieceMovementHighLight?.Invoke(true, false); // 기물 이동 칸 하이라이트 키기, 이동 가능 배치 판 대상
            switch (_pieceData.currentObjectType) // 현재 배치가능한(즉 배치하려는) 기물의 타입에 따라
            {
                case ObjectType.Soldier: // 보병일 경우
                    await PieceEvents.OnShowCanAttackPieces?.Invoke(ObjectType.Miner); // 공격 가능한 광부 기물들 하이라이트 키기
                    await PieceEvents.OnShowCanAttackPieces?.Invoke(ObjectType.Soldier); // 공격 가능한 보병 기물들 하이라이트 키기
                    break;
                case ObjectType.Tank: // 전차일 경우
                    await PieceEvents.OnShowCanAttackPieces?.Invoke(ObjectType.Miner); // 공격 가능한 광부 기물들 하이라이트 키기
                    await PieceEvents.OnShowCanAttackPieces?.Invoke(ObjectType.Soldier); // 공격 가능한 보병 기물들 하이라이트 키기
                    await PieceEvents.OnShowCanAttackPieces?.Invoke(ObjectType.Tank); // 공격 가능한 전차 기물들 하이라이트 키기
                    break;
            }
            _pieceBase.PieceVariable.isSelected = true; // 선택 되었다고 할당
        }
    }
}
// 마지막 작성 일자: 2025.09.25