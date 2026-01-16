using InGame.MyEnum;
using InGame.MyUI.Card;
using MyUtil;
using MyUtil.MyObjectPool;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace InGame.MyManager
{
    // 작성자: 조혜찬
    // 가지고 있는 카드들을 관리하는 매니저 싱글톤 클래스
    public class CardManager : MonoSingleton<CardManager>
    {
        private bool _haveFirePowerCard; // 화력 카드가 패에 있는지 여부
        public bool HaveFirePowerCard { get => _haveFirePowerCard; set => _haveFirePowerCard = value; } // 위 변수 프로퍼티

        private bool _cardUsed; // 카드 사용 여부
        public bool CardUsed { get => _cardUsed; set => _cardUsed = value; }

        private Dictionary<CardType, bool> _cardUsedCheckMap = new Dictionary<CardType, bool>();

        private TaskCompletionSource<bool> _cardReverseTask; // 카드 뒤집기 대기 테스크
        public TaskCompletionSource<bool> CardReverseTask { get => _cardReverseTask; set => _cardReverseTask = value; } // 카드 뒤집기 대기 테스크 프로퍼티

        [SerializeField] private Transform _uiCardsParent; // ui 카드 부모

        [SerializeField] private Transform _team1CardParent; // 팀1카드 부모
        [SerializeField] private Transform _team2CardParent; // 팀2카드 부모
        [SerializeField] private Transform _team3CardParent; // 팀3카드 부모

        protected override void Awake()
        {
            base.Awake();

            _cardUsedCheckMap.Add(CardType.CastleUpgrade, false);
            _cardUsedCheckMap.Add(CardType.Drought, false);
            _cardUsedCheckMap.Add(CardType.GoodHarvest, false);
            _cardUsedCheckMap.Add(CardType.FirePower, false);
            _cardUsedCheckMap.Add(CardType.RoadChange, false);
        }

        // 같은 타입의 카드가 사용됐는지 확인 및 처리하는 함수
        public bool CheckSameTypeCardWasUsed(CardType type)
        {
            if (_cardUsedCheckMap[type]) // type 형태의 카드를 이미 사용 했었다면
            {
                UIManager.Instance.WarningUIMake("같은 카드를 두 번 이상 사용할 수 없습니다"); // 경고창 띄우기
                return true; // 일전에 사용했다고 반환
            }

            _cardUsedCheckMap[type] = true; // 이미 사용하지 않은 경우 type 형태의 카드를 사용했다고 할당
            return false; // 그리고 일전에 사용한 적 없다고 반환
        }

        // 같은 타입의 카드 사용 여부를 초기화 시켜주는 함수
        public void ResetCardUse()
        {
            foreach(var type in _cardUsedCheckMap.Keys.ToList()) // 맵 순회
            {
                _cardUsedCheckMap[type] = false; // 해당 타입의 카드가 사용되지 않았다고 할당
            }
        }

        // 화력 카드 탐색 함수
        public UICardBase FindFirePowerCard()
        {
            for (int i = _uiCardsParent.childCount - 1; i >= 0; i--)
            {
                UICardBase uiCardBase = _uiCardsParent.GetChild(i).GetComponent<UICardBase>(); // 카드의 UICardBase 클래스 가져오기
                if (uiCardBase.UICardData.poolType == ObjectPoolType.FirePowerUICard) // 화력 카드라면
                {
                    return uiCardBase;
                }
            }

            return null;
        }

        public bool IsHaveCard(TeamType teamType)
        {
            switch(teamType)
            {
                case TeamType.Team1:
                    return _team1CardParent.childCount > 0 ? true : false;
                case TeamType.Team2:
                    return _team2CardParent.childCount > 0 ? true : false;
                case TeamType.Team3:
                    return _team3CardParent.childCount > 0 ? true : false;
                default:
                    return false;
            }
        }
    }
}
// 마지막 작성 일자: 2026.01.16