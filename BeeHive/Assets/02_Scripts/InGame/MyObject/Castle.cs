using UnityEngine;

namespace InGame.MyObject
{
    // 작성자: 조혜찬
    // 성 클래스(지켜야할 성)
    public class Castle : MonoBehaviour
    {
        [SerializeField] private int _hp;
        public int HP { get { return _hp; } set { _hp = value; } }
    }
}
// 마지막 작성 일자: 2025.10.01