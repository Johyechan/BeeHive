using System;
using UnityEngine;

namespace MyUtil.MyEvent
{
    // 작성자: 조혜찬
    // 드로우 이벤트를 가지는 정적 클래스
    public static class DrawEventSystem
    {
        public static Action OnCardUISet; // 드로우 액션 - 드로우 했을 때 실행되야 할 함수들이 구독하는 이벤트
        public static Action<Transform> OnCardObjectSet; // 카드 세팅 액션 - 여러 플레이어의 카드들을 세팅할 함수들이 구독하는 이벤트
    }
}
// 마지막 작성 일자: 2025.08.28