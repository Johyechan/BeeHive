using MyUtil.Interface;
using UnityEngine;

namespace MyUtil.GameMode
{
    // 작성자: 조혜찬
    // 게임 모드 매니저
    public class GameModeManager : MonoSingleton<GameModeManager>
    {
        private IGameMode _currentGameMode;

        public IGameMode CurrentGameMode 
        { 
            get
            {
                if (_currentGameMode == null)
                {
                    _currentGameMode = new MultiplayMode();
                }

                return _currentGameMode;
            }
        } // 현재 게임 모드 프로퍼티

        protected override void Awake()
        {
            base.Awake();

            Ready();
        }

        // 게임 모드 할당 함수
        public void SetMode(IGameMode gameMode)
        {
            _currentGameMode = gameMode;
        }
    }
}
// 마지막 작성 일자: 2026.04.08