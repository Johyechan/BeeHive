using InGame.MyEnum;
using InGame.MyEvent;
using InGame.MyObject;
using MyUtil;
using System.Collections.Generic;
using UnityEngine;

namespace InGame.MyManager.MyPiece
{
    public class PieceManager : MonoSingleton<PieceManager>
    {
        [SerializeField] private float _intensity; // Emission의 강도

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
                                NetworkManager.Instance.Socket.Emit("debug", $"팀1이 빛나리 (PieceManager: 51)");
                                pieceBase.Material.SetColor("_EmissionColor", Color.red * _intensity);
                                break;
                            case TeamType.Team2:
                                NetworkManager.Instance.Socket.Emit("debug", $"팀2가 빛나리 (PieceManager: 55)");
                                pieceBase.Material.SetColor("_EmissionColor", Color.blue * _intensity);
                                break;
                            case TeamType.Team3:
                                pieceBase.Material.SetColor("_EmissionColor", Color.green * _intensity);
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
                    pieceBase.Material.SetColor("_EmissionColor", Color.black); // 블랙으로 Emission이 눈에 안보이도록 설정
                }
            }
        }
    }
}
// 마지막 작성 일자: 2025.09.10