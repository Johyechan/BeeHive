using InGame.MyManager.MyPlacePlane;
using InGame.MyObject.MyObjectEnum;
using InGame.MyObject.MyObjectInterface;
using System.Collections.Generic;
using UnityEngine;

namespace InGame.MyObject
{
    // 작성자: 조혜찬
    // 배치 칸의 기능 클래스
    public abstract class PlacePlaneObjectBase : MonoBehaviour, IClickObject
    {
        [SerializeField] private NearCastleType _nearCastleType; // 성에 근접한 배치판인지 확인하는 변수
        public NearCastleType NearCastleTypeProp { get { return _nearCastleType; } } // _nearCastleType 프로퍼티

        private Renderer _renderer; // 머티리얼을 들고 오기 위한 변수

        private Material _material; // 하이라이트 머티리얼 변수

        private Collider _collider; // 콜라이더 변수

        private ObjectType _placedObjectType; // 어떤 기물이 배치되어있는지 알기 위한 변수
        public ObjectType PlacedObjectTypeProp { get { return _placedObjectType; } set { _placedObjectType = value; } } // 외부에서 어떤 기물이 배치되어있는지 알고, 어떤 기물을 배치한 것인지 설정하기 위한 프로퍼티
        private ObjectType _canPlacePiece; // 현재 배치가 가능한 기물 객체를 확인하는 변수
        public ObjectType CanPlacePieceProp { get { return _canPlacePiece; } set { _canPlacePiece = value; } } // 외부에서 현재 배치가 가능한 기물 객체를 설정할 프로퍼티

        private bool _isChecked; // 이전에 확인이 되었는지 확인하는 변수
        public bool IsCheckedProp { get { return _isChecked; } set { _isChecked = value; } } // 이전에 확인이 되었는지 확인하는 변수 프로퍼티

        protected virtual void Awake()
        {
            _renderer = GetComponent<Renderer>();
            _collider = GetComponent<Collider>();
            _collider.enabled = false; // 콜라이더 비활성화
            _placedObjectType = ObjectType.None; // 아무것도 안 올려져 있는 상태로 초기화
        }

        protected virtual void Start()
        {
            _material = _renderer.material; // 공용 머티리얼이 아닌 인스턴스화를 통한 개인 머티리얼을 가져옴, Start에서 실행하는 이유는 다 생성이된 후 가져와서 특정 객체가 못 가져오는 상황을 방지하기 위해서
        }

        // 하이라이트를 키는 함수
        public void HighLightOn()
        {
            PlacePlaneManager.Instance.HighLightHandlerProp.CanPlacePlanesProp.Add(this); // 하이라이트가 켜지는 이 객체의 클래스 추가
            _collider.enabled = true; // 클릭이 되도록 콜라이더 활성화
            _material.color = new Color(_material.color.r, _material.color.g, _material.color.b, 1); // 알파 값을 1로 올리면서 보이도록 변경
        }

        // 하이라이트를 끄는 함수
        public void HighLightOff()
        {
            _placedObjectType = ObjectType.None; // 배치 가능한 타입을 None으로 초기화
            _collider.enabled = false; // 클릭이 되지 않도록 콜라이더 비활성화
            _material.color = new Color(_material.color.r, _material.color.g, _material.color.b, 0); // 알파 값을 0으로 바꿔 보이지 않도록 변경
        }

        public abstract void ObjectClicked();
    }
}
// 마지막 작성 일자: 2025.07.15