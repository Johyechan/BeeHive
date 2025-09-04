using InGame.MyEnum;
using System;

namespace InGame.MyEvent
{
    // 작성자: 조혜찬
    // 경고 관련 이벤트들을 가지는 정적 클래스
    public static class WarningEvent
    {
        public static Func<TurnType, string, bool> OnCheckCurrentTurn; // 받은 턴과 현재 턴을 비교하는 델리게이트
        public static Func<int, string, bool> OnCanPayCost; // 비용을 지불할 수 있는지 확인하는 델리게이트
        public static Func<int, string, bool> OnCheckLeftPieceCount; // 남은 기물이 있는지 확인하는 델리게이트
        public static Func<bool> OnCheckCurrentTurnTeam; // 현재 턴의 팀을 확인하는 델리게이트
    }
}
// 마지막 작성 일자: 2025.09.04