using DG.Tweening;
using InGame.MyManager;
using InGame.MyObject;
using InGame.MyUI.Card;
using InGame.MyUI.MyUIInterface;
using System.Collections;
using UnityEngine;

namespace InGame.MyUI.MyUIButton
{
    // 작성자: 조혜찬
    // 카드 사용 버튼 클래스
    public class CardUseButton : MonoBehaviour, IUIClick
    {
        [SerializeField] private CanvasGroup _canvasGroup; // 버튼의 부모 패널의 CanvasGroup

        [SerializeField] private float _animationDuration; // 애니메이션 지속 시간

        private UICardBase _uiCardBase; // 사용될 카드의 베이스 클래스
        public UICardBase UICardBase { get => _uiCardBase; set => _uiCardBase = value; } // 외부에서 할당하기 위한 프로퍼티

        // 클릭 시 실행될 함수
        public void OnUIClick()
        {
            if (_uiCardBase.UseCard() == false) // 카드 사용에 예외가 발생했다면
            {
                DOTween.Sequence()
                .Append(_canvasGroup.DOFade(0, _animationDuration)) // 페이드 아웃
                .OnComplete(() =>
                {
                    _canvasGroup.gameObject.SetActive(false);
                }); // 객체 비활성화
                return; // 반환
            }

            DOTween.Sequence()
                .Append(_canvasGroup.DOFade(0, _animationDuration))
                .OnComplete(() =>
                {
                    ReverseCardObject(); // 카드 객체 뒤집기
                    _canvasGroup.gameObject.SetActive(false);
                }); // 페이드 아웃
        }

        // UI 카드에 맞는 카드 객체를 뒤집는 함수
        private void ReverseCardObject()
        {
            ReverseCardInfo reverseCardInfo = new ReverseCardInfo()
            {
                roomID = SceneMgr.Instance.CurrentRoomID, // 현재 방 ID
                cardID = _uiCardBase.UICardVariable.cardObj.GetComponent<CardObject>().ID, // 뒤집히는 카드의 ID
                animationDuration = _animationDuration, // 애니메이션 지속 시간
            };
            string json = JsonUtility.ToJson(reverseCardInfo); // Json 형태로 변환
            NetworkManager.Instance.Socket.Emit("reverseCard", json); // 서버에 전송

            DOTween.Sequence()
                .Append(_uiCardBase.UICardVariable.cardObj.transform.DORotate(new Vector3(0, _uiCardBase.UICardVariable.cardObj.transform.eulerAngles.y, 180), _animationDuration)) // y축은 Team1의 경우 플레이어의 시야를 고려하여 180도 돌아가 있기 때문에 카드의 y값으로 그대로 적용, z축으로 180도 회전
                .Join(_uiCardBase.UICardVariable.cardObj.transform.DOMoveY(0.0001f, _animationDuration));// y축을 조금 올리는 이유는 안 올릴 경우 바닥을 뚫는 문제 발생
        }
    }
}
// 마지막 작성 일자: 2025.11.25