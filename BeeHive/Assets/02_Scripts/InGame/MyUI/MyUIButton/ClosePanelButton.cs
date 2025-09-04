using InGame.MyUI.MyUIInterface;
using UnityEngine;
using DG.Tweening;
using MyUtil.MyObjectPool;

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
            _targetPanel.DOFade(0, _animationDuration).SetLink(gameObject); // _targetPanel을 _animationDuration 동안 페이드 아웃
            if(_panelType != ObjectPoolType.None) // 오브젝트 풀링에 돌려놓을 객체라면
            {
                ObjectPoolManager.Instance.ReturnObject(_panelType, _targetPanel.gameObject); // 돌려놓기
            }
            else // 아니라면
                _targetPanel.gameObject.SetActive(false); // 비활성화
        }
    }
}
// 마지막 작성 일자: 2025.08.07