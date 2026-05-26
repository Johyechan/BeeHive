using MyUtil.Interface;
using Steamworks;
using UnityEngine;

namespace MyUtil.GameMode
{
    // 작성자: 조혜찬
    // 게임 모드 매니저
    public class GameModeManager : MonoSingleton<GameModeManager>
    {
        private const uint MAIN_APP_ID = 4317470;
        private const uint TEST_APP_ID = 480;
        private const uint FRIENDPASS_APP_ID = 4778260;

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

        private LicenseType _currentLicenseType = LicenseType.None; // 현재 권한 타입
        public LicenseType CurrentLicenseType { get => _currentLicenseType; } // 현재 권한 타입 프로퍼티

        protected override void Awake()
        {
            base.Awake();

            switch(SteamUtils.GetAppID().m_AppId)
            {
                case MAIN_APP_ID:
                    _currentLicenseType = LicenseType.Main;
                    break;
                case FRIENDPASS_APP_ID:
                    _currentLicenseType = LicenseType.FriendPass;
                    break;
                case TEST_APP_ID:
                    _currentLicenseType = LicenseType.Main;
                    break;
            }

            Ready();
        }

        // 게임 모드 할당 함수
        public void SetMode(IGameMode gameMode)
        {
            _currentGameMode = gameMode;
        }
    }
}
// 마지막 작성 일자: 2026.05.26