using DG.Tweening;
using InGame.MyUI.MyUIInterface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        [SerializeField] private List<RectTransform> _underButtons = new List<RectTransform>(); // 밑에 있는 버튼 리스트

        [SerializeField] private GuideButtonPanelAutoSize panelAutoSize; // 스크롤 뷰의 크기를 자동으로 변경하는 클래스
        
        private Tweener _tweener; // 닷트윈 실행 기능 저장 변수

        private bool _isShow = false; // 보여주는 상태 여부

        private List<TaskCompletionSource<bool>> _animationWaitTcs = new List<TaskCompletionSource<bool>>();

        public async void OnUIClick()
        {
            foreach(var tcs in  _animationWaitTcs) // 애니메이션 완료 대기
            {
                await tcs.Task;
            }

            _animationWaitTcs.Clear(); // 애니메이션 대기 리스트 비우기

            Vector2 currentSize = _listRect.sizeDelta; // 현재 크기
            _tweener?.Kill(); // 실행되던 기능 종료

            foreach(var button in _underButtons)
            {
                button.DOKill();
            }

            if (_isShow) // 보여진 상태일 경우
            {
                ClickAnimation(currentSize, false);
            }
            else // 숨겨진 상태일 경우
            {
                ClickAnimation(currentSize, true);
            }
        }

        // 클릭 되었을 때 실행될 애니메이션
        private void ClickAnimation(Vector2 currentSize, bool isShow)
        {
            float changeValue = isShow == true ? -_showHeight : _showHeight; // 보여주는 애니메이션이 필요하다면 버튼을 밑으로 내리고 숨길 경우 버튼을 위로 올린다
            float height = isShow == true ? _showHeight : _hideHeight; // 보여주는 경우 _showHeight, 숨기는 경우 _hideHeight

            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>(); // 새로운 애니메이션 대기 tcs 생성
            _animationWaitTcs.Add(tcs); // 애니메이션 대기 리스트에 새로 생성한 tcs 추가

            _tweener = _listRect.DOSizeDelta(new Vector2(currentSize.x, height), _animationDuration) // 버튼을 숨기고 있는 패널 사이즈 변경
                .OnComplete(() =>
                {
                    tcs.SetResult(true);
                });

            foreach (var button in _underButtons) // 밑에 있는 버튼 순회
            {
                TaskCompletionSource<bool> buttonTcs = new TaskCompletionSource<bool>(); // 새로운 애니메이션 대기 tcs 생성
                _animationWaitTcs.Add(buttonTcs); // 애니메이션 대기 리스트에 새로 생성한 tcs 추가
                float targetYPos = button.anchoredPosition.y + changeValue; // 버튼의 목표 위치 저장
                button.DOAnchorPosY(targetYPos, _animationDuration) // 버튼 이동
                    .OnComplete(() =>
                    {
                        buttonTcs.SetResult(true); // 애니메이션 완료
                    });
            }

            panelAutoSize.ChangeScrollViewHeight(-changeValue); // 스크롤 뷰 크기 변경(스크롤 크기는 버튼이 내려갈 수 록 높이가 커져야하기 때문에 앞에 -를 붙여 부호를 반전시킨다)

            _isShow = isShow;
        }
    }
}
// 마지막 작성 일자: 2026.06.10