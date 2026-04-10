using DG.Tweening;
using InGame.MyUI.MyUIInterface;
using UnityEngine;

namespace InGame.MyUI.MyUIButton
{
    // 작성자: 조혜찬
    // 리스트를 보여주는 버튼
    public class ShowListButton : MonoBehaviour, IUIClick
    {
        [SerializeField] private RectTransform _listRect; // 리스트 RectTransform

        [SerializeField] private float _showHeight; // 보여줄 때 Height 값
        [SerializeField] private float _hideHeight; // 숨길 때 Height 값
        [SerializeField] private float _animationDuration; // 애니메이션 지속시간

        private Tweener _tweener; // 닷트윈 실행 기능 저장 변수

        private bool _isShow = false; // 보여주는 상태 여부

        public void OnUIClick()
        {
            Vector2 currrentSize = _listRect.sizeDelta; // 현재 크기
            _tweener?.Kill(); // 실행되던 기능 종료

            if (_isShow) // 보여진 상태일 경우
            {
                _tweener = _listRect.DOSizeDelta(new Vector2(currrentSize.x, _hideHeight), _animationDuration);
                _isShow = false;
            }
            else // 숨겨진 상태일 경우
            {
                _tweener = _listRect.DOSizeDelta(new Vector2(currrentSize.x, _showHeight), _animationDuration);
                _isShow = true;
            }
        }
    }
}
// 마지막 작성 일자: 2026.04.10