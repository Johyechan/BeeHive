using MyUtil.MyEvent;
using UnityEngine;

namespace InGame.MyObject
{
    // 작성자: 조혜찬
    // 카드 배열을 관리하는 클래스
    public class CardArray : ObjectArrayBase
    {
        private void OnEnable()
        {
            DrawEventSystem.OnDraw += ObjectRePlace; // 드로우 이벤트에 구독
        }
    }
}
// 마지막 작성 일자: 2025.07.08