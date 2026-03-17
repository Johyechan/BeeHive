using MyUtil.Interface;
using UnityEngine;

namespace MyUtil.GameMode
{
    // 작성자: 조혜찬
    // 멀티 플레이 모드
    public class MultiplayMode : IGameMode
    {
        public bool UseServer() => true; // 서버 사용함

        public bool IsTutorial() => false; // 멀티 플레이
    }
}
// 마지막 작성 일자: 2026.03.17