using UnityEngine;

namespace InGame.MyUI
{
    // 작성자: 조혜찬
    // 버튼들의 스크롤 뷰 크기를 자동으로 맞춰주는 기능을 가지는 클래스
    public class GuideButtonPanelAutoSize : MonoBehaviour
    {
        [SerializeField] private int _showLastButtonScrollViewHeight; // 제일 밑에 있는 버튼을 눌렀을 때 버튼들을 숨기고 있던 스크롤 뷰
        [SerializeField] private int _hideLastButtonScrollViewHeight; // 제일 밑에 있는 버튼을 눌렀을 때 버튼들을 숨기고 있던 스크롤 뷰

        private RectTransform _scrollViewRectTransform; // 스크롤 뷰의 RectTransform

        private void Awake()
        {
            _scrollViewRectTransform = GetComponent<RectTransform>();
        }

        public void ChangeScrollViewHeight(float lastButtonYPos, bool isShow)
        {
            float _lastButtonScrollViewHeight = isShow == true ? _showLastButtonScrollViewHeight : _hideLastButtonScrollViewHeight;
            Vector2 currentSize = _scrollViewRectTransform.sizeDelta; // 현재 크기 버튼들의 스크롤 뷰 크기
            float targetHeight = lastButtonYPos + 600 + _lastButtonScrollViewHeight; // 맨 밑 버튼 위치 + 600 + 맨 밑 버튼을 눌렀을 때 보여지는 스크롤 뷰 높이 = 목표 높이
            Vector2 newSize = new Vector2(currentSize.x, targetHeight); // 새 크기
            _scrollViewRectTransform.sizeDelta = newSize; // 버튼들의 스크롤 뷰 크기에 새 크기를 할당
        }
    }
}
// 마지막 작성 일자: 2026.06.09