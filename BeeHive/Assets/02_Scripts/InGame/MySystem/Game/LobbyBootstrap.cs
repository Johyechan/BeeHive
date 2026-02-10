using UnityEngine;

namespace InGame.MySystem
{
    // 작성자: 조혜찬
    // 로비 씬에서 제일 먼저 실행될 클래스 - 부팅 클래스
    public class LobbyBootstrap : MonoBehaviour
    {
        private void Awake()
        {
            LobbyReady.Gate.Reset(); // 로비 준비 대기 초기화
        }
    }
}
// 마지막 작성 일자: 2026.02.10