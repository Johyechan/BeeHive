using DG.Tweening;
using InGame.MyEnum;
using InGame.MyManager;
using MyUtil.MyEvent;
using MyUtil.MyObjectPool;
using System.Collections;
using UnityEngine;

namespace InGame.MyObject.Handler
{
    // 작성자: 조혜찬
    // 카드 세팅 핸들러
    public class CardSettingHandler
    {
        private float _animationDuration;
        private float _yInterval;

        public CardSettingHandler(float animationDuration, float yInterval)
        {
            _animationDuration = animationDuration;
            _yInterval = yInterval;
        }

        // 카드 세팅 코루틴
        public IEnumerator CardSettingCo(Transform addCardTrans, ObjectPoolType cardPoolType, int usedCardCount)
        {
            if (cardPoolType != ObjectPoolType.CastleUpgradeCard) // 카드가 성벽 강화 카드가 아닐 경우에만
            {
                bool tweenEnd = false; // 트윈 종료 여부를 판단하는 변수
                DOTween.Sequence()
                    .AppendInterval(_animationDuration) // 대기
                    .Append(addCardTrans.DOLocalMove(new Vector3(0, _yInterval * usedCardCount, 0), _animationDuration))
                    .OnComplete(() => tweenEnd = true); // 사용한 카드 위치 이동

                yield return new WaitUntil(() => tweenEnd); // 트윈이 종료될 때까지 대기
            }

            yield return new WaitUntil(() => CardManager.Instance.CardReverseTask.Task.IsCompleted);

            switch (TurnManager.Instance.CurrentTeamType) // 현재 팀에 따라 카드 재세팅
            {
                case TeamType.Team1:
                    yield return new WaitUntil(() => DrawEventSystem.OnCardObjectSet.Invoke(DeckManager.Instance.DeckProp.player1CardsParent));
                    break;
                case TeamType.Team2:
                    yield return new WaitUntil(() => DrawEventSystem.OnCardObjectSet.Invoke(DeckManager.Instance.DeckProp.player2CardsParent));
                    break;
            }

            if (DeckManager.Instance.IsEmpty && cardPoolType != ObjectPoolType.CastleUpgradeCard) // 덱이 비어 있으며 현재 사용한 카드가 성벽 강화 카드가 아닐 경우
            {
                DeckManager.Instance.IsEmpty = false; // 덱이 비어 있지 않은 상태로 할당
                DeckManager.Instance.ReMakeDeck(); // 덱 다시 만들기
            }
        }
    }
}
// 마지막 작성 일자: 2025.12.03