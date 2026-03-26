using DG.Tweening;
using InGame.MyManager;
using InGame.MyManager.Global;
using InGame.MyManager.Local;
using InGame.MyObject;
using InGame.MyUI.Card;
using InGame.MyUI.MyUIInterface;
using MyUtil.GameMode;
using MyUtil.MyObjectPool;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

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
            InGameContext.Current.Data.CardManager.CardReverseTask = new TaskCompletionSource<bool>(); // 카드 뒤집기 대기 테스크 할당

            if(_uiCardBase != null) // ui 카드가 존재할 때
            {
                if (_uiCardBase.UseCard() == false) // 카드 사용에 예외가 발생했다면
                {
                    DOTween.Sequence()
                    .Append(_canvasGroup.DOFade(0, _animationDuration)) // 페이드 아웃
                    .OnComplete(() =>
                    {
                        _canvasGroup.gameObject.SetActive(false);
                    }); // 객체 비활성화
                    EventSystem.current.SetSelectedGameObject(null); // 선택한 객체 초기화
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
            else // ui 카드가 존재하지 않는다면
            {
                UIManager.Instance.WarningUIMake("카드가 존재하지 않습니다");
                EventSystem.current.SetSelectedGameObject(null); // 선택한 객체 초기화
                return;
            }
            EventSystem.current.SetSelectedGameObject(null); // 선택한 객체 초기화
        }

        // UI 카드에 맞는 카드 객체를 뒤집는 함수
        private void ReverseCardObject()
        {
            ReverseCardInfo reverseCardInfo = new ReverseCardInfo()
            {
                roomID = SceneMgr.Instance.CurrentRoomID, // 현재 방 ID
                cardID = _uiCardBase.UICardVariable.cardObj.GetComponent<CardObject>().NetworkId, // 뒤집히는 카드의 ID
                animationDuration = _animationDuration, // 애니메이션 지속 시간
            };
            string json = JsonUtility.ToJson(reverseCardInfo); // Json 형태로 변환
            if (GameModeManager.Instance.CurrentGameMode.UseServer())
                NetworkManager.Instance.Socket.Emit("reverseCard", json); // 서버에 전송

            DOTween.Sequence()
                .Append(_uiCardBase.UICardVariable.cardObj.transform.DORotate(new Vector3(0, _uiCardBase.UICardVariable.cardObj.transform.eulerAngles.y, 180), _animationDuration)) // y축은 Team1의 경우 플레이어의 시야를 고려하여 180도 돌아가 있기 때문에 카드의 y값으로 그대로 적용, z축으로 180도 회전
                .Join(_uiCardBase.UICardVariable.cardObj.transform.DOMoveY(0.0001f, _animationDuration)) // y축을 조금 올리는 이유는 안 올릴 경우 바닥을 뚫는 문제 발생
                .AppendInterval(_animationDuration) // 대기
                .OnComplete(() =>
                {
                    CardObject cardObject = _uiCardBase.UICardVariable.cardObj.GetComponent<CardObject>();
                    if(cardObject.CardPoolType == ObjectPoolType.CastleUpgradeCard) // 현재 카드가 성벽 강화 카드라면
                    {
                        ObjectPoolManager.Instance.ReturnObject(cardObject.CardPoolType, _uiCardBase.UICardVariable.cardObj); // 성벽 강화 카드를 풀에 반환 - 성벽 강화 카드는 재사용 불가 카드이기 때문
                    }
                    InGameContext.Current.Data.CardManager.CardReverseTask.SetResult(true); // 카드 뒤집기 완료
                });
        }
    }
}
// 마지막 작성 일자: 2026.03.26