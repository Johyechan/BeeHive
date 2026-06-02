using DG.Tweening;
using InGame.MyManager.Global;
using InGame.MyUI.MyUIInterface;
using MyUtil.GameMode;
using MyUtil.MyObjectPool;
using Tutorial;
using Tutorial.MyEnum;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Settings;

namespace InGame.MyUI
{
    // 작성자: 조혜찬
    // 특정 패널을 닫는 버튼 클래스
    public class ClosePanelButton : MonoBehaviour, IUIClick
    {
        [SerializeField] private CanvasGroup _targetPanel; // 닫을 패널

        [SerializeField] private float _animationDuration; // 애니메이션 지속 시간

        [SerializeField] private ObjectPoolType _panelType; // 오브젝트 풀링에 돌려놓을 객체라면 타입 붙여둘 변수
 
        // 클릭 시 실행될 함수
        public void OnUIClick()
        {
            _targetPanel.DOFade(0, _animationDuration) // _targetPanel을 _animationDuration 동안 페이드 아웃
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    if (_panelType != ObjectPoolType.None) // 오브젝트 풀링에 돌려놓을 객체라면
                    {
                        ObjectPoolManager.Instance.ReturnObject(_panelType, _targetPanel.gameObject); // 돌려놓기
                    }
                    else // 아니라면
                    {
                        if (GameModeManager.Instance.CurrentGameMode.IsTutorial()) // 튜토리얼 일 경우
                        {
                            string checkCardGoNextTurn = LocalizationSettings.StringDatabase.GetLocalizedString(
                                "Tutorial",
                                "Tutorial_CheckCardGoNextTurn"
                            );
                            TutorialManager.Instance.SetTutorialPanel(true, checkCardGoNextTurn, TutorialManager.Instance.ButtonClick, 0.18f, 0.008f, new Vector4(0.92f, 0.095f), new Vector4(0.66f, 0.4f));
                        }

                        _targetPanel.gameObject.SetActive(false); // 비활성화
                    }
                });
            EventSystem.current.SetSelectedGameObject(null); // 선택한 객체 초기화
        }
    }
}
// 마지막 작성 일자: 2026.06.02