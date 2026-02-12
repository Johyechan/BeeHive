using InGame.MyEvent;
using InGame.MyManager;
using InGame.MyManager.Global;
using InGame.MyObject.Interface;
using InGame.MyObject.MyObjectInterface;
using MyUtil.MyObjectPool;
using UnityEngine;

namespace InGame.MyObject
{
    // 작성자: 조혜찬
    // 카드 객체 클래스
    public class CardObject : MonoBehaviour, INetworkIdObject
    {
        [SerializeField] private ObjectPoolType _cardUIPoolType; // 해당 카드가 생성 시킬 UI 카드 풀 타입
        public ObjectPoolType CardUIPoolType { get => _cardUIPoolType; } // 위 변수 프로퍼티

        [SerializeField] private ObjectPoolType _cardPoolType; // 해당 카드의 객체 풀 타입
        public ObjectPoolType CardPoolType { get => _cardPoolType; }

        private int _id;
        public int ID { get => _id; }

        public int NetworkId { get; set; } // 네트워크 ID

        public GameObject CurrentObject => gameObject;
    }
}
// 마지막 작성 일자: 2026.02.12