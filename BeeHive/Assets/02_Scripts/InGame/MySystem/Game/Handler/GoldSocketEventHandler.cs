using InGame.MyManager;
using MyUtil;
using UnityEngine;

namespace InGame.MySystem.Game.Handler
{
    // 작성자: 조혜찬
    // 금 관련 소켓 이벤트 연결 핸들러 클래스
    public class GoldSocketEventHandler : BaseSocketEventHandler
    {
        private GoldSetHandle _goldSetHandle; // 금 관련 세팅 핸들러

        // 생성자(금 관련 세팅 핸들러)
        public GoldSocketEventHandler(GoldSetHandle goldSetHandle)
        {
            _goldSetHandle = goldSetHandle;
        }

        public override void OnConnect()
        {
            NetworkManager.Instance.Socket.On("goldSet", data =>
            {
                string json = data.GetValue().ToString(); // 문자열로 data 받기
                SetGoldInfo setGoldInfo = JsonUtility.FromJson<SetGoldInfo>(json); // SetGoldInfo 구조체로 값 받기
                MainThreadDispatcher.Enqueue(() =>
                {
                    _ = _goldSetHandle.Setting(setGoldInfo.team, setGoldInfo.goldCoin, setGoldInfo.goldBar); // 금화 및 금괴 객체 세팅(팀, 금화 개수, 금괴 개수)
                });
            });
        }
    }
}
// 마지막 작성 일자: 2026.01.15