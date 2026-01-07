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
            if(!SteamAPI.IsSteamRunning()) // 스팀이 돌아가고 있지 않는다면
            {
                Debug.Log("Steam이 안 돌아가고 있어서 실패");
                Application.Quit(); // 강제 종료
            }

            if(!SteamAPI.Init())
            {
                Debug.Log("SteamAPI Init 실패");
                Application.Quit(); // 강제 종료
            }

            NetworkManager.Instance.IsSteamAPIInitSuccess(true); // SteamAPI Init 성공 할당
            await Task.CompletedTask;
            return true;
        }
    }
}
// 마지막 작성 일자: 2026.01.07