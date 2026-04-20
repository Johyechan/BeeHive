using DG.Tweening;
using InGame.MyManager.Local;
using InGame.MyUI.MyUIButton;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace InGame.MyUI
{
    // 작성자: 조혜찬
    // 예 또는 아니오를 확인할 때까지 대기하는 비동기 UI 전용 클래스
    public class ConfirmUI : MonoBehaviour
    {
        [SerializeField] private Button _yesButton; // 예 버튼
        [SerializeField] private Button _noButton; // 아니오 버튼

        [SerializeField] private float _animationDuration; // 애니메이션 지속 시간

        [SerializeField] private CardUseButton _cardUseButton; // 카드 사용 버튼 - 예 버튼 클래스

        [SerializeField] private TMP_Text _askText; // 사용을 묻는 텍스트

        private CanvasGroup _canvasGroup; // UI 애니메이션을 위한 canvasGroup 변수

        private UnityAction _yesButtonAction; // 예 버튼 델리게이트 (여기에 함수를 저장하여 구독 및 해제)
        private UnityAction _noButtonAction; // 아니오 버튼 델리게이트 (여기에 함수를 저장하여 구독 및 해제)

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        // 확인 종료 후 초기화 함수
        public void ConfirmEnd()
        {
            _yesButton.onClick.RemoveListener(_yesButtonAction); // 예 버튼 초기화
            _noButton.onClick.RemoveListener(_noButtonAction); // 아니오 버튼 초기화
        }

        public void Confirm(Action<bool> onResult, string message)
        {
            _askText.ForceMeshUpdate(); // TMP를 GPU에 강제로 올리기

            _cardUseButton.UICardBase = InGameContext.Current.Data.CardManager.FindFirePowerCard(); // 화력 카드 할당

            _askText.text = message;

            _yesButtonAction = () => onResult(true); // 예 버튼 이벤트
            _noButtonAction = () => onResult(false); // 아니오 버튼 이벤트

            _yesButton.onClick.AddListener(_yesButtonAction); // 예 버튼에 true를 반환하는 기능 구독
            _noButton.onClick.AddListener(_noButtonAction); // 아니오 버튼에 false 반환하는 기능 구독
            _canvasGroup.DOFade(1, _animationDuration); // 페이드 인
        }
    }
}
// 마지막 작성 일자: 2026.02.03