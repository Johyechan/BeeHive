using InGame.MyEvent;
using InGame.MyManager.MyPlacePlane;
using InGame.MyObject.MyObjectEnum;
using UnityEngine;

namespace InGame.MyObject
{
    // 작성자: 조혜찬
    // 광부 기물 클래스
    public class Miner : PieceBase
    {
        private void Awake()
        {
            _parent = GameObject.Find("PlayerMiners").transform; // 광부 객체의 부모 변수 할당
        }

        public override void ObjectClicked()
        {
            // 클릭 되었을 때 이동 가능한 배치 칸 하이라이트 활성화
            if(!_isSelected) // 선택된 상태가 아닐 경우
            {
                HighLightEventSystem.CurrentCanPlaceType = ObjectType.None; // 배치 하는 것이 아닌 이동의 여부이기에 None으로 설정
                HighLightEventSystem.OnPieceHighLightUIAction?.Invoke(false, true); // 하이라이트 끄기, 배치 가능 배치 판 대상
                HighLightEventSystem.OnPieceHighLightObjAction?.Invoke(true, false); // 하이라이트 키기, 이동 가능 배치 판 대상
                _isSelected = true; // 선택 되었다고 할당
            }
            else // 선택된 상태일 경우
            {
                HighLightEventSystem.OnPieceHighLightObjAction?.Invoke(false, false); // 하이라이트 끄기, 이동 가능 배치 칸 대상
                _isSelected = false; // 선택 해제 되었다고 할당
            }
        }
    }
}
// 마지막 작성 일자: 2025.07.18