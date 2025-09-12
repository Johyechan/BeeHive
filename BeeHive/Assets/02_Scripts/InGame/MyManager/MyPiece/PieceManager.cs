using InGame.MyEnum;
using InGame.MyEvent;
using InGame.MyObject;
using MyUtil;
using System.Collections.Generic;

namespace InGame.MyManager.MyPiece
{
    public class PieceManager : MonoSingleton<PieceManager>
    {
        private Dictionary<ObjectType, List<PieceBase>> _canAttackPieceMap = new Dictionary<ObjectType, List<PieceBase>>(); // 공격 가능한 기물들을 저장하는 맵
        public Dictionary<ObjectType, List<PieceBase>> CanAttackPieceMap { get =>  _canAttackPieceMap; } // 위 변수 프로퍼티

        protected override void Awake()
        {
            base.Awake();

            _canAttackPieceMap.Add(ObjectType.Miner, new List<PieceBase>());
            _canAttackPieceMap.Add(ObjectType.Soldier, new List<PieceBase>());
            _canAttackPieceMap.Add(ObjectType.Tank, new List<PieceBase>());
        }

        private void OnEnable()
        {
            PieceEvents.OnShowCanAttackPieces += ShowCanAttackPieces;
            PieceEvents.OnHideCanAttackPieces += HideCanAttackPieces;
        }

        private void OnDisable()
        {
            PieceEvents.OnShowCanAttackPieces -= ShowCanAttackPieces;
            PieceEvents.OnHideCanAttackPieces -= HideCanAttackPieces;
        }

        private void ShowCanAttackPieces(ObjectType type)
        {
            foreach(var piece in _canAttackPieceMap) // 공격 가능 기물들 저장 맵 순회
            {
                if(piece.Key == type) // 매개 변수로 받은 공격 가능 기물의 타입과 현재 순서의 타입이 같다면
                {
                    foreach (var pieceBase in piece.Value) // 해당 타입에 맞는 기물들을 저장한 리스트 순회
                    {
                        switch(pieceBase.teamType) // 해당 기물의 팀 타입에 따라
                        {
                            case TeamType.Team1:
                                pieceBase.ChangeMaterial(false);
                                break;
                            case TeamType.Team2:
                                pieceBase.ChangeMaterial(false);
                                break;
                            case TeamType.Team3:
                                pieceBase.ChangeMaterial(false);
                                break;
                        }
                    }
                    break;
                }
            }
        }

        private void HideCanAttackPieces()
        {
            foreach (var piece in _canAttackPieceMap) // 공격 가능 기물들 저장 맵 순회
            {
                foreach (var pieceBase in piece.Value) // 해당 타입에 맞는 기물들을 저장한 리스트 순회
                {
                    pieceBase.ChangeMaterial(true);
                }
            }
        }
    }
}
// 마지막 작성 일자: 2025.09.12