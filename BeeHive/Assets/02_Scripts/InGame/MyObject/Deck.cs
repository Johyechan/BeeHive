using DG.Tweening;
using InGame.MyManager.MyCard;
using InGame.MyObject.MyObjectInterface;
using MyUtil.MyEvent;
using MyUtil.MyObjectPool;
using UnityEngine;

namespace InGame.MyObject
{
    // 작성자: 조혜찬
    // 덱 클래스 - 클릭되었을 때 카드를 추가 시키는 기능을 가지는 클래스
    public class Deck : MonoBehaviour, IClickObject
    {
        [SerializeField] private Transform _playerCardsParent; // 플레이어 카드들의 부모 Transform 변수
        [SerializeField] private Transform _otherCardsParent; // 적 카드들의 부모 Transform 변수

        [SerializeField] private RectTransform _playerUICardsParent; // 플레이어 UI 카드들의 부모 RectTransform 변수

        private Transform _deckTransform; // 덱 Transform 변수 - 현재 덱에 있는 카드의 수를 알기 위한 변수

        private int _currentDeckCardCount; // 현재 덱에 있는 카드 수

        // 변수 초기화
        private void Awake()
        {
            _deckTransform = GetComponent<Transform>();

            _currentDeckCardCount = _deckTransform.childCount; 
        }

        private void Update()
        {
            // 임시
            if(Input.GetKeyDown(KeyCode.D))
            {
                if (!DrawManager.Instance.IsCanDraw) // 만약 Draw가 불가능하다면
                    return; // 반환

                Sequence sequence = DOTween.Sequence()
                    .AppendCallback(() => DrawCard())
                    .AppendCallback(() => DrawEventSystem.OnDraw?.Invoke());
            }
        }

        public void DrawCard(bool isPlayerDraw = true)
        {
            if (isPlayerDraw) // 만약 플레이어가 드로우하는 상태라면
            {
                _deckTransform.GetChild(_currentDeckCardCount - 1).SetParent(_playerCardsParent); // 덱에 있는 카드를 플레이어의 카드로 변경 - 실제 값은 -1을 하지 않아야 하지만 인덱스로 활용할 것이기 때문에 -1을 하여 배열 크기 초과 오류를 방지
                GameObject uiCard = ObjectPoolManager.Instance.GetObject(ObjectPoolType.UIcard, _playerUICardsParent); // UI 카드를 추가하여 플레이어 UI 카드에 추가
            }
            else // 만약 적이 드로우하는 상태라면
            {
                _deckTransform.GetChild(_currentDeckCardCount - 1).SetParent(_otherCardsParent); // 덱에 있는 카드를 적의 카드로 변경 - 실제 값은 -1을 하지 않아야 하지만 인덱스로 활용할 것이기 때문에 -1을 하여 배열 크기 초과 오류를 방지
            }

            _currentDeckCardCount--; // 현재 덱에 있는 카드 수 감소
        }
        
        // 클릭되었을 때 실행될 함수
        public void ObjectClicked()
        {
            DrawCard(); // 플레이어가 카드를 획득하는 형태로 드로우 함수 실행
        }
    }
}
// 마지막 작성 일자: 2025.07.08