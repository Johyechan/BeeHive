using InGame.MyEnum;
using InGame.MyManager;
using InGame.MyManager.MyPiece;
using InGame.MyObject;
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

            NetworkManager.Instance.Socket.On("castleHpChanged", (value) =>
            {
                int teamType = value.GetValue<int>();
                TeamType hpChangedCastleTeamType = (TeamType)teamType; // 서버에서 받은 int형식 변수를 TeamType enum 값으로 변경
                Castle hpChangedCastle = TeamManager.Instance.GetCastle(hpChangedCastleTeamType); // 최대 체력이 올라간 팀에 맞는 성 가져오기
                hpChangedCastle.HP++; // 최대 체력 증가
            });
        }
    }
}
// 마지막 작성 일자: 2025.10.01