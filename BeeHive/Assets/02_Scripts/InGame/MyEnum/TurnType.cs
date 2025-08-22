namespace InGame.MyEnum
{
    // 작성자: 조혜찬
    // 턴 enum
    public enum TurnType
    {
        MakeTurn = 0, // 생산 턴
        DrawTurn = 1, // 카드 뽑기 결정 턴
        MainTurn = 2, // 생성, 이동 턴
        TurnEnd = 3, // 턴 종료
        ChangeTeam = 4 // 팀 변경
    }
}
// 마지막 작성 일자: 2025.08.22