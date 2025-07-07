using InGame.MyUI.MyUIInterface;
using DG.Tweening;
using UnityEngine;

namespace InGame.MyUI.MyUIButton
{
    // 작성자: 조혜찬
    // 보여주는 애니메이션을 실행시키는 버튼의 부모 클래스
    public abstract class ShowButtonBase : MonoBehaviour, IUIButton
    {
        [SerializeField] private float _delayForNext; // 다음 함수를 실행하기까지 대기 시간
        [SerializeField] private float _animationDelay; // 애니메이션 실행 시간 변수

        // 클릭 시 실행될 함수
        public abstract void OnUIButtonClick();

        // 보여주는 애니메이션 함수 - 매개변수로 보여줄 UI의 RectTransform과 내릴 UI의 RectTransform을 받는다
        protected void ShowAnimationY(RectTransform showUI, RectTransform showDownUI, float showYPos, float showDownYPos)
        {
            Sequence sequence = DOTween.Sequence()
                .Append(showDownUI.DOAnchorPosY(showDownYPos, _animationDelay)) // 위 아래로만 움직일 것이기 때문에 앵커 포지션 Y축으로만 움직이는 DOTWEEN 함수 사용 - 우선 showDownYPos 위치로 _animationDelay 동안 Y축 이동
                .Insert(_delayForNext, showUI.DOAnchorPosY(showYPos, _animationDelay)); // 위 아래로만 움직일 것이기 때문에 앵커 포지션 Y축으로만 움직이는 DOTWEEN 함수 사용 - showYPos 위치로 _delayForNext 초 후 _animationDelay 동안 Y축 이동
        }
    }
}

