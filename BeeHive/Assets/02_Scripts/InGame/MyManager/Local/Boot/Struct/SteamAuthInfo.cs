using System;

namespace InGame.MyManager.Boot.Struct
{
    [Serializable] // 직렬화
    public struct SteamAuthInfo
    {
        public string ticket; // 스팀에게서 받은 인증 티켓
        public uint appID; // 앱 ID
    }
}
// 마지막 작성 일자: 2026.05.26