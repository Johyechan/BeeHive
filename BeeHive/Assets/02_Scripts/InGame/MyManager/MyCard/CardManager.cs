using InGame.MyEnum;
using InGame.MyUI.Card;
using MyUtil;
using MyUtil.MyObjectPool;
using System.Collections;
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

        [SerializeField] private Transform _uiCardsParent; // ui 카드 부모

        [SerializeField] private Transform _team1CardParent; // 팀1카드 부모
        [SerializeField] private Transform _team2CardParent; // 팀2카드 부모
        [SerializeField] private Transform _team3CardParent; // 팀3카드 부모

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
// 마지막 작성 일자: 2025.10.24