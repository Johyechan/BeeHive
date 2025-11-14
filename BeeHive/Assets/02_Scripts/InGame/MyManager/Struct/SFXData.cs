using System;
using UnityEngine;

namespace InGame.MyManager
{
    // 작성자: 조혜찬
    // 효과음 값 가지는 구조체
    [Serializable]
    public struct SFXData
    {
        public SFXType sfxType; // 효과음 타입
        public AudioSource audioSource; // 효과음
    }
}
// 마지막 작성 일자: 2025.11.14