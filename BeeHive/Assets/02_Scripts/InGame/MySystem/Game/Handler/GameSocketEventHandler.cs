using InGame.MyManager;
using InGame.MyManager.MyPiece;
using UnityEngine;

namespace InGame.MySystem.Game.Handler
{
    // 작성자: 조혜찬
    // 게임 관련 소켓 이벤트 연결 핸들러 클래스
    public class GameSocketEventHandler : BaseSocketEventHandler
    {
        public override void OnConnect()
        {
            NetworkManager.Instance.Socket.On("drought", (value) =>
            {
                int isDrought = value.GetValue<int>();
                PieceManager.Instance.IsDrought = isDrought == 1; // 가뭄 여부 변경 - isDrought가 1일 경우 참, 1이 아닐 경우 거짓 할당
            });
        }
    }
}
// 마지막 작성 일자: 2025.09.29