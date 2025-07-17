using InGame.MyManager.MyPlacePlane;
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
        [SerializeField] private ObjectType _objectType; // 배치 가능한 객체 타입 변수

        // 클릭 시 실행될 함수
        public void OnUIButtonClick()
        {
            foreach(var piece in PlacePlaneManager.Instance.HighLightHandlerProp.CanPiecePlacePlanesProp) // 배치 가능한 기물 칸들 순회
            {
                piece.CanPlacePieceTypeProp = _objectType; // 배치 가능한 타입을 설정
            }

            HighLightEventSystem.OnPieceHighLight?.Invoke(true); // 하이라이트 키기
        }
    }
}
// 마지막 작성 일자: 2025.07.17