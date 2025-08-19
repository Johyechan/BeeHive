using InGame.MyEvent;
using InGame.MyManager;
using InGame.MyManager.MyPlacePlane;
using UnityEngine;

namespace InGame.MyObject
{
    // 작성자: 조혜찬
    // 광부 기물 클래스
    public class Miner : PieceBase
    {
        private void Awake()
        {
            ParentSet();
        }

        // 부모 초기화 함수
        private void ParentSet()
        {
            _parent = GameObject.Find(TeamManager.Instance.MinerParentName).transform; // 보병 객체의 부모 할당
        }
    }
}
// 마지막 작성 일자: 2025.08.19