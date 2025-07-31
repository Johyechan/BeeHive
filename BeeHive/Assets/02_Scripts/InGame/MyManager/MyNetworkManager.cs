using Mirror;
using TMPro;
using UnityEngine;

namespace InGame.MyManager
{
    // 작성자: 조혜찬
    // 서버 싱글톤 매니저
    public class MyNetworkManager : NetworkManager
    {
        [SerializeField] private TMP_Text _log; // 임시 로그

        private string _mainServerIP; // 중앙 서버 IP
        // 중앙 서버 IP 프로퍼티
        public string MainServerIP { get => _mainServerIP; }

        private string _roomServerIP; // 방 서버 IP
        // 방 서버 IP 프로퍼티
        public string RoomServerIP { get => _roomServerIP; }

        public override void Awake()
        {
            base.Awake();
            _mainServerIP = "172.30.1.7"; // 중앙 서버 IP 할당
            _log.text = _mainServerIP;
            singleton.networkAddress = _mainServerIP; // NetworkManager의 기본 네트워크 주소를 중앙 서버 IP로 할당
            singleton.StartClient(); // 클라이언트 모드로 네트워크 시작, 서버에 연결 시도

            NetworkClient.RegisterHandler<SearchRoomResponseMessage>(OnReceiveSearchRoomRespones); // 서버로부터 SearchRoomReponseMessage 형식의 메세지를 받는 핸들러 초기화
        }

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

        private void OnReceiveSearchRoomRespones(SearchRoomResponseMessage msg)
        {
            _roomServerIP = msg.ip; // 방 서버 IP를 할당
            _log.text = "방 IP 찾음" + msg.ip;
        }
    }
}
// 마지막 작성 일자: 2025.07.28