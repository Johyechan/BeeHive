using InGame.MyEnum;
using InGame.MyManager.Local;
using MyUtil.GameMode;
using System.Collections;
using Tutorial;
using UnityEngine;

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

                if (GameModeManager.Instance.CurrentGameMode.IsTutorial()) // 현재 게임 모드가 튜토리얼일 경우
                {
                    StartCoroutine(TutorialCo()); //
                }
            }
        }

        // 튜토리얼 코루틴
        private IEnumerator TutorialCo()
        {
            TutorialManager.Instance.SetTutorialPanel(false); // 투명 벽 생성

            yield return new WaitForSeconds(_animationDelay); // _animationDelay 만큼 대기 후

            TutorialManager.Instance.SetTutorialPanel(true, "광부를 생성합시다.", "버튼 클릭", 0.1f, 0.008f, new Vector4(0.4475f, 0.123f), new Vector4(0.3f, 0.3f)); // 가이드 패널 생성
        }
    }
}
// 마지막 작성 일자: 2026.03.17