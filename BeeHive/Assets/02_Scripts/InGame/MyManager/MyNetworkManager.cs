using InGame.MyObject.MyObjectEnum;
using Mirror;
using UnityEngine;

namespace InGame.MyManager
{
    // 작성자: 조혜찬
    // 서버 싱글톤 매니저
    public class MyNetworkManager : NetworkManager
    {
        private int _currentTeam = 1; // 현재 팀 타입
        private int _maxTeam; // 최대 팀 타입

        // 클라이언트가 처음 서버에 진입했을 때 불리는 함수
        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            base.OnServerAddPlayer(conn);

            // TeamManager를 활용하는 것으로 변경
            //var gameObject = conn.identity
            
            if(GameManager.Instance.PlayerCount == 2) // 2인용 대전일 때
            {
                _maxTeam = 2; // 최대 팀을 2로 지정
            }
            else if(GameManager.Instance.PlayerCount == 3) // 3인용 대전일 때
            {
                _maxTeam = 3; // 최대 팀을 3으로 지정
            }

            GameManager.Instance.TeamType = (TeamType)_currentTeam++; // 현재 팀 할당 이후 1 증가

            if(_currentTeam > _maxTeam) // 만약 최대 팀보다 현재 팀이 크다면
            {
                _currentTeam = 1; // 현재 팀을 1로 할당
            }
        }
    }
}
// 마지막 작성 일자: 2025.07.28