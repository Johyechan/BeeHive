using InGame.MyEnum;
using InGame.MyManager.Local;
using MyUtil.GameMode;
using Tutorial;
using UnityEngine;

namespace InGame.MyUI.MyUIButton
{
    // 작성자: 조혜찬
    // UI에서 보유한 카드들을 보여주게 만드는 UI 버튼 클래스
    public class CardShowButton : ShowButtonBase
    {
        public override void OnUIClick()
        {
            if (InGameContext.Current.Data.ShowButtonManager.IsShowType(ShowUIType.Card)) // 노출된 UI 타입이 Card라면
            {
                InGameContext.Current.Data.ShowButtonManager.SetShowType(ShowUIType.None);
                HideAnimationY(_cardsUI, _showDownYPos); // 숨김 함수 실행
            }
            else // ShowUI타입이 None이거나 Piece라면
            {
                InGameContext.Current.Data.ShowButtonManager.SetShowType(ShowUIType.Card);
                ShowAnimationY(_cardsUI, _piecesUI, _showYPos, _showDownYPos); // 노출 함수 실행

                if(GameModeManager.Instance.CurrentGameMode.IsTutorial()) // 튜토리얼 일 경우
                {
                    TutorialManager.Instance.SetTutorialPanel(true, "카드를 확인 해봅시다.", "대상 우클릭", 0.1f, 0.008f, new Vector4(0.5f, 0.15f), new Vector4(1.2f, 1.2f));
                }
            }
        }
    }
}
// 마지막 작성 일자: 2026.03.18
