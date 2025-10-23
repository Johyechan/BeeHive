using InGame.MyEnum;
using InGame.MyInput.Struct;
using InGame.MyManager;
using InGame.MyObject;
using System.Threading.Tasks;
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

        private InputDrawHandlerData _handlerData;

        public InputDrawHandler(Deck deck, int delay, InputDrawHandlerData handlerData)
        {
            _deck = deck;
            _canDraw = true;
            _delay = delay;
            _handlerData = handlerData;
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
            if (await _handlerData.returnHandler.IsReturn()) // 반환을 해야한다면
                return; // 반환

            _ = Delay(); // 연속적인 드로우를 막기 위한 딜레이 시작

            switch (TeamManager.Instance.CurrentTeamType)
            {
                case TeamType.Team1: // 현재 팀이 Team1일 때
                    await _handlerData.functionHandler.DrawFunction(_deck.deckTransform, _deck.player1CardsParent, _deck._playerUICardsParent);
                    break;

                case TeamType.Team2: // 현재 팀이 Team2일 때
                    await _handlerData.functionHandler.DrawFunction(_deck.deckTransform, _deck.player2CardsParent, _deck._playerUICardsParent);
                    break;

                case TeamType.Team3: // 현재 팀이 Team3일 때
                    await _handlerData.functionHandler.DrawFunction(_deck.deckTransform, _deck.player3CardsParent, _deck._playerUICardsParent);
                    break;
            }

            _handlerData.socketEventHandler.CallSocketEvent();
        }
    }
}
// 마지막 작성 일자: 2025.09.18