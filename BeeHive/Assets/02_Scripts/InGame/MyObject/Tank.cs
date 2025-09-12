using InGame.MyEvent;
using InGame.MyManager;
using UnityEngine;

namespace InGame.MyObject
{
    // 작성자: 조혜찬
    // 전차 기물 클래스
    public class Tank : PieceBase
    {
        protected override void Awake()
        {
            base.Awake();

            ParentSet();
        }

        // 부모 초기화 함수
        private void ParentSet()
        {
            _parent = GameObject.Find(TeamManager.Instance.TankParentName).transform; // 보병 객체의 부모 할당
        }

        public override void ObjectClicked()
        {
            base.ObjectClicked();
        }
    }
}
// 마지막 작성 일자: 2025.09.12