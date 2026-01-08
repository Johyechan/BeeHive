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
                NetworkManager.Instance.Socket.Emit("debug", "스팀이 안 돌아가고 있음 - SteamChecker");
                Application.Quit(); // 강제 종료
            }

            
            NetworkManager.Instance.Socket.Emit("debug", "스팀 API Init 성공 - SteamChecker");
            await Task.CompletedTask;
            return true;
        }
    }
}
// 마지막 작성 일자: 2026.01.07