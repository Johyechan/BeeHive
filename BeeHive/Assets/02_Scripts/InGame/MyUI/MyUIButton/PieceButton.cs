using InGame.MyObject;
using InGame.MyObject.MyObjectEnum;
using InGame.MyUI.MyUIInterface;
using System.Collections.Generic;
using UnityEngine;

namespace InGame.MyUI.MyUIButton
{
    // 작성자: 조혜찬
    // 기물 UI 버튼 클래스
    public class PieceButton : MonoBehaviour, IUIButton
    {
        [SerializeField] private List<Transform> _highLightPosList; // 배치할 수 있는 위치들을 가지는 리스트

        private bool _isHighLightOn; // 배치 칸의 하이라이트가 켜져있는지 확인하는 변수

        private void Awake()
        {
            _isHighLightOn = false; // 꺼져있는 상태로 초기화
        }

        // 클릭 시 실행될 함수
        public void OnUIButtonClick()
        {
            foreach(Transform trans in  _highLightPosList) // 리스트 안에 있는 값 순회
            {
                PlacePlaneObjectBase highLightObjectBase = trans.GetComponent<PlacePlaneObjectBase>(); // 하이라이트 스크립트 가져오기

                if (highLightObjectBase.PlacedObjectType != ObjectType.None) // 배치된 상태라면
                {
                    highLightObjectBase.HighLightOff();
                    continue; // 아래 코드 무시
                }

                if (!_isHighLightOn) // 배치 칸에 하이라이트가 켜져있다면
                {
                    highLightObjectBase.HighLightOn(); // 배치할 수 있는 위치를 보여주는 하이라이트 오브젝트 활성화
                }
                else // 아니라면
                {
                    highLightObjectBase.HighLightOff(); // 배치할 수 있는 위치를 보여주는 하이라이트 오브젝트 활성화
                }
            }

            _isHighLightOn = _isHighLightOn == false ? true : false; // 하이라이트가 꺼져있다면 true로 켜져있다면 false로 변경
        }
    }
}
// 마지막 작성 일자: 2025.07.10