using InGame.MyManager.Global;
using UnityEngine;

namespace InGame.MyUI
{
    // 작성자: 조혜찬
    // 버튼들의 스크롤 뷰 크기를 자동으로 맞춰주는 기능을 가지는 클래스
    public class GuideButtonPanelAutoSize : MonoBehaviour
    {
        private RectTransform _scrollViewRectTransform; // 스크롤 뷰의 RectTransform

        private void Awake()
        {
            _scrollViewRectTransform = GetComponent<RectTransform>();
        }

        public void ChangeScrollViewHeight(float buttonPosChangeValue)
        {
            Vector2 currentSize = _scrollViewRectTransform.sizeDelta; // 현재 크기 저장
            float finalHeight = currentSize.y + buttonPosChangeValue; // 최종 높이를 현재 높이 + 버튼들의 이동 값으로 결정
            _scrollViewRectTransform.sizeDelta = new Vector2(currentSize.x, finalHeight); // 크기 변경(x축은 그대로 y축은 버튼들의 이동 값 만큼 변경)
        }
    }
}
// 마지막 작성 일자: 2026.06.10