using InGame.MyEnum;
using System;
using System.Threading.Tasks;

namespace InGame.MyEvent
{
    // 작성자: 조혜찬
    // 경고 관련 이벤트들을 가지는 정적 클래스
    public static class WarningEvent
    {
        public static Func<TurnType, string, Task<bool>> OnCheckCurrentTurn; // 받은 턴과 현재 턴을 비교하는 델리게이트
        public static Func<int, string, Task<bool>> OnCanPayCost; // 비용을 지불할 수 있는지 확인하는 델리게이트
        public static Func<int, string, Task<bool>> OnCheckLeftPieceCount; // 남은 기물이 있는지 확인하는 델리게이트
        public static Func<Task<bool>> OnCheckCurrentTurnTeam; // 현재 턴의 팀을 확인하는 델리게이트
        public static Func<Task<bool>> OnCanMakePiece; // 기물 생성 가능 여부를 확인하는 델리게이트
        public static Func<ObjectType, bool, Task<bool>> OnCanMovePiece; // 특정 타입의 기물이 이동할 수 있는지 확인하는 델리게이트
    }
}
// 마지막 작성 일자: 2026.01.14