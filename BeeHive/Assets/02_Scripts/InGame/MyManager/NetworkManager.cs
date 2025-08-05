using InGame.MyNetwork;
using MyUtil;
using SocketIOClient;
using System;
using System.Net.Sockets;
using UnityEngine;

namespace InGame.MyManager
{
    // 작성자: 조혜찬
    // 서버와 연결하는 싱글톤 매니저 클래스
    public class NetworkManager : MonoSingleton<NetworkManager>
    {
        private SocketIOUnity _socket; // 유니티에서 Socket.IO 서버와 통신하기 위한 객체
        public SocketIOUnity Socket { get => _socket; } // 외부에서 _socket에 안전하게 접근 가능한 프로퍼티

        private RoomNetworkHandler _roomNetworkHandler; // 방과 관련된 서버 신호를 받는 핸들러

        protected override void Awake()
        {
            base.Awake();

            _roomNetworkHandler = new RoomNetworkHandler(); // 핸들러 초기화

            var uri = new Uri("http://localhost:3000"); // 서버 주소 설정
            var options = new SocketIOOptions()
            {
                Transport = SocketIOClient.Transport.TransportProtocol.WebSocket // 통신 방식을 WebSocket으로 설정
            };
            _socket = new SocketIOUnity(uri, options); 

            _socket.Connect(); // 서버에 연결 시도

            _socket.OnConnected += (sender, e) =>
            {
                // 오류 발생 시 오류 표기
                _socket.On("error", response =>
                {
                    Debug.Log(response.GetValue<string>());
                    return;
                });

                _roomNetworkHandler.Init(); // 방과 관련된 서버 신호를 받는 핸들러 초기화
            };
        }
    }
}
// 마지막 작성 일자: 2025.08.05