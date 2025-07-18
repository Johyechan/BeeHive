using UnityEngine;

namespace InGame.MyObject
{
    // 작성자: 조혜찬
    // 보병 기물 클래스
    public class Soldier : PieceBase
    {
        private void Awake()
        {
            _parent = GameObject.Find("PlayerSoldiers").transform; // 보병 객체의 부모 할당
        }

        public override void ObjectClicked()
        {
            throw new System.NotImplementedException();
        }
    }
}
// 마지막 작성 일자: 2025.07.18