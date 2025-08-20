using UnityEngine;

namespace MyUtil.MyObjectPool
{
    // 작성자: 조혜찬
    // 풀링 enum 값
    public enum ObjectPoolType
    {
        None, // 아무것도 아닌 상태
        UIcard, // UI 카드
        Road, // 도로 기물
        UIPanel, // 경고 또는 알림, 동의 여부를 띄우는 UI 패널
        GoldCoin, // 금화
        GoldBar // 금괴
    }
}
// 마지막 작성 일자: 2025.07.08