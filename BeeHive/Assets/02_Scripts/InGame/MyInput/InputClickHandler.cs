using InGame.MyObject;
using MyUtil;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputClickHandler
{
    private RaycastHit _mouseRaycastHit; // 마우스 레이를 쐈을 때 닿았을 때 정보를 저장할 변수

    private bool _isOverUI; // UI위에 마우스 커서가 있는지 확인할 변수
    public bool IsOverUI { get { return _isOverUI; } set { _isOverUI = value; } } // _isOverUI 프로퍼티

    // 마우스 좌클릭 시 실행될 함수
    public void OnMouseClick(InputAction.CallbackContext context)
    {
        if (!_isOverUI) // 마우스 커서가 UI위에 있지 않다면
        {
            GameObject clickedObj = RaycastUtil.MouseRaycast(out _mouseRaycastHit, 100, LayerMask.GetMask("ClickObj")); // 레이를 쏘기
            if (clickedObj != null) // 레이에 닿은 객체가 있다면
            {
                PlacePlaneObjectBase placePlaneObjectBase = clickedObj.GetComponent<PlacePlaneObjectBase>(); // PlacePlaneObjectBase를 가져오기
                placePlaneObjectBase.ObjectClicked(); // PlacePlaneObjectBase에 있는 인터페이스 함수를 실행 - 기물을 옮기는 함수 실행
            }
        }
    }
}
