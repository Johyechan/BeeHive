using UnityEngine;

namespace MyUtil
{
    // 작성자: 조혜찬
    // 씬이 변경되어도 삭제되지 않는 객체들에게 부여할 클래스
    public class DontDestroyObject : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
// 마지막 작성 일자: 2025.07.29