using System;
using UnityEngine;

namespace InGame.MyObject
{
    // 작성자: 조혜찬
    // UsedDeck에서 사용할 변수를 가지는 구조체
    [Serializable]
    public struct UsedDeckData
    {
        public float animationDuration; // 애니메이션 지속시간
        public float cardShuffleDuration; // 셔플 시간
        public float uiFadeDuration; // ui 활성화 시간
        public float yInterval; // y축 간격
        public float shuffleMinYPos; // 셔플 y 위치

        public int shuffleCount; // 셔플 횟수

        public CanvasGroup deckShuffleAnimationUI; // 덱 셔플 애니메이션 UI
        
        public RectTransform uiCardDeck; // ui 덱
        
        public Deck deck; // 덱
    }
}
// 마지막 작성 일자: 2025.12.03