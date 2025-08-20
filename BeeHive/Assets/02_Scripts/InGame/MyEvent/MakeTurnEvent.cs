using System;
using UnityEngine;

namespace InGame.MyEvent
{
    // 작성자: 조혜찬
    // 생산 턴에 실행될 이벤트를 가지는 정적 클래스
    public static class MakeTurnEvent
    {
        public static Action OnMakeTurn; // 생산 턴에 실행할 액션
    }
}
// 마지막 작성 일자: 2025.08.20