using InGame.MyEnum;
using InGame.MyManager.Local;
using MyUtil.GameMode;
using System.Collections;
using Tutorial;
using Tutorial.MyEnum;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Settings;

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
                    StartCoroutine(TutorialCo());
                }
            }
            EventSystem.current.SetSelectedGameObject(null); // 선택한 객체 초기화
        }

        // 튜토리얼 코루틴
        protected override IEnumerator TutorialCo()
        {
            yield return base.TutorialCo(); // 부모 코루틴 대기

            switch (TutorialManager.Instance.CurrentTutorialState) // 현재 튜토리얼 상태가
            {
                case TutorialState.Turn1_Player:
                    string createRoad = LocalizationSettings.StringDatabase.GetLocalizedString(
                        "Tutorial",
                        "Tutorial_CreateRoad"
                    );
                    TutorialManager.Instance.SetTutorialPanel(true, createRoad, TutorialManager.Instance.ButtonClick, 0.1f, 0.008f, new Vector4(0.356f, 0.123f), new Vector4(0.5f, 0.3f)); // 가이드 패널 생성
                    break;
                case TutorialState.Turn2_Player:
                    TutorialManager.Instance.SetTutorialPanel(true, "전차를 생성합시다.", "버튼 클릭", 0.1f, 0.008f, new Vector4(0.651f, 0.123f), new Vector4(0.3f, 0.3f)); // 가이드 패널 생성
                    break;
                case TutorialState.Turn6_Player:
                    TutorialManager.Instance.SetTutorialPanel(true, "보병을 생성합시다.", "버튼 클릭", 0.1f, 0.008f, new Vector4(0.552f, 0.123f), new Vector4(0.3f, 0.3f));
                    break;
                case TutorialState.Turn8_Player:
                    TutorialManager.Instance.SetTutorialPanel(true, "전차를 생성합시다.", "버튼 클릭", 0.1f, 0.008f, new Vector4(0.651f, 0.123f), new Vector4(0.3f, 0.3f)); // 가이드 패널 생성
                    break;
            }
        }
    }
}
// 마지막 작성 일자: 2026.04.06