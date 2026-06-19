using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.MyUI
{
    // 작성자: 조혜찬
    // 타이머 UI 연출 클래스
    public class TimerUI : MonoBehaviour
    {
        [SerializeField] private Image _timerUI;

        // 객체 활성화 시
        private void OnEnable()
        {
            _timerUI.fillAmount = 1; // UI 이미지 최대로 채워두기
        }

        // 객체 비활성화 시
        private void OnDisable()
        {
            _timerUI.DOKill(true); // 타이머 UI에 적용된 닷 트윈 최종 값 적용 후 제거
        }

        // 타이머 연출 시작 함수
        public void TimerAnimationStart(float waitTime)
        {
            _timerUI.DOFillAmount(0, waitTime);
        }
    }
}
// 마지막 작성 일자: 2026.06.19