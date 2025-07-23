using InGame.MyObject.MyObjectInterface;
using MyUtil;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InGame.MyInput
{
    // 작성자: 조혜찬
    // 클릭 기능을 제공하는 핸들러 클래스
    public class InputClickHandler
    {
        private RaycastHit _mouseRaycastHit; // 마우스 레이를 쏴서 닿았을 때 정보를 저장할 변수

        private bool _isOverUI; // UI위에 마우스 커서가 있는지 확인할 변수
        public bool IsOverUI { get { return _isOverUI; } set { _isOverUI = value; } } // _isOverUI 프로퍼티

        // 마우스 클릭 시 호출되는 함수
        public GameObject OnMouseClick()
        {
            if (!_isOverUI) // 마우스 커서가 UI위에 있지 않다면
            {
                // 마우스 위치로 레이 캐스트 수행(hit, 레이 캐스트 거리, 레이어 마스크(ClickObj layer만 인식))
                GameObject clickedObj = RaycastUtil.MouseRaycast(out _mouseRaycastHit, 100, LayerMask.GetMask("ClickObj"));
                if (clickedObj != null) // 레이에 닿은 객체가 있다면
                {
                    return _mouseRaycastHit.collider.gameObject;
                }
            }

            return null;
        }
    }
}
// 마지막 작성 일자: 2025.07.23