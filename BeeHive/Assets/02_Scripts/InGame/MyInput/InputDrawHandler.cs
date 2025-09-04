using DG.Tweening;
using InGame.MyEnum;
using InGame.MyEvent;
using InGame.MyManager;
using InGame.MyManager.MyCard;
using InGame.MyObject;
using MyUtil.MyEvent;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InGame.MyInput
{
    // 작성자: 조혜찬
    // 드로우 핸들러 클래스
    public class InputDrawHandler
    {
        private Deck _deck; // 덱 클래스 - 드로우 할 때 필요한 객체들을 가지는 클래스

        private bool _canDraw; // 연속적인 드로우로 인해 발생하는 버그를 막기 위한 변수

        private int _delay; // 딜레이 시간

        public InputDrawHandler(Deck deck, int delay)
        {
            _deck = deck;
            _canDraw = true;
            _delay = delay;
        }

        // 인풋 액션에 구독할 함수 오버라이드
        public void Draw(InputAction.CallbackContext context)
        {
            if (!_canDraw) return; // _canDraw가 false 일때 바로 반환
            _ = Draw();
        }

        // 딜레이 함수
        private async Task Delay()
        {
            _canDraw = false; // 드로우 불가 상태
            await Task.Delay(_delay);
            _canDraw = true; // 드로우 가능 상태
        }

        // 드로우 함수
        private async Task Draw()
        {
            if (TurnManager.Instance.CurrentTurnType != TurnType.DrawTurn) // 드로우 턴이 아니라면
                return; // 반환

            if (TurnManager.Instance.CurrentTeamType != TeamManager.Instance.CurrentTeamType) // 내 팀의 턴이 아니라면
                return; // 반환

            if (!DrawManager.Instance.IsCanDraw) // 만약 Draw가 불가능하다면
                return; // 반환

            if (!WalletEvent.OnUseGoldBar.Invoke(2)) // 금괴 2개를 사용할 수 없다면
                return; // 반환

            _ = Delay(); // 연속적인 드로우를 막기 위한 딜레이 시작

            switch (TeamManager.Instance.CurrentTeamType)
            {
                case TeamType.Team1: // 현재 팀이 Team1일 때
                    Sequence seq1 = DOTween.Sequence()
                          .AppendCallback(() => DrawManager.Instance.DrawCard(_deck.deckTransform, _deck.player1CardsParent, _deck._playerUICardsParent)) // 카드 드로우 실행
                          .AppendCallback(() => DrawEventSystem.OnCardUISet?.Invoke())
                          .JoinCallback(() => DrawEventSystem.OnCardObjectSet?.Invoke(_deck.player1CardsParent));// 드로우 이벤트 인보크 후 시퀀스 완료
                    await seq1.AsyncWaitForCompletion(); // Task 완료 반환 대기
                    break;
                case TeamType.Team2: // 현재 팀이 Team1일 때
                    Sequence seq2 = DOTween.Sequence()
                          .AppendCallback(() => DrawManager.Instance.DrawCard(_deck.deckTransform, _deck.player2CardsParent, _deck._playerUICardsParent)) // 카드 드로우 실행
                          .AppendCallback(() => DrawEventSystem.OnCardUISet?.Invoke())
                          .JoinCallback(() => DrawEventSystem.OnCardObjectSet?.Invoke(_deck.player2CardsParent)); // 드로우 이벤트 인보크 후 시퀀스 완료
                    await seq2.AsyncWaitForCompletion(); // Task 완료 반환 대기
                    break;
                case TeamType.Team3: // 현재 팀이 Team1일 때
                    Sequence seq3 = DOTween.Sequence()
                          .AppendCallback(() => DrawManager.Instance.DrawCard(_deck.deckTransform, _deck.player3CardsParent, _deck._playerUICardsParent)) // 카드 드로우 실행
                          .AppendCallback(() => DrawEventSystem.OnCardUISet?.Invoke())
                          .JoinCallback(() => DrawEventSystem.OnCardObjectSet?.Invoke(_deck.player3CardsParent)); // 드로우 이벤트 인보크 후 시퀀스 완료
                    await seq3.AsyncWaitForCompletion(); // Task 완료 반환 대기
                    break;
            }

            DrawInfo drawInfo = new DrawInfo()
            {
                roomID = SceneMgr.Instance.CurrentRoomID, // 현재 방 ID
                targetID = NetworkManager.Instance.CurrentPlayerID // 현재 클라이언트 ID
            };

            string json = JsonUtility.ToJson(drawInfo); // Json 형태로 변환
            NetworkManager.Instance.Socket.Emit("draw", json); // 서버에 DrawCompleted 신호 보내기
        }
    }
}
// 마지막 작성 일자: 2025.09.02