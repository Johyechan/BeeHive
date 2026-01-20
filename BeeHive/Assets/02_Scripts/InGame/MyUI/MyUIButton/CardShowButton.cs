using DG.Tweening;
using InGame.MyEnum;
using InGame.MyManager;
using InGame.MyManager.UI.Button;
using UnityEngine;

namespace InGame.MyUI.MyUIButton
{
    // 작성자: 조혜찬
    // UI에서 보유한 카드들을 보여주게 만드는 UI 버튼 클래스
    public class CardShowButton : ShowButtonBase
    {
        public override void OnUIClick()
        {
            if (ShowButtonManager.Instance.IsShowType(ShowUIType.Card)) // 노출된 UI 타입이 Card라면
            {
                ShowButtonManager.Instance.SetShowType(ShowUIType.None);
                HideAnimationY(_cardsUI, _showDownYPos); // 숨김 함수 실행
            }
            else // ShowUI타입이 None이거나 Piece라면
            {
                ShowButtonManager.Instance.SetShowType(ShowUIType.Card);
                ShowAnimationY(_cardsUI, _piecesUI, _showYPos, _showDownYPos); // 노출 함수 실행
            }
        }
    }
}
// 마지막 작성 일자: 2026.01.20
