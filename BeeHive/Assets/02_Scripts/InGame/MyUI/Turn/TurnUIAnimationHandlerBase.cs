using DG.Tweening;
using InGame.MyManager;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace InGame.MyUI.Turn
{
    // 작성자: 조혜찬
    // 턴마다 나오는 UI 애니메이션 클래스들의 부모 클래스
    public abstract class TurnUIAnimationHandlerBase
    {
        protected CanvasGroup _canvasGroup; // 애니메이션 백그라운드 이미지 변수

        protected TMP_Text _tmpText; // 현재 턴을 보여줄 text 변수

        protected float _animationDuration; // 애니메이션 시간

        public TurnUIAnimationHandlerBase(CanvasGroup canvasGroup, TMP_Text tmpText, float animationDuration)
        {
            _canvasGroup = canvasGroup;
            _tmpText = tmpText;
            _animationDuration = animationDuration;
        }

        // 애니메이션을 구현할 함수
        public virtual async Task UIAnimationPlay()
        {
            await DOTween.Sequence() // 차례대로 실행 시키기 위한 DOTween 시퀀스
                .AppendCallback(() => _canvasGroup.gameObject.SetActive(true)) // 캔버스 그룹 오브젝트 활성화
                .Append(_canvasGroup.DOFade(1, _animationDuration)) // _animationDuration 동안 페이드 인
                .Append(_canvasGroup.DOFade(0, _animationDuration)) // _animationDuration 동안 페이드 아웃
                .AppendCallback(() => _canvasGroup.gameObject.SetActive(false)) // 캔버스 그룹 오브젝트 비활성화
                .AsyncWaitForCompletion();
        }
    }
}
// 마지막 작성 일자: 2025.09.23