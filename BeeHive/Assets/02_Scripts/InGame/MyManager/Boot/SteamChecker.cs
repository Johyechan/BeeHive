using Steamworks;
using System.Threading.Tasks;
using UnityEngine;

namespace InGame.MyManager.Boot
{
    // 작성자: 조혜찬
    // 스팀 관련 검증 클래스
    public class SteamChecker : CheckerBase
    {
        protected override async Task<bool> Check()
        {
            if(!SteamAPI.Init())
            {
                Debug.Log("SteamAPI Init 실패");
                Application.Quit(); // 강제 종료
            }

            await Task.CompletedTask;
            return true;
        }
    }
}
// 마지막 작성 일자: 2026.01.06