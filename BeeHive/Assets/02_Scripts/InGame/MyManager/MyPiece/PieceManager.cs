using DG.Tweening;
using InGame.MyEnum;
using InGame.MyEvent;
using InGame.MyManager.MyPiece.Handler;
using InGame.MyManager.MyPlacePlane;
using InGame.MyObject.Piece;
using MyUtil;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace InGame.MyManager.MyPiece
{
    public class PieceManager : MonoSingleton<PieceManager>
    {
        private Dictionary<ObjectType, List<PieceBase>> _canAttackPieceMap = new Dictionary<ObjectType, List<PieceBase>>(); // 공격 가능한 기물들을 저장하는 맵
        public Dictionary<ObjectType, List<PieceBase>> CanAttackPieceMap { get =>  _canAttackPieceMap; } // 위 변수 프로퍼티

        private AttackRelatedPiecesMoveHandler _attackRelatedPiecesMoveHandler; // 공격 관련 기물들 이동 핸들러

        private CanAttackPieceStateHandler _canAttackPieceStateHandler; // 공격 가능한 기물들의 상태 변경 핸들러

        protected override void Awake()
        {
            base.Awake();

            _attackRelatedPiecesMoveHandler = new AttackRelatedPiecesMoveHandler();
            _canAttackPieceStateHandler = new CanAttackPieceStateHandler();

            _canAttackPieceMap.Add(ObjectType.Miner, new List<PieceBase>());
            _canAttackPieceMap.Add(ObjectType.Soldier, new List<PieceBase>());
            _canAttackPieceMap.Add(ObjectType.Tank, new List<PieceBase>());
        }

        private void OnEnable()
        {
            // 이벤트 Task로 변경해야함
            PieceEvents.OnShowCanAttackPieces += ShowCanAttackPieces;
            PieceEvents.OnHideCanAttackPieces += HideCanAttackPieces;
        }

        private void OnDisable()
        {
            PieceEvents.OnShowCanAttackPieces -= ShowCanAttackPieces;
            PieceEvents.OnHideCanAttackPieces -= HideCanAttackPieces;
        }

        // 공격 당한 기물과 공격한 기물이 이동하는 함수(공격 당한 기물, 공격한 기물공격 당한 기물의 부모, 공격한 기물의 부모, 공격 당한 기물의 목적지, 공격한 기물의 목적지)
        public async Task AttackRelatedPiecesMove(PieceBase returnPiece, PieceBase attackPiece, Transform returnParent, Transform attackParent, Vector3 returnPos, Vector3 attackPos)
        {
            await _attackRelatedPiecesMoveHandler.AttackRelatedPiecesMove(returnPiece, attackPiece, returnParent, attackParent, returnPos, attackPos);
        }

        private async Task ShowCanAttackPieces(ObjectType type)
        {
            await _canAttackPieceStateHandler.ShowCanAttackPieces(type, _canAttackPieceMap);
        }

        private async Task HideCanAttackPieces()
        {
            await _canAttackPieceStateHandler.HideCanAttackPieces(_canAttackPieceMap);
        }

        public async Task FindCanPlacePlane()
        {
            await PlacePlaneManager.Instance.FindCanPlacePlane().AsyncWaitForCompletion();
        }
    }
}
// 마지막 작성 일자: 2025.09.19