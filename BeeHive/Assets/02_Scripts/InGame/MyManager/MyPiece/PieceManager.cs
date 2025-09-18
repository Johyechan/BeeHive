using DG.Tweening;
using InGame.MyEnum;
using InGame.MyEvent;
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

        protected override void Awake()
        {
            base.Awake();

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
        public async Task MoveAttackRelatedPieces(PieceBase returnPiece, PieceBase attackPiece, Transform returnParent, Transform attackParent, Vector3 returnPos, Vector3 attackPos)
        {
            HighLightEvents.OnPieceMovementHighLight?.Invoke(false, false); // 하이라이트 끄기, 이동 가능 배치 칸 대상
            HighLightEvents.OnRoadPlacementHighLight?.Invoke(false); // 도로 배치 칸 하이라이트 끄기
            HighLightEvents.OnPiecePlacementHighLight?.Invoke(false, true); // 기물 배치 칸 하이라이트 끄기, 배치 가능 배치 판 대상
            _ = PieceEvents.OnHideCanAttackPieces?.Invoke(); // 공격 가능한 기물들 하이라이트 끄기

            UIManager.Instance.CanInteractionUI = false; // UI 상호작용 불가능 상태로 할당

            attackPiece.PieceVariable.currentPlacePlane.PlacedObjectType = ObjectType.None; // 공격 기물의 현재 칸의 배치된 객체 타입을 초기화
            attackPiece.PieceVariable.currentPlacePlane.TeamType = TeamType.None; // 공격 기물의 현재 칸의 팀 타입을 초기화

            attackPiece.PieceVariable.currentPlacePlane = returnPiece.PieceVariable.currentPlacePlane; // 공격 기물의 배치된 칸을 공격 당한 기물이 배치되어 있던 칸으로 초기화
            returnPiece.PieceVariable.currentPlacePlane = null; // 공격 받은 기물의 배치된 칸을 null로 초기화
            attackPiece.PieceVariable.currentPlacePlane.PlacedPiece = attackPiece; // 배치된 칸의 배치된 기물을 공격한 기물로 할당
            attackPiece.PieceVariable.currentPlacePlane.PlacedObjectType = attackPiece.CurrentObjectType; // 공격 기물의 배치된 칸의 배치된 기물의 타입을 공격 기물의 타입으로 할당
            attackPiece.PieceVariable.currentPlacePlane.TeamType = attackPiece.CurrentTeamType; // 공격 기물의 배치된 칸의 팀 타입을 공격 기물의 팀 타입으로 할당

            await returnPiece.MoveToPlacePlane(returnParent, returnPos); // 공격 받은 기물 이동
            await attackPiece.MoveToPlacePlane(attackParent, attackPos); // 공격한 기물 이동

            if(attackPiece.CurrentObjectType == ObjectType.Soldier)
            {
                if (attackPiece.CurrentTeamType == TeamManager.Instance.CurrentTeamType) // 공격한 기물이 현재 팀의 기물일 경우에만
                {
                    ChangeRoadInfo changeRoadInfo = new ChangeRoadInfo
                    {
                        roomID = SceneMgr.Instance.CurrentRoomID, // 현재 방 ID
                        teamType = (int)attackPiece.CurrentTeamType, // 공격한 기물 팀 타입
                        placePlaneID = attackPiece.PieceVariable.currentPlacePlane.Id // 공격한 기물의 목적지 칸의 ID
                    };

                    string json = JsonUtility.ToJson(changeRoadInfo);

                    NetworkManager.Instance.Socket.Emit("changeRoad", json);

                    await PieceEvents.OnChangeNearRoad?.Invoke(attackPiece.CurrentTeamType, attackPiece.PieceVariable.currentPlacePlane); // 도로 변경 이벤트 호출
                }
            }

            await FindCanPlacePlane(); // 다시 이동가능한 위치 찾기
        }

        private async Task ShowCanAttackPieces(ObjectType type)
        {
            foreach(var piece in _canAttackPieceMap) // 공격 가능 기물들 저장 맵 순회
            {
                if(piece.Key == type) // 매개 변수로 받은 공격 가능 기물의 타입과 현재 순서의 타입이 같다면
                {
                    foreach (var pieceBase in piece.Value) // 해당 타입에 맞는 기물들을 저장한 리스트 순회
                    {
                        switch(pieceBase.CurrentTeamType) // 해당 기물의 팀 타입에 따라
                        {
                            case TeamType.Team1:
                                await pieceBase.ChangeMaterial(false);
                                break;
                            case TeamType.Team2:
                                await pieceBase.ChangeMaterial(false);
                                break;
                            case TeamType.Team3:
                                await pieceBase.ChangeMaterial(false);
                                break;
                        }
                    }
                    break;
                }
            }
        }

        private async Task HideCanAttackPieces()
        {
            NetworkManager.Instance.Socket.Emit("debug", $"{_canAttackPieceMap}");
            foreach (var piece in _canAttackPieceMap) // 공격 가능 기물들 저장 맵 순회
            {
                foreach (var pieceBase in piece.Value) // 해당 타입에 맞는 기물들을 저장한 리스트 순회
                {
                    await pieceBase.ChangeMaterial(true);
                }
            }
        }

        public async Task FindCanPlacePlane()
        {
            await PlacePlaneManager.Instance.FindCanPlacePlane().AsyncWaitForCompletion();
        }
    }
}
// 마지막 작성 일자: 2025.09.18