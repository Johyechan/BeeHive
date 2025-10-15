using DG.Tweening;
using InGame.MyManager;
using InGame.MyManager.MyCard;
using InGame.MyObject;
using InGame.MyUI.Card;
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

            NetworkManager.Instance.Socket.On("cardReversed", async (data) =>
            {
                string json = data.GetValue().ToString(); // 문자열로 값 받기

                CardReverseInfo cardReverseInfo = JsonUtility.FromJson<CardReverseInfo>(json); // CarReverseInfo 형태로 Json을 변환

                GameObject cardObj = ObjectIdManager.Instance.FindObject(cardReverseInfo.cardID); // 뒤집힐 카드 객체 탐색

                NetworkManager.Instance.Socket.Emit("debug", $"카드 객체{cardObj}");

                await cardObj.transform.DORotate(new Vector3(0, cardObj.transform.eulerAngles.y, 90), cardReverseInfo.animationDuration / 2).AsyncWaitForCompletion(); // 카드 뒤집기
                await cardObj.transform.DORotate(new Vector3(0, cardObj.transform.eulerAngles.y, 180), cardReverseInfo.animationDuration / 2).AsyncWaitForCompletion(); // 카드 뒤집기

                await cardObj.transform.DOMoveY(0.0001f, cardReverseInfo.animationDuration).AsyncWaitForCompletion(); // 뒤집힌 카드가 땅을 뚫지 않게 조금 위로 이동

                UsedDeck usedDeck = GameObject.Find("UsedDeck").GetComponent<UsedDeck>();
                usedDeck.AddCardInToUsedDeck(cardObj.transform);
            });
        }
    }
}
// 마지막 작성 일자: 2025.10.15