using InGame.MyObject;
using UnityEngine;

namespace InGame.MyObject
{
    // 작성자: 조혜찬
    // 전차 기물 클래스
    public class Tank : PieceBase
    {
        private void Awake()
        {
            _parent = GameObject.Find("PlayerTanks").transform; // 전차 기물 부모 할당
        }
    }
}
// 마지막 작성 일자: 2025.07.23