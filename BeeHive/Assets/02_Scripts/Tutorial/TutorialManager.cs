using UnityEngine;

namespace Tutorial
{
    // 작성자: 조혜찬
    // 튜토리얼 매니저
    public class TutorialManager : MonoBehaviour
    {
        public static TutorialManager Instance { get; private set; } // 외부에서 접근 가능한 인스턴스 프로퍼티
    }
}
// 마지막 작성 일자: 2026.03.09