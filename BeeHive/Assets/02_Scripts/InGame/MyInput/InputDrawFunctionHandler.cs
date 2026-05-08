using InGame.MyManager.Local;
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
            await InGameContext.Current.Data.DrawManager.DrawCard(deckTransform, playerCardsParent, playerUICardsParent); // 카드 드로우 실행
        }
    }
}
// 마지막 작성 일자: 2026.05.08