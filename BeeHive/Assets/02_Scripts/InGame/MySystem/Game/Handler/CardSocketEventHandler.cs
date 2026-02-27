using DG.Tweening;
using InGame.MyManager.Global;
using InGame.MyManager.Local;
using InGame.MyObject;
using MyUtil;
using System.Threading.Tasks;
using UnityEngine;

namespace InGame.MySystem.Game.Handler
{
    // 작성자: 조혜찬
    // 카드 관련 소켓 이벤트 연결 핸들러 클래스
    public class CardSocketEventHandler : BaseSocketEventHandler
    {
        public override void OnConnect()
        {
            NetworkManager.Instance.Socket.On("setCard", (data) =>
            {
                if (NetworkManager.Instance.IsClientOver) // 클라이언트가 종료 되었다면
                    return; // 반환

                string json = data.GetValue().ToString(); // 문자열로 data 받기
                SetCardInfo setCardInfo = JsonUtility.FromJson<SetCardInfo>(json); // 카드 세팅에 필요한 값을 가지는 구조체로 값 받기

                MainThreadDispatcher.Enqueue(() =>
                {
                    _ = InGameContext.Current.Data.DrawManager.CardSetHandle.Setting(setCardInfo.targetTeam, setCardInfo.cardCount); // Task 반환 없이 바로 실행
                });
            });

            NetworkManager.Instance.Socket.On("cardReversed", (data) =>
            {
                if (NetworkManager.Instance.IsClientOver) // 클라이언트가 종료 되었다면
                    return; // 반환

                InGameContext.Current.Data.CardManager.CardReverseTask = new TaskCompletionSource<bool>();
                string json = data.GetValue().ToString(); // 문자열로 값 받기
                CardReverseInfo cardReverseInfo = JsonUtility.FromJson<CardReverseInfo>(json); // CarReverseInfo 형태로 Json을 변환

                MainThreadDispatcher.Enqueue(() =>
                {
                    GameObject cardObj = ObjectIdManager.Instance.FindObject(cardReverseInfo.cardID); // 뒤집힐 카드 객체 탐색
                    if (!cardObj) // 카드 객체를 못 찾은 경우
                    {
                        NetworkManager.Instance.Socket.Emit("debug", $"카드 객체 못 찾음");
                        return; // 반환
                    }
                    CardObject cardObject = cardObj.GetComponent<CardObject>();

                    _ = cardObj.transform.DORotate(new Vector3(0, cardObj.transform.eulerAngles.y, 90), cardReverseInfo.animationDuration / 2).AsyncWaitForCompletion(); // 카드 뒤집기
                    _ = cardObj.transform.DORotate(new Vector3(0, cardObj.transform.eulerAngles.y, 180), cardReverseInfo.animationDuration / 2).AsyncWaitForCompletion(); // 카드 뒤집기
                    _ = cardObj.transform.DOMoveY(0.0001f, cardReverseInfo.animationDuration).AsyncWaitForCompletion(); // 뒤집힌 카드가 땅을 뚫지 않게 조금 위로 이동
                    InGameContext.Current.Data.CardManager.CardReverseTask.SetResult(true);

                    UsedDeck usedDeck = GameObject.Find("UsedDeck").GetComponent<UsedDeck>();
                    usedDeck.AddCardInToUsedDeck(cardObj.transform);
                });
            });
        }

        public override void OnDisconnect()
        {
            NetworkManager.Instance.Socket.Off("setCard");
            NetworkManager.Instance.Socket.Off("cardReversed");
        }
    }
}
// 마지막 작성 일자: 2026.02.03