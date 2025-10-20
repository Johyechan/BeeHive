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
                string json = value.GetValue().ToString();
                CastleHpChangeInfo castleHpChangeInfo = JsonUtility.FromJson<CastleHpChangeInfo>(json); // Json 값 변환

                TeamType hpChangedCastleTeamType = (TeamType)castleHpChangeInfo.changeTeamType; // 서버에서 받은 int형식 변수를 TeamType enum 값으로 변경
                NetworkManager.Instance.Socket.Emit("debug", $"서버로: {hpChangedCastleTeamType.ToString()}");
                Castle hpChangedCastle = TeamManager.Instance.GetCastle(hpChangedCastleTeamType); // 체력이 올라간 팀에 맞는 성 가져오기
                hpChangedCastle.CastleUpgrade(castleHpChangeInfo.changedHp); // 체력 증가
            });
        }
    }
}
// 마지막 작성 일자: 2025.10.20