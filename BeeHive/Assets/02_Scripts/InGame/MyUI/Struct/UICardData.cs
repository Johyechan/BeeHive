using MyUtil.MyObjectPool;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.MyUI
{
    // 작성자: 조혜찬
    // Inspactar 창에서 할당해야하는 변수들을 가지는 구조체
    [Serializable]
    public struct UICardData
    {
        public string currentCardName; // 현재 카드 이름

        public Image currentCardImage; // 현재 카드 이미지

        public ObjectPoolType poolType; // 카드의 풀 타입

        public float animationDuration; // 애니메이션 시간
        public float animationYValue; // y축으로 올라가는 값
    }
}
// 마지막 작성 일자: 2026.02.24