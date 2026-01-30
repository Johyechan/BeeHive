using InGame.MyNetwork;
using MyUtil;
using SocketIOClient;
using Steamworks;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace InGame.MyManager
{
    // 작성자: 조혜찬
    // 서버와 연결하는 싱글톤 매니저 클래스
    public class NetworkManager : MonoSingleton<NetworkManager>
    {
        private SocketIOUnity _socket; // 유니티에서 Socket.IO 서버와 통신하기 위한 객체
        public SocketIOUnity Socket { get => _socket; } // 외부에서 _socket에 안전하게 접근 가능한 프로퍼티

        private string _currentClientName; // 현재 클라이언트 닉네임
        // 현재 클라이언트 닉네임
        public string CurrentClientName { get => _currentClientName; set => _currentClientName = value; }

        private string _currentPlayerID; // 현재 클라이언트 ID
        // 현재 클라이언트 ID 프로퍼티
        public string CurrentPlayerID { get => _currentPlayerID; }

        private bool _isSteamAPIInitSuccess = false; // 스팀 api Init 성공 여부

        private bool _isClientOver = false; // 클라이언트 종료 여부
        public bool IsClientOver { get => _isClientOver; } // 클라이언트 종료 여부 프로퍼티

        private RoomNetworkHandler _roomNetworkHandler; // 방과 관련된 서버 신호를 받는 핸들러

        private TaskCompletionSource<bool> _socketConnectedTcs; // 소켓 연결 여부 tcs

        // 소켓 연결 여부 대기 함수
        public Task WaitSocketConnected() => _socketConnectedTcs.Task;

        protected override void Awake()
        {
            base.Awake();

            _isSteamAPIInitSuccess = SteamAPI.Init(); // SteamAPI Init 성공 여부 할당

            _socketConnectedTcs = new TaskCompletionSource<bool>();

            _roomNetworkHandler = new RoomNetworkHandler(); // 핸들러 초기화

            var uri = new Uri("http://129.80.97.177:3000"); // 서버 주소 설정
            var options = new SocketIOOptions()
            {
                Transport = SocketIOClient.Transport.TransportProtocol.WebSocket // 통신 방식을 WebSocket으로 설정
            };
            _socket = new SocketIOUnity(uri, options); 

            _socket.Connect(); // 서버에 연결 시도

            _socket.OnConnected += (sender, e) =>
            {
                _socketConnectedTcs?.TrySetResult(true);
                // 현재 클라이언트 ID를 서버에서 받아온다
                _socket.On("myID", data =>
                {
                    if (_isClientOver) // 클라이언트가 종료 되었다면
                        return; // 반환

                    string id = data.GetValue<string>();
                    MainThreadDispatcher.Enqueue(() => _currentPlayerID = id); // 현재 클라이언트 ID 할당
                });

                // 오류 발생 시 오류 표기
                _socket.On("error", response =>
                {
                    if (_isClientOver) // 클라이언트가 종료 되었다면
                        return; // 반환

                    string text = response.GetValue<string>();
                    MainThreadDispatcher.Enqueue(() =>
                    {
                        UIManager.Instance.WarningUIMake(text);
                    });
                    return;
                });

                _roomNetworkHandler.Init(); // 방과 관련된 서버 신호를 받는 핸들러 초기화
            };
        }

        private void OnDisable()
        {
            _roomNetworkHandler.Disable();
            _socket.Off("myID");
            _socket.Off("error");
        }

        private void Update()
        {
            if (_isSteamAPIInitSuccess) // 스팀이 돌아가고 있으며, Init()이 성공 했을 때
            {
                SteamAPI.RunCallbacks(); // 스팀 클라이언트에서 발생한 이벤트들을 게임으로 전달
            }
            else // 스팀 Init() 실패라면
            {
                _socket.Emit("debug", "스팀 Init실패");
                Application.Quit(); // 어플리케이션 즉시 종료
            }
        }
        
        private void OnApplicationQuit()
        {
            _isClientOver = true; // 클라이언트 종료
            _socket.Dispose(); // 소켓 정리
            SteamAPI.Shutdown(); // 스팀과의 연결 정리
        }
    }
}
// 마지막 작성 일자: 2026.01.30