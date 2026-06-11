using DG.Tweening;
using InGame.MyEnum;
using InGame.MyEvent;
using InGame.MyManager.Global;
using InGame.MyManager.Local;
using InGame.MyObject.Interface;
using InGame.MyObject.MyObjectInterface;
using InGame.MyObject.Piece.Data;
using InGame.MyObject.Piece.Handler;
using InGame.MyObject.Piece.Variable;
using InGame.MyUI;
using MyUtil.MyObjectPool;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace InGame.MyObject.Piece
{
    // 작성자: 조혜찬
    // 기물들의 기본적인 기능을 가지는 부모 클래스
    public abstract class PieceBase : MonoBehaviour, IClickObject, INetworkIdObject
    {
        [SerializeField] private PieceData _pieceData; // Inspector창에서 할당을 받는 변수들을 가지는 클래스

        [SerializeField] private int _damage; // 기물 피해량
        public int Damage { get => _damage; } // 외부에서 접근 가능한 기물 피해량 프로퍼티

        public ConfirmUI CurrentConfirmUI { get => _pieceData.confirmUI; set => _pieceData.confirmUI = value; } // 확인 UI 프로퍼티

        public PieceData PieceData { get => _pieceData; }// _pieceData 프로퍼티

        public ObjectType CurrentObjectType { get => _pieceData.currentObjectType; } // 현재 객체의 타입

        public TeamType CurrentTeamType { get => _pieceData.teamType; } // 현재 팀 타입 프로퍼티

        protected PieceVariable _pieceVariable = new PieceVariable(); // 변경이 잦은 변수들을 가지는 클래스
        public PieceVariable PieceVariable { get => _pieceVariable; }

        public int NetworkId { get; set; } // 네트워크 ID

        public GameObject CurrentObject => gameObject;

        protected virtual void Awake()
        {
            _pieceData.changeMaterialHandler = new ChangeMaterialHandler(_pieceData.materialData, gameObject); // 머티리얼 변경 핸들러 생성
            _pieceData.pieceMoveHandler = new PieceMoveHandler(this, _pieceData); // 기물 이동 핸들러 생성
            _pieceData.pieceDeselectHandler = new PieceDeselectHandler(); // 기물 선택 해제 핸들러 생성
            _pieceData.pieceSelectHandler = new PieceSelectHandler(this, _pieceData); // 기물 선택 핸들러 생성
            _pieceData.pieceAttackedHandler = new PieceAttackedHandler(this, _pieceData); // 기물 공격 받는 기능 핸들러 생성
        }

        protected virtual void OnEnable()
        {

        }

        protected virtual void OnDisable()
        {

        }

        // 외부에서 머티리얼을 변경할 때 사용하는 함수(기본 머티리얼로 변경할지 여부)
        public void ChangeMaterial(bool isChangeToOrigin)
        {
            _pieceData.changeMaterialHandler.ChangeMaterial(isChangeToOrigin); // 머티리얼 변경 핸들러 함수 호출
        }

        // 공격한 기물 처리 함수
        public void PieceDestroy()
        {
            transform.DOLocalMoveY(ObjectPoolManager.Instance.AnimationYPos, _pieceData.animationDuration)
                .OnComplete(() =>
                {
                    Destroy(gameObject); // 기물 파괴
                });
        }

        // 기물들을 지정 위치로 이동 시키는 함수(부모, 이동 위치, 기물 이동인지(첫 배치가 아닌 배치되어있는 상태에서 다른 칸으로 이동), 각도)
        public async Task MoveToPlacePlane(Transform parent, Vector3 targetPos, bool isMove = false, float angle = 0)
        {
            await _pieceData.pieceMoveHandler.MoveToPlacePlane(parent, targetPos, isMove, angle);
        }

        // 오브젝트가 마우스로 클릭되었을 때 실행될 함수
        public virtual async void ObjectClicked()
        {
            if(InGameContext.Current.Data.PieceManager.IsPlayAnimation) // 기물 애니메이션 실행 중이라면
            {
                return; // 반환
            }

            if(InGameContext.Current.Data.PieceManager.IsRoadChanging) // 도로가 변경 중이라면
            {
                return; // 반환
            }

            if (!WarningEvent.OnCheckCurrentTurnTeam()) // 현재 턴이 자신의 턴이 아닐 경우
            {
                HighLightEvents.OnPiecePlacementHighLight?.Invoke(false, true); // 기물 칸 하이라이트를 끄는 매개변수로 이벤트 콜(하이라이트 키기 여부, 배치 칸 이동 칸 여부 - true는 배치칸, false는 이동칸)
                HighLightEvents.OnPieceMovementHighLight?.Invoke(false, false); // 기물 칸 하이라이트를 끄는 매개변수로 이벤트 콜(하이라이트 키기 여부, 배치 칸 이동 칸 여부 - true는 배치칸, false는 이동칸)
                PieceEvents.OnHideCanAttackPieces?.Invoke(true); // 공격 가능한 기물들 하이라이트 끄기
                return; // 반환
            }

            string str = LocalizationSettings.StringDatabase.GetLocalizedString(
                "Game",
                "Game_UI_NotMainTurnCanNotMove"
            );
            // 현재 턴이 메인 턴이 아니라면
            if (!WarningEvent.OnCheckCurrentTurn.Invoke(TurnType.MainTurn, str))
                return; // 반환

            if (_pieceData.teamType != TeamManager.Instance.CurrentTeamType) // 현재 팀과 다른 팀의 기물이라면
            {
                await _pieceData.pieceAttackedHandler.PieceAttacked();
                return; // 반환
            }

            // 클릭 되었을 때 이동 가능한 배치 칸 하이라이트 활성화
            if (!_pieceVariable.isSelected) // 선택된 상태가 아닐 경우
            {
                _pieceData.pieceSelectHandler.PieceSelect(); // 선택 함수 호출
            }
            else // 선택된 상태일 경우
            {
                _pieceData.pieceDeselectHandler.PieceDeselect(); // 선택 해제 함수 호출
            }
        }
    }
}
// 마지막 작성 일자: 2026.06.11