using MyUtil.MyObjectPool;
using UnityEngine;

namespace InGame.MyObject
{
    // 작성자: 조혜찬
    // 카드 객체 클래스
    public class CardObject : MonoBehaviour
    {
        [SerializeField] private ObjectPoolType _poolType; // 해당 카드가 생성 시킬 UI 카드 풀 타입
        public ObjectPoolType PoolType { get => _poolType; } // 위 변수 프로퍼티
    }
}
// 마지막 작성 일자: 2025.10.01