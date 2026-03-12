using System;
using UnityEngine;

namespace Tutorial.Struct
{
    // 작성자: 조혜찬
    // 튜토리얼 매니저가 Inspector 창에서 할당 받아야 하는 변수들을 가지는 구조체
    [Serializable] // Inspector 창에서 값을 받을 수 있게 직렬화
    public struct TutorialManagerData
    {
        public CanvasGroup tutorialOverlay; // 튜토리얼 UI

        public float animationDuration; // 애니메이션 지속시간
    }
}
// 마지막 작성 일자: 2026.03.12