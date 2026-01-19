using MyUtil;
using System.Threading.Tasks;
using UnityEngine;

namespace InGame
{
    // 작성자: 조혜찬
    // 방 준비 완료 여부 관리 클래스
    public static class RoomReady
    {
        public static readonly ReadyGate Gate = new();
    }
}
// 마지막 작성 일자: 2026.01.19