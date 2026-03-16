using System;
using TMPro;
using UnityEngine;

namespace Tutorial.Struct
{
    // 작성자: 조혜찬
    // 튜토리얼 매니저가 Inspector 창에서 할당 받아야 하는 변수들을 가지는 구조체
    [Serializable] // Inspector 창에서 값을 받을 수 있게 직렬화
    public struct TutorialManagerData
    {
        public CanvasGroup tutorialDimmer; // 튜토리얼 UI (클릭 가능한 대상을 알려주는 UI)
        public CanvasGroup tutorialBlockPanel; // 튜토리얼 UI (클릭을 완전히 방지하는 UI)

        public TMP_Text guideTxt; // 안내문

        public Material dimmerMat; // 튜토리얼 UI (클릭 가능한 대상을 알려주는 UI) 머티리얼

        public float animationDuration; // 애니메이션 지속시간
        public float inputDelay; // 인풋 딜레이
    }
}
// 마지막 작성 일자: 2026.03.16