using InGame.MyEnum;
using InGame.MyManager.Local;

namespace InGame.MyUI.MyUIButton
{
    // 작성자: 조혜찬
    // UI에서 기물들을 보여주게 만드는 UI 버튼 클래스
    public class PieceShowButton : ShowButtonBase
    {
        public override void OnUIClick()
        {
            if (InGameContext.Current.Data.ShowButtonManager.IsShowType(ShowUIType.Piece)) // 노출된 UI 타입이 Piece라면
            {
                InGameContext.Current.Data.ShowButtonManager.SetShowType(ShowUIType.None);
                HideAnimationY(_piecesUI, _showDownYPos); // 숨김 함수 실행
            }
            else // ShowUI타입이 None이거나 Card라면
            {
                InGameContext.Current.Data.ShowButtonManager.SetShowType(ShowUIType.Piece);
                ShowAnimationY(_piecesUI, _cardsUI, _showYPos, _showDownYPos); // 노출 함수 실행
            }
        }
    }
}
// 마지막 작성 일자: 2026.02.03