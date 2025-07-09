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

        // 클릭 시 실행될 함수
        public void OnUIButtonClick()
        {
            foreach(Transform trans in  _highLightPosList) // 리스트 안에 있는 값 순회
            {
                PlacePlaneObjectBase highLightObjectBase = trans.GetComponent<PlacePlaneObjectBase>(); // 하이라이트 스크립트 가져오기

                if (highLightObjectBase.PlacedObjectType != ObjectType.None) // 배치된 상태라면
                    continue; // 아래 코드 무시

                highLightObjectBase.HighLightOn(); // 배치할 수 있는 위치를 보여주는 하이라이트 오브젝트 활성화
            }
        }
    }
}
