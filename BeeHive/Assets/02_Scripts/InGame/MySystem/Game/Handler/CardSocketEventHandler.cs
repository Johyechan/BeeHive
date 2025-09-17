using InGame.MyManager;
using InGame.MyManager.MyCard;
using UnityEngine;

namespace InGame.MySystem.Game.Handler
{
    // 작성자: 조혜찬
    // 카드 관련 소켓 이벤트 연결 핸들러 클래스
    public class CardSocketEventHandler : BaseSocketEventHandler
    {
        public override void OnConnect()
        {
            NetworkManager.Instance.Socket.On("setCard", async (data) =>
            {
                string json = data.GetValue().ToString(); // 문자열로 data 받기
                SetCardInfo setCardInfo = JsonUtility.FromJson<SetCardInfo>(json); // 카드 세팅에 필요한 값을 가지는 구조체로 값 받기
                await DrawManager.Instance.CardSetHandle.Setting(setCardInfo.targetTeam, setCardInfo.cardCount); // Task 반환 없이 바로 실행
            });
        }
    }
}
// 마지막 작성 일자: 2025.09.16