using DG.Tweening;
using InGame.MyEnum;
using InGame.MyManager;
using InGame.MyManager.Global;
using InGame.MyManager.Local;
using InGame.MyManager.Turn;
using InGame.MyUI.Card;
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
            else // 성벽 강화 카드일 경우
            {
                ObjectPoolManager.Instance.ReturnObject(cardPoolType, addCardTrans.gameObject); // 성벽 강화 카드를 풀에 반환 - 성벽 강화 카드는 재사용 불가 카드이기 때문
            }

            yield return new WaitUntil(() => InGameContext.Current.Data.CardManager.CardReverseTask.Task.IsCompleted);

            bool end = false;
            switch (InGameContext.Current.Data.TurnManager.CurrentTeamType) // 현재 팀에 따라 카드 재세팅
            {
                case TeamType.Team1:
                    end = DrawEventSystem.OnCardObjectSet.Invoke(InGameContext.Current.Data.DeckManager.DeckProp.player1CardsParent);
                    yield return new WaitUntil(() => end);
                    break;
                case TeamType.Team2:
                    end = DrawEventSystem.OnCardObjectSet.Invoke(InGameContext.Current.Data.DeckManager.DeckProp.player2CardsParent);
                    yield return new WaitUntil(() => end);
                    break;
            }

            DrawEventSystem.OnCardUISet?.Invoke();// 카드 UI 재세팅

            if (InGameContext.Current.Data.DeckManager.IsEmpty && cardPoolType != ObjectPoolType.CastleUpgradeCard) // 덱이 비어 있으며 현재 사용한 카드가 성벽 강화 카드가 아닐 경우
            {
                if(InGameContext.Current.Data.TurnManager.CurrentTeamType == TeamManager.Instance.CurrentTeamType) // 현재 턴의 팀과 클라이언트의 팀이 같을 경우
                {
                    InGameContext.Current.Data.DeckManager.IsEmpty = false; // 덱이 비어 있지 않은 상태로 할당
                    InGameContext.Current.Data.DeckManager.ReMakeDeck(); // 덱 다시 만들기
                }
            }
        }
    }
}
// 마지막 작성 일자: 2026.02.19