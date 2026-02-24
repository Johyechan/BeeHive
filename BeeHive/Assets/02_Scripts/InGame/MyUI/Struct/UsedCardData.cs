using System;
using UnityEngine;

namespace InGame.MyUI
{
    // 작성자: 조혜찬
    // 사용한 카드의 정보를 가지는 구조체
    [Serializable]
    public class UsedCardData : MonoBehaviour
    {
        public string roomID; // 현재 방 ID
        public int usedCardType; // 사용된 카드의 풀 타입
    }
}
// 마지막 작성 일자: 2026.02.24