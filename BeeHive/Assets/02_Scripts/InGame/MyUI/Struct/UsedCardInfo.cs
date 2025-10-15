using System;
using UnityEngine;

namespace InGame.MyUI
{
    // 작성자: 조혜찬
    // 사용한 카드의 정보를 가지는 구조체
    [Serializable]
    public struct UsedCardInfo
    {
        public string usedCardName; // 사용된 카드의 이름
        public string usedCardInformation; // 사용된 카드의 정보(효과)
    }
}
// 마지막 작성 일자: 2025.10.15