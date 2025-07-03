using InGame.MyUI.MyUIInterface;
using DG.Tweening;
using UnityEngine;

namespace InGame.MyUI.MyUIButton
{
    // 작성자: 조혜찬
    // 햄버거 메뉴 버튼 클래스
    public class HamburgerMenuButton : MonoBehaviour, IUIButton
    {
        [SerializeField] private RectTransform _hamburgerMenuViewRectTransform; // 햄버거 메뉴 뷰를 아래로 내려 열고 위로 올려 닫기 위해 필요한 변수
        [SerializeField] private RectTransform _hamburgerMenuIconRectTransform; // 햄버거 메뉴 아이콘의 RectTransform - 여기서는 Icon의 회전을 위해 필요한 변수

        [SerializeField] private float _animationDelay; // 애니메이션 실행 시간 변수
        [SerializeField] private float _hamburgerMenuOpenHeight; // 햄버거 메뉴 열렸을 때 햄버거 메뉴 뷰의 길이
        [SerializeField] private float _hamburgerMenuOpenZRotationValue; // 햄버거 메뉴가 열렸을 때 햄버거 메뉴 아이콘의 z축 회전 값
        

        private float _originSizeWidth; // 햄버거 메뉴 뷰의 너비의 크기 - 너비는 변경하지 않을 예정이기에 저장

        private bool _isOpen; // 햄버거 메뉴가 열려있는 상태인지 확인하기 위한 변수

        private void Awake()
        {
            _isOpen = false; // 안 열린 상태로 초기화
            _originSizeWidth = _hamburgerMenuViewRectTransform.sizeDelta.x; // 햄버거 메뉴 뷰의 너비 저장
        }

        // 클릭 시 실행될 함수
        public void OnUIButtonClick()
        {
            // 클릭 시 애니메이션 실행 - 아이콘 회전, 아래로 바 내려가기
            if(!_isOpen) // 햄버거 메뉴가 열리지 않은 상태라면
            {
                ClickAnimation(_hamburgerMenuOpenHeight, _hamburgerMenuOpenZRotationValue, true); // 햄버거 메뉴 뷰의 높이는 _hamburgerMenuOpenHeight, 햄버거 메뉴 아이콘의 z축 회전 값은 _hamburgerMenuOpenZRotationValue, 햄버거 메뉴가 열린 상태로 변경
            }
            else // 햄버거 메뉴가 열린 상태라면
            {
                ClickAnimation(0, 0, false); // 햄버거 메뉴 뷰의 높이는 0, 햄버거 메뉴 아이콘의 z축 회전 값은 0, 햄버거 메뉴가 닫힌 상태로 변경
            }
        }

        private void ClickAnimation(float height, float rotationValue, bool isOpen)
        {
            _hamburgerMenuViewRectTransform.DOSizeDelta(new Vector2(_originSizeWidth, height), _animationDelay, true); // _animationDelay 동안 햄버거 메뉴 뷰의 높이 변경
            _hamburgerMenuIconRectTransform.DORotate(new Vector3(0, 0, rotationValue), _animationDelay); // _animationDelay 동안 햄버거 메뉴 아이콘 회전
            _isOpen = isOpen; // 햄버거 메뉴 열림 여부 변경
        }
    }
}
// 마지막 작성 일자: 2025.07.01