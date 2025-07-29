using InGame.MyObject.MyObjectEnum;
using Mirror;
using UnityEngine;

namespace InGame.MyManager
{
    // 작성자: 조혜찬
    // 서버 싱글톤 매니저
    public class MyNetworkManager : NetworkManager
    {
        // 클라이언트가 처음 서버에 진입했을 때 불리는 함수
        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            base.OnServerAddPlayer(conn);

            var teamManagerObj = conn.identity; // 연결된 클라이언트 객체 할당
            if (teamManagerObj.TryGetComponent(out TeamManager teamManager)) // 클라이언트 객체에서 TeamManager를 가져올 수 있는지 확인
            {
                teamManager.SetTeam(); // 팀 배정
            }
        }
    }
}
// 마지막 작성 일자: 2025.07.28