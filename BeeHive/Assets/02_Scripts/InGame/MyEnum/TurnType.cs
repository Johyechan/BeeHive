namespace InGame.MyEnum
{
    // 작성자: 조혜찬
    // 턴 enum
    public enum TurnType
    {
        MakeTurn, // 생산 턴
        DrawTurn, // 카드 뽑기 결정 턴
        MainTurn, // 생성, 이동 턴
        TurnEnd, // 턴 종료
        ChangeTeam // 팀 변경
    }
}

