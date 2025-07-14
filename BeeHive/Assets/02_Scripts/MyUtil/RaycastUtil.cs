using UnityEngine;

namespace MyUtil
{
    // 작성자: 조혜찬
    // 레이캐스트 관련 유틸 정적 클래스
    public static class RaycastUtil
    {
        // 마우스 레이캐스트 - 마우스로 객체를 클릭하기 위한 함수
        public static GameObject MouseRaycast(out RaycastHit hit, float maxDistance = 100, int layerMask = 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // 카메라 메인에서 화면 마우스 위치에서 쏘는 레이
            if(Physics.Raycast(ray, out hit, maxDistance, layerMask)) // 레이캐스트를 통해서 레이가 닿았다면
            {
                return hit.collider.gameObject; // 닿은 객체 반환
            }
            return null; // 레이캐스트를 통해서 레이가 닿지 않았다면 null을 반환
        }
    }
}
// 마지막 작성 일자: 2025.07.14