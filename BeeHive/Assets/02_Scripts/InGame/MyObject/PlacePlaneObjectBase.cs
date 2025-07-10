using InGame.MyObject.MyObjectEnum;
using System.Collections.Generic;
using UnityEngine;

namespace InGame.MyObject
{
    // 작성자: 조혜찬
    // 배치 칸의 기능 클래스
    public class PlacePlaneObjectBase : MonoBehaviour
    {
        private Renderer _renderer; // 머티리얼을 들고 오기 위한 변수
        private Material _material; // 하이라이트 머티리얼 변수

        private ObjectType _placedObjectType; // 어떤 기물이 배치되어있는지 알기 위한 변수

        public ObjectType PlacedObjectType // 외부에서 어떤 기물이 배치되어있는지 알고, 어떤 기물을 배치한 것인지 설정하기 위한 프로퍼티
        {
            get
            {
                return _placedObjectType;
            }
            set
            {
                _placedObjectType = value;
            }
        }

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
            _material = _renderer.material; // 공용 머티리얼이 아닌 인스턴스화를 통한 개인 머티리얼을 가져옴
            _placedObjectType = ObjectType.None; // 아무것도 안 올려져 있는 상태로 초기화
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
    }
}
// 마지막 작성 일자: 2025.07.10