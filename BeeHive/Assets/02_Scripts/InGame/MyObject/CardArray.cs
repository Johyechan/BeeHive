using MyUtil.MyEvent;

namespace InGame.MyObject
{
    // 작성자: 조혜찬
    // 카드 배열을 관리하는 클래스
    public class CardArray : ObjectArrayBase
    {
        private void OnEnable()
        {
            DrawEventSystem.OnCardObjectSet += ObjectRePlace; // 카드 세팅 이벤트에 구독
        }

        private void OnDisable()
        {
            DrawEventSystem.OnCardObjectSet -= ObjectRePlace; // 카드 세팅 이벤트에서 구독 해제
        }
    }
}
// 마지막 작성 일자: 2025.08.28