using UnityEngine;

namespace InGame.MySystem
{
    // 작성자: 조혜찬
    // 게임 씬에서 제일 먼저 실행될 클래스 - 부팅 클래스
    public class GameBootstrap : MonoBehaviour
    {
        private void Awake()
        {
            LocalManagerReady.Gate.Reset(); // 씬 내 매니저 세팅 대기 초기화
            TeamReady.Gate.Reset(); // 팀 할당 대기 초기화
            GameReady.Gate.Reset(); // 게임 준비 대기 초기화
            EventReady.Reset(); // 이벤트 준비 대기 초기화
        }
    }
}
// 마지막 작성 일자: 2026.02.03