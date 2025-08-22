using System;
using UnityEngine;

namespace InGame.MyEvent
{
    // 작성자: 조혜찬
    // 턴 넘기기 버튼 활성화 여부 이벤트를 가지는 정적 클래스
    public static class TurnChangeButtonEvent
    {
        public static Action<bool> OnSetInteractable; // 버튼 활성화 여부 액션
    }
}
// 마지막 작성 일자: 2025.08.22