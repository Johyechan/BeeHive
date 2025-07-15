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
        private Renderer _renderer; // 머티리얼을 들고 오기 위한 변수
        private Material _material; // 하이라이트 머티리얼 변수

        private ObjectType _placedObjectType; // 어떤 기물이 배치되어있는지 알기 위한 변수
        public ObjectType PlacedObjectType { get { return _placedObjectType; } set { _placedObjectType = value; } } // 외부에서 어떤 기물이 배치되어있는지 알고, 어떤 기물을 배치한 것인지 설정하기 위한 프로퍼티
        private ObjectType _canPlacePiece; // 현재 배치가 가능한 기물 객체를 확인하는 변수
        public ObjectType CanPlacePiece { get { return _canPlacePiece; } set { _canPlacePiece = value; } } // 외부에서 현재 배치가 가능한 기물 객체를 설정할 프로퍼티

        protected virtual void Awake()
        {
            _renderer = GetComponent<Renderer>();
            _placedObjectType = ObjectType.None; // 아무것도 안 올려져 있는 상태로 초기화
        }

        protected virtual void Start()
        {
            _material = _renderer.material; // 공용 머티리얼이 아닌 인스턴스화를 통한 개인 머티리얼을 가져옴, Start에서 실행하는 이유는 다 생성이된 후 가져와서 특정 객체가 못 가져오는 상황을 방지하기 위해서
        }

        // 하이라이트를 키는 함수
        public void HighLightOn()
        {
            _material.color = new Color(_material.color.r, _material.color.g, _material.color.b, 1); // 알파 값을 1로 올리면서 보이도록 변경
        }

        // 하이라이트를 끄는 함수
        public void HighLightOff()
        {
            _material.color = new Color(_material.color.r, _material.color.g, _material.color.b, 0); // 알파 값을 0으로 바꿔 보이지 않도록 변경
        }

        public abstract void ObjectClicked();
    }
}
// 마지막 작성 일자: 2025.07.15