using DG.Tweening;
using InGame.MyEnum;
using InGame.MyManager;
using InGame.MyManager.MyCard;
using MyUtil.MyEvent;
using System.Threading.Tasks;
using UnityEngine;

namespace InGame.MySystem.Game
{
    public struct CardParents // 플레이어의 카드 객체 부모를 가지는 구조체
    {
        public Transform player1Parent;
        public Transform player2Parent;
        public Transform player3Parent;
    }
    // 작성자: 조혜찬
    // 카드 인게임 세팅 핸들 클래스
    public class CardSetHandle
    {
        private Transform _deckParent; // 덱 객체 부모

        private Transform _player1CardsParent; // 플레이어1 객체 부모
        private Transform _player2CardsParent; // 플레이어2 객체 부모
        private Transform _player3CardsParent; // 플레이어3 객체 부모

        public CardSetHandle(Transform deckParent, CardParents cardParents)
        {
            _deckParent = deckParent;

            _player1CardsParent = cardParents.player1Parent;
            _player2CardsParent = cardParents.player2Parent;
            _player3CardsParent = cardParents.player3Parent;
        }

        public async Task Setting(int targetTeam, int cardCount)
        {
            NetworkManager.Instance.Socket.Emit("debug", "뭐야");
            TeamType type = (TeamType)targetTeam; // 팀 구하기

            switch (type)
            {
                case TeamType.Team1: // 팀1 일 때
                    await GetCards(cardCount, _player1CardsParent); // 해당 플레이어가 카드 드로우
                    break;
                case TeamType.Team2: // 팀2 일 때
                    await GetCards(cardCount, _player2CardsParent); // 해당 플레이어가 카드 드로우
                    break;
                case TeamType.Team3: // 팀3 일 때
                    await GetCards(cardCount, _player3CardsParent); // 해당 플레이어가 카드 드로우
                    break;
            }

            await Task.CompletedTask; // Task 완료
        }

        private async Task GetCards(int cardCount, Transform playerCardsParent)
        {
            int count = Mathf.Abs(playerCardsParent.childCount - cardCount); // 실제 카드 수와 이미 생성되어 있던 카드 수 차이 구하기

            for(int i = 0; i < count; i++) // 카드 수 차이만큼 반복
            {
                await DrawManager.Instance.DrawCard(_deckParent, playerCardsParent, null, false); // ui는 제외, 객체만 드로우
                Sequence seq = DOTween.Sequence()
                    .AppendCallback(() => DrawEventSystem.OnCardObjectSet?.Invoke(playerCardsParent)); // 카드 객체 정렬

                await seq.AsyncWaitForCompletion(); // Task 완료 대기
            }
            await Task.CompletedTask; // Task 완료
        }
    }
}
// 마지막 작성 일자: 2025.09.09