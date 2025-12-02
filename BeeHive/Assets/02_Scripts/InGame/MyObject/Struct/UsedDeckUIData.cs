using System;
using TMPro;
using UnityEngine;

namespace InGame.MyObject
{
    // 작성자: 조혜찬
    // 사용한 카드 덱 UI 변수를 가지는 구조체
    [Serializable]
    public struct UsedDeckUIData
    {
        public CanvasGroup usedDeckCanvasGroup;
        public TMP_Text castleUpgradeCardCount; // 사용된 성벽 강화 카드 수
        public TMP_Text roadChangeCardCount; // 사용된 도로 변형 카드 수
        public TMP_Text droughtCardCount; // 사용된 가뭄 카드 수
        public TMP_Text goodHarvestCardCount; // 사용된 풍년 카드 수
        public TMP_Text firePowerCardCount; // 사용된 화력 카드 수
    }
}
//마지막 작성 일자: 2025.12.02