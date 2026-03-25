using InGame.MyEnum;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Tutorial.Event
{
    // 작성자: 조혜찬
    // 튜토리얼에 필요한 이벤트들을 가지는 클래스
    public static class TutorialEvents
    {
        public static Action OnIntroEnd; // 인트로 종료 이벤트
        public static Action OnTurnEnd; // 턴 종료 이벤트
        public static Func<Task> OnTutorialDraw; // 튜토리얼 드로우 이벤트
    }
}
// 마지막 작성 일자: 2026.03.25