using InGame.MyEnum;
using System;

namespace InGame.MyEvent
{
    // 작성자: 조혜찬
    // 하이라이트를 관련 이벤트를 제공하는 정적 클래스
    public static class HighLightEvents
    {
        // 현재 선택된 배치 속성을 저장하는 변수
        private static ObjectType _selectedPlacementType = ObjectType.None; 
        // 위에 있는 변수를 외부에서 접근 및 수정을 하기 위해 존재하는 프로퍼티
        public static ObjectType SelectedPlacementType 
        { 
            get => _selectedPlacementType;
            set => _selectedPlacementType = value;
        }

        // 기물 배치 가능 칸의 하이라이트 이벤트 (bool: 하이라이트 활성화 여부, bool: 배치용도인지 이동 용도인지 여부)
        public static Action<bool, bool> OnPiecePlacementHighLight;
        // 도로 배치 가능 칸의 하이라이트 이벤트 (bool: 하이라이트 활성화 여부)
        public static Action<bool> OnRoadPlacementHighLight;
        // 기물 이동 가능 칸의 하이라이트 이벤트 (bool: 하이라이트 활성화 여부, bool: 배치용도인지 이동 용도인지 여부)
        public static Action<bool, bool> OnPieceMovementHighLight;
    }
}
// 마지막 작성 일자: 2025.07.23