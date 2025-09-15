using InGame.MyManager;

namespace InGame.MyObject.Piece.ObjectPieces
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
            PieceVariable.parent = TeamManager.Instance.GetSoldierTransform(TeamManager.Instance.CurrentTeamType); // 보병 객체의 부모 할당
        }

        public override void ObjectClicked()
        {
            base.ObjectClicked();
        }
    }
}
// 마지막 작성 일자: 2025.09.15