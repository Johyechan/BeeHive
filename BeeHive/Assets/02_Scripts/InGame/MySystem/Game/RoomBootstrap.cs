using UnityEngine;

namespace InGame.MySystem
{
    // 작성자: 조혜찬
    // 방 씬에서 제일 먼저 실행될 클래스 - 부팅 클래스
    public class RoomBootstrap : MonoBehaviour
    {
        private void Awake()
        {
            RoomReady.Gate.Reset(); // 방 준비 대기 초기화
        }
    }
}
// 마지막 작성 일자: 2026.01.20