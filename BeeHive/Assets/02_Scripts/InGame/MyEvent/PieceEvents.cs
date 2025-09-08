using InGame.MyEnum;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace InGame.MyEvent
{
    // 작성자: 조혜찬
    // 기물 관련 이벤트를 가지는 정적 클래스
    public static class PieceEvents
    {
        public static Func<int, TeamType, Transform, Task> OnGetRoad; // 도로를 가져오는 이벤트
        public static Action<Transform, TeamType> OnDestroyLeftRoad; // 사용하지 않은 도로를 삭제하는 이벤트
    }
}
// 마지막 작성 일자: 2025.09.05