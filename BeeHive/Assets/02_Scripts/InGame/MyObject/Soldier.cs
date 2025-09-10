using InGame.MyEvent;
using InGame.MyManager;
using UnityEngine;

namespace InGame.MyObject
{
    // 작성자: 조혜찬
    // 보병 기물 클래스
    public class Soldier : PieceBase
    {
        protected override void Awake()
        {
            base.Awake();

            ParentSet();
        }

        // 부모 초기화 함수
        private void ParentSet()
        {
            _parent = GameObject.Find(TeamManager.Instance.SoldierParentName).transform; // 보병 객체의 부모 할당
        }

        public override void ObjectClicked()
        {
            base.ObjectClicked();

            PieceEvents.OnShowCanAttackPieces(MyEnum.ObjectType.Miner); // 공격 가능한 광부 기물들 하이라이트 키기
            PieceEvents.OnShowCanAttackPieces(MyEnum.ObjectType.Soldier); // 공격 가능한 보병 기물들 하이라이트 키기
        }
    }
}
// 마지막 작성 일자: 2025.09.10