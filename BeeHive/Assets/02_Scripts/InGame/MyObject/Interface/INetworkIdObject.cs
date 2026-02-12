using UnityEngine;

namespace InGame.MyObject.Interface
{
    // 작성자: 조혜찬
    // 네트워크 ID를 가져야 하는 객체들이 가지는 인터페이스
    public interface INetworkIdObject
    {
        public int NetworkId { get; set; } // 네트워크 ID

        public GameObject CurrentObject { get; } // 네트워크 ID를 가지는 객체
    }
}
// 마지막 작성 일자: 2026.02.12