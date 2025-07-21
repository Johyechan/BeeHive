using InGame.MyObject.MyObjectEnum;
using System;

namespace InGame.MyEvent
{
    // 작성자: 조혜찬
    // 하이라이트를 활성화 여부를 결정할 이벤트를 가지는 정적 클래스
    public static class HighLightEventSystem
    {
        private static ObjectType _currentCanPlaceType = ObjectType.None; // 기물 칸에 배치 가능한 타입 확인 변수 - 현재 배치 가능한 기물이 다르다면 자기 자신을 누른 것이 아닌 다른 것을 눌렀다는 것이기 때문에 기물 하이라이트는 끄지 않지만 배치 가능한 타입은 변경이 가능, 처음 상태는 None으로 초기화
        public static ObjectType CurrentCanPlaceType { get { return _currentCanPlaceType; } set { _currentCanPlaceType = value; } }

        public static Action<bool> OnPieceHighLight; // 하이라이트를 키거나 끌 때 불릴 이벤트
        public static Action<bool> OnRoadHighLight; // 하이라이트를 키거나 끌 때 불릴 이벤트
    }

    // 마지막 작성 일자: 2025.07.21
}
