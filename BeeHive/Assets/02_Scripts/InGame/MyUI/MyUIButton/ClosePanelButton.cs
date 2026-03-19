using InGame.MyUI.MyUIInterface;
using UnityEngine;
using DG.Tweening;
using MyUtil.MyObjectPool;
using MyUtil.GameMode;
using Tutorial;
using Tutorial.MyEnum;
using InGame.MyManager.Global;

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
                            TutorialManager.Instance.SetTutorialPanel(true, "카드를 확인 했으니, 이제 메인 턴으로 넘어갑시다.", "버튼 클릭", 0.18f, 0.008f, new Vector4(0.92f, 0.095f), new Vector4(0.66f, 0.4f));
                        }

                        _targetPanel.gameObject.SetActive(false); // 비활성화
                    }
                });
        }
    }
}
// 마지막 작성 일자: 2026.03.19