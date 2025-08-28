using DG.Tweening;
using InGame.MyEnum;
using InGame.MyEvent;
using InGame.MyManager;
using InGame.MyManager.MyCard;
using InGame.MyObject.MyObjectInterface;
using MyUtil;
using MyUtil.MyEvent;
using MyUtil.MyObjectPool;
using System.Threading.Tasks;
using UnityEngine;

namespace InGame.MyObject
{
    // 작성자: 조혜찬
    // 덱 클래스 - 클릭되었을 때 카드를 추가 시키는 기능을 가지는 클래스
    public class Deck : MonoBehaviour
    {
        public Transform player1CardsParent; // 플레이어1 카드들의 부모 Transform 변수
        public Transform player2CardsParent; // 플레이어2 카드들의 부모 Transform 변수
        public Transform player3CardsParent; // 플레이어3 카드들의 부모 Transform 변수

        [SerializeField] private RectTransform _playerUICardsParent; // 플레이어 UI 카드들의 부모 RectTransform 변수

        public Transform deckTransform; // 덱 Transform 변수 - 현재 덱에 있는 카드의 수를 알기 위한 변수

        // 변수 초기화
        private void Awake()
        {
            deckTransform = GetComponent<Transform>();
        }

        private void Update()
        {
            // 임시
            if(Input.GetKeyDown(KeyCode.D))
            {
                _ = Draw(); // Task 반환 없이 바로 Draw 함수 실행
            }
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

            switch(TeamManager.Instance.CurrentTeamType)
            {
                case TeamType.Team1: // 현재 팀이 Team1일 때
                    await DOTween.Sequence()
                          .AppendCallback(() => DrawManager.Instance.DrawCard(deckTransform, player1CardsParent, _playerUICardsParent)) // 카드 드로우 실행
                          .AppendCallback(() => DrawEventSystem.OnCardUISet?.Invoke())
                          .JoinCallback(() => DrawEventSystem.OnCardObjectSet?.Invoke(player2CardsParent)).AsyncWait();// 드로우 이벤트 인보크 후 시퀀스 완료 시 Task 완료 반환
                    break;
                case TeamType.Team2: // 현재 팀이 Team1일 때
                    await DOTween.Sequence()
                          .AppendCallback(() => DrawManager.Instance.DrawCard(deckTransform, player2CardsParent, _playerUICardsParent)) // 카드 드로우 실행
                          .AppendCallback(() => DrawEventSystem.OnCardUISet?.Invoke())
                          .JoinCallback(() => DrawEventSystem.OnCardObjectSet?.Invoke(player2CardsParent)).AsyncWait(); // 드로우 이벤트 인보크 후 시퀀스 완료 시 Task 완료 반환
                    break;
                case TeamType.Team3: // 현재 팀이 Team1일 때
                    await DOTween.Sequence()
                          .AppendCallback(() => DrawManager.Instance.DrawCard(deckTransform, player3CardsParent, _playerUICardsParent)) // 카드 드로우 실행
                          .AppendCallback(() => DrawEventSystem.OnCardUISet?.Invoke())
                          .JoinCallback(() => DrawEventSystem.OnCardObjectSet?.Invoke(player3CardsParent)).AsyncWait(); // 드로우 이벤트 인보크 후 시퀀스 완료 시 Task 완료 반환
                    break;
            }

            NetworkManager.Instance.Socket.Emit("draw"); // 서버에 DrawCompleted 신호 보내기
        }
    }
}
// 마지막 작성 일자: 2025.07.08