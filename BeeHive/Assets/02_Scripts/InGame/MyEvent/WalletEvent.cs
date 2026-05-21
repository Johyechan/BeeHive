using InGame.MyEnum;
using System;
using System.Threading.Tasks;

namespace InGame.MyEvent
{
    // 작성자: 조혜찬
    // 지갑 관련(금화 및 금회 획득, 사용) 이벤트를 가지는 정적 클래스
    public static class WalletEvent
    {
        public static Action<int> OnGetGoldCoin; // 금화 획득 이벤트
        public static Action<int, bool> OnGetGoldBar; // 금괴 획득 이벤트
        public static Action<TaskCompletionSource<bool>> OnSetGold; // 금화 및 금괴의 객체와 UI 세팅 이벤트
        public static Func<int, bool> OnUseGoldBar; // 금괴 사용 이벤트
        public static Func<int, bool> OnCanUseGoldBar; // 금괴 사용 가능 여부 확인 이벤트
        public static Func<int> OnTeam1MinerDigValue; // 광부가 버는 금화 양을 확인하는 이벤트
        public static Func<int> OnTeam2MinerDigValue; // 광부가 버는 금화 양을 확인하는 이벤트
    }
}
// 마지막 작성 일자: 2026.05.21