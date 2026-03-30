using InGame.MyObject;
using System;
using UnityEngine;

namespace Tutorial.Struct
{
    // 작성자: 조혜찬
    // 튜토리얼을 위해 미리 할당한 카드 객체 정보 구조체
    [Serializable]
    public struct TutorialCardObjectData
    {
        public CardObject cardObj; // 카드 객체
        public int id; // UI 카드와 매칭을 위한 ID
    }
}
// 마지막 작성 일자: 2026.03.30