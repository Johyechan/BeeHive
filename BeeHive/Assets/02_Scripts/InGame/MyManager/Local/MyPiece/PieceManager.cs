using DG.Tweening;
using InGame.MyEnum;
using InGame.MyEvent;
using InGame.MyManager.Global;
using InGame.MyManager.MyPiece.Handler;
using InGame.MyObject;
using InGame.MyObject.Piece;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace InGame.MyManager.Local.MyPiece
{
    // 작성자: 조혜찬
    // 기물 매니저 클래스

    public class PieceManager : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _waitConfirmUI; // 확인 대기 UI

        [SerializeField] private float _animationDuration; // 애니메이션 지속 시간

        [SerializeField] private int _waitTime; // 공격 대응 대기 시간
        public int WaitTime { get => _waitTime; } // 공격 대응 대기 시간 프로퍼티

        private Dictionary<PieceBase, List<PieceBase>> _canAttackPieceMap = new Dictionary<PieceBase, List<PieceBase>>(); // 공격 가능한 기물들을 저장하는 맵
        public Dictionary<PieceBase, List<PieceBase>> CanAttackPieceMap { get =>  _canAttackPieceMap; } // 위 변수 프로퍼티

        private Dictionary<PieceBase, List<PieceBase>> _canFirePowerAttackPieceMap = new Dictionary<PieceBase, List<PieceBase>>(); // 화력으로 공격 가능한 기물들을 저장하는 맵
        public Dictionary<PieceBase, List<PieceBase>> CanFirePowerAttackPieceMap { get => _canFirePowerAttackPieceMap; } // 위 변수 프로퍼티

        private Dictionary<PieceBase, List<PiecePlacePlaneObject>> _canFirePowerAttackPiecePlaceMap = new Dictionary<PieceBase, List<PiecePlacePlaneObject>>(); // 화력으로 공격 가능한 기물 배치칸
        public Dictionary<PieceBase, List<PiecePlacePlaneObject>> CanFirePowerAttackPiecePlaceMap { get => _canFirePowerAttackPiecePlaceMap; } // 위 변수 프로퍼티

        private List<PieceBase> _canChangeRoadList = new List<PieceBase>(); // 변경 가능한 도로를 저장하는 리스트
        public List<PieceBase> CanChangeRoadList { get =>  _canChangeRoadList; } // 위 변수 프로퍼티

        private AttackRelatedPiecesMoveHandler _attackRelatedPiecesMoveHandler; // 공격 관련 기물들 이동 핸들러

        private CanAttackPieceStateHandler _canAttackPieceStateHandler; // 공격 가능한 기물들의 상태 변경 핸들러

        private bool _isDrought; // 가뭄인지 여부를 확인하는 변수
        public bool IsDrought { get => _isDrought; set => _isDrought = value; } // 위 변수 프로퍼티
        private bool _opponentDrought; // 상대가 가뭄인지 여부
        public bool OpponentDrought { get => _opponentDrought; set => _opponentDrought = value; } // 상대가 가뭄인지 여부 프로퍼티

        private bool _isPlayAnimation = false; // 기물이 특정 위치로 가기 위해 애니메이션이 실행중이라면
        public bool IsPlayAnimation { get => _isPlayAnimation; set => _isPlayAnimation = value; } // 위 변수 프로퍼티

        private TaskCompletionSource<int> _tcs; // 1이 참, 0이 거짓

        private void Awake()
        {
            _attackRelatedPiecesMoveHandler = new AttackRelatedPiecesMoveHandler();
            _canAttackPieceStateHandler = new CanAttackPieceStateHandler();

            NetworkManager.Instance.Socket.On("opponentChooseOne", value =>
            {
                if (NetworkManager.Instance.IsClientOver) // 클라이언트가 종료 되었다면
                    return; // 반환

                int result = value.GetValue<int>();

                _tcs?.TrySetResult(result);
            });
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

        // 확인 대기 UI 페이드 인아웃 함수
        public void FadeInOutWaitConfirmUI(float endValue)
        {
            if(endValue > 0)
            {
                _waitConfirmUI.gameObject.SetActive(true); // 활성화
            }

            _waitConfirmUI.DOFade(endValue, _animationDuration)
                .OnComplete(() =>
                {
                    if (endValue <= 0)
                    {
                        _waitConfirmUI.gameObject.SetActive(false); // 비활성화
                    }
                });
        }

        public async Task<int> OpponentChoice(int delay = 5) // 기본적으로 5초 대기
        {
            _tcs = new TaskCompletionSource<int>();

            var delayTask = Task.Delay(delay * 1000); // 1000을 곱함으로써 millisecond에 맞추기
            var completed = await Task.WhenAny(_tcs.Task, delayTask); // 두 Task중 먼저 끝난 Task를 할당

            if(completed == delayTask) // 대기 시간이 다 지난 상황이라면
            {
                return 0;
            }

            return await _tcs.Task; // 상대가 5초 전에 먼저 선택한 경우 _tcs가 완료 상태가 되기 때문에 더 이상 대기 X 그래서 await를 통해 다시 값 반환
        }

        // 공격 당한 기물과 공격한 기물이 이동하는 함수(공격 당한 기물, 공격한 기물공격 당한 기물의 부모, 공격한 기물의 부모, 공격 당한 기물의 목적지, 공격한 기물의 목적지)
        public async Task AttackRelatedPiecesMove(PieceBase returnPiece, PieceBase attackPiece, Transform returnParent, Transform attackParent, Vector3 returnPos, Vector3 attackPos)
        {
            await _attackRelatedPiecesMoveHandler.AttackRelatedPiecesMove(returnPiece, attackPiece, returnParent, attackParent, returnPos, attackPos);
        }

        private void ShowCanAttackPieces(PieceBase attackingPiece)
        {
            _canAttackPieceStateHandler.ShowCanAttackPieces(attackingPiece, _canAttackPieceMap, _canFirePowerAttackPiecePlaceMap); // 근거리 공격 가능 기물 탐색

            if (attackingPiece.CurrentObjectType == ObjectType.Tank) // 현재 기물이 전차일 경우
            {
                if(InGameContext.Current.Data.CardManager.HaveFirePowerCard) // 화력 카드를 가지고 있는 경우
                {
                    _canAttackPieceStateHandler.ShowCanAttackPieces(attackingPiece, _canFirePowerAttackPieceMap, _canFirePowerAttackPiecePlaceMap, true); // 원거리 공격 가능 기물 탐색
                }
            }
        }

        private void HideCanAttackPieces(bool changeFirePowerAttack)
        {
            _canAttackPieceStateHandler.HideCanAttackPieces(_canAttackPieceMap, _canFirePowerAttackPiecePlaceMap); // 근거리 공격 대상 숨기기
            _canAttackPieceStateHandler.HideCanAttackPieces(_canFirePowerAttackPieceMap, _canFirePowerAttackPiecePlaceMap, true, changeFirePowerAttack); // 원거리 공격 대상 숨기기
        }

        public void FindCanPlacePlane()
        {
            InGameContext.Current.Data.PlacePlaneManager.FindCanPlacePlane();
        }
    }
}
// 마지막 작성 일자: 2026.05.22