using MyUtil.Interface;
using UnityEngine;

namespace MyUtil.GameMode
{
    // 작성자: 조혜찬
    // 튜토리얼 모드
    public class TutorialMode : IGameMode
    {
        public bool UseServer() => false; // 서버 사용 안함

        public bool IsTutorial() => true; // 튜토리얼
    }
}
// 마지막 작성 일자: 2026.03.17