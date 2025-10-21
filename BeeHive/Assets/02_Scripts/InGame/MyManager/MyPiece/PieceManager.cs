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

        private Dictionary<ObjectType, List<PieceBase>> _canFirePowerAttackPieceMap = new Dictionary<ObjectType, List<PieceBase>>(); // 화력으로 공격 가능한 기물들을 저장하는 맵
        public Dictionary<ObjectType, List<PieceBase>> CanFirePowerAttackPieceMap { get => _canFirePowerAttackPieceMap; } // 위 변수 프로퍼티

        private List<PieceBase> _canChangeRoadList = new List<PieceBase>(); // 변경 가능한 도로를 저장하는 리스트
        public List<PieceBase> CanChangeRoadList { get =>  _canChangeRoadList; } // 위 변수 프로퍼티

        private AttackRelatedPiecesMoveHandler _attackRelatedPiecesMoveHandler; // 공격 관련 기물들 이동 핸들러

        private CanAttackPieceStateHandler _canAttackPieceStateHandler; // 공격 가능한 기물들의 상태 변경 핸들러

        private bool _isDrought; // 가뭄인지 여부를 확인하는 변수
        public bool IsDrought { get => _isDrought; set => _isDrought = value; } // 위 변수 프로퍼티

        protected override void Awake()
        {
            base.Awake();

            _attackRelatedPiecesMoveHandler = new AttackRelatedPiecesMoveHandler();
            _canAttackPieceStateHandler = new CanAttackPieceStateHandler();

            _canAttackPieceMap.Add(ObjectType.Miner, new List<PieceBase>());
            _canAttackPieceMap.Add(ObjectType.Soldier, new List<PieceBase>());
            _canAttackPieceMap.Add(ObjectType.Tank, new List<PieceBase>());

            _canFirePowerAttackPieceMap.Add(ObjectType.Miner, new List<PieceBase>());
            _canFirePowerAttackPieceMap.Add(ObjectType.Soldier, new List<PieceBase>());
            _canFirePowerAttackPieceMap.Add(ObjectType.Tank, new List<PieceBase>());
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

        private async Task ShowCanAttackPieces(ObjectType canAttackType, ObjectType currentPieceType)
        {
            await _canAttackPieceStateHandler.ShowCanAttackPieces(canAttackType, _canAttackPieceMap); // 근거리 공격 가능 기물 탐색

            if (currentPieceType == ObjectType.Tank) // 현재 기물이 전차일 경우
                await _canAttackPieceStateHandler.ShowCanAttackPieces(canAttackType, _canFirePowerAttackPieceMap, true); // 원거리 공격 가능 기물 탐색
        }

        private async Task HideCanAttackPieces()
        {
            await _canAttackPieceStateHandler.HideCanAttackPieces(_canAttackPieceMap); // 근거리 공격 대상 숨기기
            await _canAttackPieceStateHandler.HideCanAttackPieces(_canFirePowerAttackPieceMap, true); // 원거리 공격 대상 숨기기
        }

        public async Task FindCanPlacePlane()
        {
            await PlacePlaneManager.Instance.FindCanPlacePlane();
        }
    }
}
// 마지막 작성 일자: 2025.10.21