using UnityEngine;

namespace InGame.MySystem.Game.Handler
{
    // 작성자: 조혜찬
    // 소켓 핸들러의 기본 클래스
    public abstract class BaseSocketEventHandler
    {
        // 소켓 이벤트 연결 함수
        public abstract void OnConnect();

        // 소켓 이벤트 연결 해제 함수
        public abstract void OnDisconnect();
    }
}
// 마지막 작성 일자: 2026.01.30