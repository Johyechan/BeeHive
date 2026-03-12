using MyUtil.Interface;
using UnityEngine;

namespace MyUtil.GameMode
{
    // 작성자: 조혜찬
    // 게임 모드 매니저
    public class GameModeManager : MonoSingleton<GameModeManager>
    {
        public IGameMode CurrentGameMode { get; private set; } // 현재 게임 모드 프로퍼티

        // 게임 모드 할당 함수
        public void SetMode(IGameMode gameMode)
        {
            CurrentGameMode = gameMode;
        }
    }
}
// 마지막 작성 일자: 2026.03.12