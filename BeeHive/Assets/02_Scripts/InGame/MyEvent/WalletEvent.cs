using System;
using UnityEngine;

namespace InGame.MyEvent
{
    // 작성자: 조혜찬
    // 지갑 관련(금화 및 금회 획득, 사용) 이벤트를 가지는 정적 클래스
    public static class WalletEvent
    {
        public static Action<int> OnGetGoldCoin; // 금화 획득 이벤트
        public static Action<int> OnGetGoldBar; // 금괴 획득 이벤트
        public static Func<int, bool> OnUseGoldBar; // 금괴 사용 이벤트
    }
}
// 마지막 작성 일자: 2025.08.20