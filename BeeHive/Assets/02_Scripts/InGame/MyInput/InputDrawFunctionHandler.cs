using DG.Tweening;
using InGame.MyManager.MyCard;
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
            await DrawManager.Instance.DrawCard(deckTransform, playerCardsParent, playerUICardsParent); // 카드 드로우 실행

            Sequence seq = DOTween.Sequence()
                  .AppendCallback(() => DrawEventSystem.OnCardUISet?.Invoke())
                  .JoinCallback(() => DrawEventSystem.OnCardObjectSet?.Invoke(playerCardsParent));// 드로우 이벤트 인보크 후 시퀀스 완료

            await seq.AsyncWaitForCompletion(); // Task 완료 반환 대기
        }
    }
}
