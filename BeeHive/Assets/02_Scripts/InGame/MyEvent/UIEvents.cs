using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InGame.MyEvent
{
    // 작성자: 조혜찬
    // UI 관련 이벤트를 가지는 정적 클래스
    public static class UIEvents
    {
        public static Action OnSetLeftPieceText; // 남은 기물 수를 Text로 보여주는 이벤트
        public static Action OnShowUICardInformation; // UI 카드의 정보를 보여주는 이벤트
    }
}
// 마지막 작성 일자: 2025.10.02