using MyUtil;

namespace InGame
{
    // 작성자: 조혜찬
    // 네트워크 ID가 필요한 객체들에게 전부 할당이 됐는지 확인하는 대기 게이트
    public static class NetworkIDReady
    {
        public static readonly ReadyGate Gate = new();
    }
}
// 마지막 작성 일자: 2026.02.12