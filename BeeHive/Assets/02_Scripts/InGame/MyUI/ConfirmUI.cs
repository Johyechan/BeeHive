using DG.Tweening;
using InGame.MyManager;
using InGame.MyUI.Card;
using InGame.MyUI.MyUIButton;
using MyUtil;
using MyUtil.MyObjectPool;
using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
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

        private TaskCompletionSource<bool> _tcs; // bool 결과 값을 대기하여 받는 변수

        private UnityAction _yesButtonAction; // 예 버튼 델리게이트 (여기에 함수를 저장하여 구독 및 해제)
        private UnityAction _noButtonAction; // 아니오 버튼 델리게이트 (여기에 함수를 저장하여 구독 및 해제)

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Click(bool value)
        {
            _tcs?.TrySetResult(value);
        }

        public async Task<bool> Confirm(string message = "화력을 사용하여 공격하시겠습니까?")
        {
            NetworkManager.Instance.Socket.Emit("debug", $"일단 gpu가 있습니다 {SystemInfo.graphicsDeviceType}");

            _cardUseButton.UICardBase = CardManager.Instance.FindFirePowerCard(); // 화력 카드 할당
            NetworkManager.Instance.Socket.Emit("debug", $"화력 카드도 할당했죠");

            _askText.text = message;
            NetworkManager.Instance.Socket.Emit("debug", $"텍스트도 할당");

            _tcs = new TaskCompletionSource<bool>();
            NetworkManager.Instance.Socket.Emit("debug", $"대기 테스크도 생성");

            _yesButtonAction = () => Click(true); // 예 버튼 이벤트
            _noButtonAction = () => Click(false); // 아니오 버튼 이벤트
            NetworkManager.Instance.Socket.Emit("debug", $"델리게이트도 제작");

            _yesButton.onClick.AddListener(_yesButtonAction); // 예 버튼에 true를 반환하는 기능 구독
            _noButton.onClick.AddListener(_noButtonAction); // 아니오 버튼에 false 반환하는 기능 구독
            NetworkManager.Instance.Socket.Emit("debug", $"버튼에 이벤트 추가");

            await _canvasGroup.DOFade(1, _animationDuration).AsyncWaitForCompletion(); // 페이드 인
            NetworkManager.Instance.Socket.Emit("debug", $"페이드 인 성공");

            bool result = await _tcs.Task;
            NetworkManager.Instance.Socket.Emit("debug", $"결과 받기");

            _yesButton.onClick.RemoveListener(_yesButtonAction); // 예 버튼 초기화
            _noButton.onClick.RemoveListener(_noButtonAction); // 예 버튼 초기화
            NetworkManager.Instance.Socket.Emit("debug", $"버튼 이벤트 초기화");

            NetworkManager.Instance.Socket.Emit("debug", $"이제 결과 반환");
            return result;
        }
    }
}
// 마지막 작성 일자: 2025.10.27