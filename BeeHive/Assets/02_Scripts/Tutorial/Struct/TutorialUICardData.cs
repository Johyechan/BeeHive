using InGame.MyUI.Card;
using System;
using UnityEngine;

namespace Tutorial.Struct
{
    // 작성자: 조혜찬
    // 튜토리얼을 위해 미리 할당한 UI 카드 정보 구조체
    [Serializable]
    public struct TutorialUICardData
    {
        public UICardBase uiCard; // UI 카드
        public int id; // 카드 객체와 매칭을 위한 ID
    }
}
// 마지막 작성 일자: 2026.03.30