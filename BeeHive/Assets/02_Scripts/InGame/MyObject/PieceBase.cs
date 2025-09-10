using DG.Tweening;
using InGame.MyEnum;
using InGame.MyEvent;
using InGame.MyManager;
using InGame.MyManager.MyPlacePlane;
using InGame.MyObject.MyObjectInterface;
using UnityEngine;

namespace InGame.MyObject
{
    // 작성자: 조혜찬
    // 기물들의 기본적인 기능을 가지는 부모 클래스
    public abstract class PieceBase : MonoBehaviour, IClickObject
    {
        public TeamType teamType; // 객체의 팀 타입

        [SerializeField] private float _animationDuration; // 애니메이션 지속시간

        [SerializeField] private ObjectType _canPlaceType; // 이동 가능한 타입을 배치 칸에 알려주기 위한 변수

        protected Transform _parent; // 기물을 모아두는 부모

        protected bool _isSelected; // 선택 여부를 확인하는 변수

        protected PiecePlacePlaneObject _currentPlacePlane; // 현재 기물이 존재하고 있는 배치 칸
        // 위 변수를 외부에서 사용 및 변경할 수 있는 프로퍼티
        public PiecePlacePlaneObject CurrentPlacePlane
        {
            get => _currentPlacePlane;
            set => _currentPlacePlane = value;
        }

        protected int _id = 0; // 객체 id
        public int Id { get => _id; set => _id = value; } // 위 변수 프로퍼티

        private Material _material; // Emission을 주기 위한 머티리얼
        public Material Material { get => _material; }

        private Renderer _renderer; // 머티리얼을 할당해주기 위한 변수 

        protected virtual void Awake()
        {
            _renderer = GetComponent<Renderer>();

            _id = ObjectIdManager.Instance.Id++;
            ObjectIdManager.Instance.AddObject(_id, gameObject); // 객체 관리 매니저에 id와 함께 추가
        }

        protected virtual void OnEnable()
        {
            HighLightEvents.OnPieceMovementHighLight += HighLightOff;
        }

        private void Start()
        {
            _material = _renderer.material; // 머티리얼 할당
        }

        protected virtual void OnDisable()
        {
            HighLightEvents.OnPieceMovementHighLight -= HighLightOff;
        }

        // 외부에서 하이라이트를 끌 때 현재 스크립트에서 하이라이트 활성화 여부를 끔 상태로 만들어주는 함수
        private void HighLightOff(bool isOn, bool isMove = true) // 켜졌는지 여부, 이동 상태를 위해 켜졌는지 여부 = 어떤 값이 와도 상관 없음
        {
            if(isOn == false) // 끄는 상태일 때
            {
                GameManager.Instance.CurrentMovePiece = null; // 현재 이동하려는 기물을 null로 할당
                _isSelected = false; // 선택 해제 된 상태로 할당
            }
        }

        // 기물들을 지정 위치로 이동 시키는 함수
        public void MoveToPlacePlane(Transform parent, Vector3 targetPos, float angle = 0)
        {
            if(parent.name == "PlacePos")
            {
                gameObject.layer = LayerMask.NameToLayer("ClickObj");
            }
            else
            {
                gameObject.layer = LayerMask.NameToLayer("Default");
            }
            transform.SetParent(parent); // 부모 변경
            float yPos = targetPos.y * 1.5f; // 이후 배치할 때 애니메이션 효과를 위해 1.5배를 하여 조금 더 높이 올려준다
            Sequence sequence = DOTween.Sequence() // 시퀀스를 통해서 차례대로 순차적으로 실행
                .Append(transform.DOLocalMoveY(yPos, _animationDuration)) // 높이 먼저 올리기
                .Append(transform.DOLocalMove(new Vector3(targetPos.x, yPos, targetPos.z), _animationDuration)) // 지정한 위치로 이동
                .Append(transform.DOLocalRotate(new Vector3(0, angle, 0), _animationDuration)) // 회전 값만큼 y축 회전
                .Append(transform.DOLocalMoveY(targetPos.y, _animationDuration)) // 이후 높이 맞추기
                .AppendCallback(() => UIManager.Instance.CanInteractionUI = true); // UI 상호작용 가능 상태로 초기화
        }

        // 오브젝트가 마우스로 클릭되었을 때 실행될 함수
        public virtual void ObjectClicked()
        {
            // 클릭 되었을 때 이동 가능한 배치 칸 하이라이트 활성화
            if (!_isSelected) // 선택된 상태가 아닐 경우
            {
                HighLightEvents.OnPieceMovementHighLight?.Invoke(false, false); // 하이라이트 끄기, 이동 가능 배치 칸 대상
                HighLightEvents.OnRoadPlacementHighLight?.Invoke(false); // 도로 배치 칸 하이라이트 끄기
                HighLightEvents.OnPiecePlacementHighLight?.Invoke(false, true); // 기물 배치 칸 하이라이트 끄기, 배치 가능 배치 판 대상
                //PieceEvents.OnHideCanAttackPieces?.Invoke(); // 공격 가능한 기물들 하이라이트 끄기

                if (_canPlaceType != ObjectType.Tank) // 전차가 아닐 경우
                    PlacePlaneManager.Instance.FindCanPlacePlaneSystem.FindCanMovePlacePlane(_currentPlacePlane, TeamManager.Instance.CurrentTeamType); // 한 칸 이동 가능한 칸 찾기

                GameManager.Instance.CurrentMovePiece = gameObject; // 현재 객체를 현재 이동하려는 기물로 할당
                HighLightEvents.SelectedPlacementType = ObjectType.None; // 배치 하는 것이 아닌 이동의 여부이기에 None으로 설정

                foreach (var piece in PlacePlaneManager.Instance.HighLightHandler.CanPieceMovePlanes) // 배치 가능한 도로 칸들 순회
                {
                    NetworkManager.Instance.Socket.Emit("debug", $"{piece.gameObject.name}, {_canPlaceType} (PieceBase: 112)");
                    piece.CanPlacePieceType = _canPlaceType; // 배치 가능한 타입을 할당
                    NetworkManager.Instance.Socket.Emit("debug", $"{piece.gameObject.name}, {piece.CanPlacePieceType} (PieceBase: 114)");
                }

                HighLightEvents.OnPieceMovementHighLight?.Invoke(true, false); // 기물 이동 칸 하이라이트 키기, 이동 가능 배치 판 대상
                _isSelected = true; // 선택 되었다고 할당
            }
            else // 선택된 상태일 경우
            {
                HighLightEvents.OnPieceMovementHighLight?.Invoke(false, false); // 하이라이트 끄기, 이동 가능 배치 칸 대상
                //PieceEvents.OnHideCanAttackPieces?.Invoke(); // 공격 가능한 기물들 하이라이트 끄기
            }
        }
    }
}
// 마지막 작성 일자: 2025.09.08