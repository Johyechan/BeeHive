using UnityEngine;

namespace InGame.MyManager.Local
{
    // 작성자: 조혜찬
    // 게임 씬 내부 전역 클래스들의 집합
    public class InGameContext : MonoBehaviour
    {
        public static InGameContext Current { get; private set; } // 외부에서 접근 가능한 인스턴스

        [SerializeField] private InGameContextData _data;
        public InGameContextData Data { get => _data; }

        private void Awake()
        {
            Current = this;

            LocalManagerReady.Gate.Completed(); // 씬 내 매니저 준비 완료
        }

        private void OnDestroy()
        {
            if(Current == this) // Current가 자기 자신일 때
                Current = null; // 초기화
        }
    }
}
// 마지막 작성 일자: 2026.02.03