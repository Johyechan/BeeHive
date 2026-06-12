using DG.Tweening;
using InGame.MyEnum;
using InGame.MySystem;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace InGame.MyUI
{
    // 작성자: 조혜찬
    // 다음 턴 금화 및 금괴를 알려주는 패널
    public class ShowNextTurnGold : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Wallet _wallet;

        [SerializeField] private Canvas _canvas; // UI 캔버스

        [SerializeField] private CanvasGroup _showNextTurnGoldPanel; // 다음 턴에 벌리는 금화 및 금괴를 알려주는 패널

        [SerializeField] private TMP_Text _goldCoinCountTxt; // 금화 수 텍스트
        [SerializeField] private TMP_Text _goldBarCountTxt; // 금괴 수 텍스트

        [SerializeField] private Vector2 _offset; // 마우스로부터 얼마나 떨어질지

        [SerializeField] private float _fadeDuration; // 페이드 시간

        [SerializeField] private TeamType _teamType;

        private Tween _fadeTween; // 페이드 인 아웃 트윈 저장 변수

        // 이 스크립트를 가지는 객체 위에 마우스가 올라갔을 경우
        public void OnPointerEnter(PointerEventData eventData)
        {
            // 이미 패널이 활성화 상태면
            if (_showNextTurnGoldPanel.gameObject.activeSelf)
                return; // 반환

            RectTransform canvasRect = _canvas.GetComponent<RectTransform>(); // UI 캔버스의 RectTransform 가져오기

            // 스크린 좌표(마우스) → 특정 RectTransform 기준 Local 좌표
            // canvasRect 내부 기준으로 Local 좌표 계산 (현재 내 기준: Canvas 전체 영역 기준)
            // 현재 마우스의 Screen 좌표
            // 이 Screen 좌표를 어떤 카메라 기준으로 해석할 건지
            // 결과 값
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, Input.mousePosition, null, out Vector2 localPoint);

            _showNextTurnGoldPanel.GetComponent<RectTransform>().anchoredPosition = localPoint + _offset; // 패널 위치 확정

            _wallet.CheckNextTurnGoldCoinAndGoldBar(_teamType);

            _goldCoinCountTxt.text = $"x {_wallet.NextTurnGoldCoin}";
            _goldBarCountTxt.text = $"x {_wallet.NextTurnGoldBar}";

            _fadeTween?.Kill(true); // 트윈이 있다면 즉시 완료 후 제거

            _showNextTurnGoldPanel.gameObject.SetActive(true); // 패널 활성화
            _fadeTween = _showNextTurnGoldPanel.DOFade(1, _fadeDuration); // 패널 페이드 인
        }

        // 이 스크립트를 가지는 객체 위에 마우스가 빠졌을 경우
        public void OnPointerExit(PointerEventData eventData)
        {
            // 이미 패널이 비활성화 상태면
            if (!_showNextTurnGoldPanel.gameObject.activeSelf) 
                return; // 반환

            _fadeTween?.Kill(true); // 트윈이 있다면 즉시 완료 후 제거
            _fadeTween = _showNextTurnGoldPanel.DOFade(0, _fadeDuration) // 패널 페이드 아웃
                .OnComplete(() => // 패널 페이드 아웃 완료 후
                {
                    _showNextTurnGoldPanel.gameObject.SetActive(false); // 패널 비활성화
                });
        }
    }
}
// 마지막 작성 일자: 2026.06.12