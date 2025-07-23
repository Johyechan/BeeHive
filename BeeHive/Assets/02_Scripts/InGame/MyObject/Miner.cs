using InGame.MyEvent;
using InGame.MyManager.MyPlacePlane;
using InGame.MyObject.MyObjectEnum;
using UnityEngine;

namespace InGame.MyObject
{
    // 작성자: 조혜찬
    // 광부 기물 클래스
    public class Miner : PieceBase
    {
        private void Awake()
        {
            _parent = GameObject.Find("PlayerMiners").transform; // 광부 객체의 부모 변수 할당
        }
    }
}
// 마지막 작성 일자: 2025.07.23