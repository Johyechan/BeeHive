using InGame.MyEnum;
using InGame.MyEvent;
using InGame.MyObject.Interface;
using InGame.MyObject.MyObjectInterface;
using InGame.MyObject.Piece;
using InGame.MyObject.Piece.Data;
using InGame.MyObject.Piece.Handler;
using UnityEngine;

namespace InGame.MyObject
{
    // 작성자: 조혜찬
    // 배치 칸의 기능 클래스
    public abstract class PlacePlaneObjectBase : MonoBehaviour, IClickObject, INetworkIdObject
    {
        public bool isNearToCastle; // 성과 근접한 배치 판인지 확인

        public int team1GoldCoin; // 이 칸에서 Team1 광부가 획득할 수 있는 금화 수
        public int team2GoldCoin; // 이 칸에서 Team2 광부가 획득할 수 있는 금화 수
        public int team3GoldCoin; // 이 칸에서 Team3 광부가 획득할 수 있는 금화 수

        public TeamType currentPlayerTeamType; // 현재 플레이어의 팀 타입

        [SerializeField] private MaterialData _materialData; // 머티리얼을 변경하기 위한 변수들을 가지는 구조체

        private ChangeMaterialHandler _changeMaterialHandler; // 머티리얼 변경 핸들러

        private Collider _collider; // 콜라이더 변수

        private TeamType _teamType; // 어떤 팀인지 확인하기 위한 변수
        public TeamType TeamType { get { return _teamType; } set { _teamType = value; } } // _teamType 프로퍼티

        private ObjectType _placedObjectType; // 어떤 기물이 배치되어있는지 알기 위한 변수
        public ObjectType PlacedObjectType { get { return _placedObjectType; } set { _placedObjectType = value; } } // 외부에서 어떤 기물이 배치되어있는지 알고, 어떤 기물을 배치한 것인지 설정하기 위한 프로퍼티
        private ObjectType _canPlaceTypePiece; // 현재 배치가 가능한 기물 객체를 확인하는 변수
        public ObjectType CanPlacePieceType { get { return _canPlaceTypePiece; } set { _canPlaceTypePiece = value; } } // 외부에서 현재 배치가 가능한 기물 객체를 설정할 프로퍼티

        private PieceBase _placedPiece; // 올려져 있는 기물
        public PieceBase PlacedPiece { get => _placedPiece; set => _placedPiece = value; } // 위 변수 프로퍼티

        protected int _cost; // 비용
        public int Cost { get => _cost; set => _cost = value; } // 위 변수 프로퍼티

        protected int _leftPieceCount; // 자식 수(남은 기물 수)
        public int LeftPieceCount { get => _leftPieceCount; set => _leftPieceCount = value; }

        public int NetworkId { get; set; } // 네트워크 ID

        GameObject INetworkIdObject.CurrentObject => gameObject;

        protected virtual void Awake()
        {
            _changeMaterialHandler = new ChangeMaterialHandler(_materialData, gameObject);
            _collider = GetComponent<Collider>();

            _collider.enabled = false; // 콜라이더 비활성화
            _placedPiece = null; // 아무것도 안 올려져 있는 상태로 초기화
            _placedObjectType = ObjectType.None; // 아무것도 안 올려져 있는 상태로 초기화
            _canPlaceTypePiece = ObjectType.None; // 아무것도 배치 할 수 없는 상태로 초기화
        }

        // 공격 가능하다고 알려주는 하이라이트 온오프
        protected void CanAttackHighLight(bool isOn)
        {
            _collider.enabled = isOn;
            _changeMaterialHandler.ChangeCanAttackMaterial(isOn);
        }

        // 하이라이트를 키는 함수
        public void HighLightOn()
        {
            _collider.enabled = true; // 클릭이 되도록 콜라이더 활성화
            _changeMaterialHandler.ChangeMaterial(false);
        }

        // 하이라이트를 끄는 함수
        public void HighLightOff()
        {
            HighLightEvents.SelectedPlacementType = ObjectType.None; // 현재 어떤 기물을 배치 할 수 있는지 저장하는 변수 초기화
            _canPlaceTypePiece = ObjectType.None; // 배치 가능한 타입 초기화

            _collider.enabled = false; // 클릭이 되지 않도록 콜라이더 비활성화

            _changeMaterialHandler.ChangeMaterial(true);

            _cost = 0; // 비용 초기화
        }

        public abstract void ObjectClicked();
    }
}
// 마지막 작성 일자: 2026.05.04