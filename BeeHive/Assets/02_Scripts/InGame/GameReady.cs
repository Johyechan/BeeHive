using MyUtil;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace InGame
{
    // 작성자: 조혜찬
    // 게임이 준비 여부 관리 정적 클래스
    public static class GameReady
    {
        public static readonly ReadyGate Gate = new();
    }
}
// 마지막 작성 일자: 2026.01.19