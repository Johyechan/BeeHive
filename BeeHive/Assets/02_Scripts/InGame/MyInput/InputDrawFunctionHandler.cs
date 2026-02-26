using DG.Tweening;
using InGame.MyManager.Global;
using InGame.MyManager.Local;
using MyUtil.MyEvent;
using System.Threading.Tasks;
using UnityEngine;

namespace InGame.MyInput
{
    // 작성자: 조혜찬
    // 실질적인 드로우 기능을 수행하는 핸들러
    public class InputDrawFunctionHandler
    {
        public async Task DrawFunction(Transform deckTransform, Transform playerCardsParent, RectTransform playerUICardsParent)
        {
            NetworkManager.Instance.Socket.Emit("debug", "드로우 함수 들어옴");
            InGameContext.Current.Data.DrawManager.DrawCard(deckTransform, playerCardsParent, playerUICardsParent); // 카드 드로우 실행

            Sequence seq = DOTween.Sequence()
                  .AppendCallback(() => DrawEventSystem.OnCardUISet?.Invoke())
                  .JoinCallback(() => DrawEventSystem.OnCardObjectSet?.Invoke(playerCardsParent));// 드로우 이벤트 인보크 후 시퀀스 완료

            await seq.AsyncWaitForCompletion(); // Task 완료 반환 대기
        }
    }
}
// 마지막 작성 일자: 2026.02.03