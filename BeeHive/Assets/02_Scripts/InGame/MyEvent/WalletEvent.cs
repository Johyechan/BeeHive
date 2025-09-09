using System;
using System.Threading.Tasks;

namespace InGame.MyEvent
{
    // 작성자: 조혜찬
    // 지갑 관련(금화 및 금회 획득, 사용) 이벤트를 가지는 정적 클래스
    public static class WalletEvent
    {
        public static Func<int, Task> OnGetGoldCoin; // 금화 획득 이벤트
        public static Func<int, Task> OnGetGoldBar; // 금괴 획득 이벤트
        public static Func<int, Task<bool>> OnUseGoldBar; // 금괴 사용 이벤트
        public static Func<int, bool> OnCanUseGoldBar; // 금괴 사용 가능 여부 확인 이벤트
    }
}
// 마지막 작성 일자: 2025.09.09