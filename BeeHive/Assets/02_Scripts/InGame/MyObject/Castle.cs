using InGame.MyEnum;
using InGame.MyManager;
using TMPro;
using UnityEngine;

namespace InGame.MyObject
{
    // 작성자: 조혜찬
    // 성 클래스(지켜야할 성)
    public class Castle : MonoBehaviour
    {
        [SerializeField] private TMP_Text _hpText; // 체력을 알려주는 UI

        [SerializeField] private TeamType _castleTeamType; // 성의 팀 타입

        [SerializeField] private int _hp; // 최대 체력

        private int _maxHp; // 최대 체력
        private int _currentHp; // 현재 체력

        private void Awake()
        {
            _maxHp = _hp; // 최대 체력 초기화
            _currentHp = _maxHp; // 현재 체력을 최대 체력으로 할당

            if(_castleTeamType == TeamManager.Instance.CurrentTeamType) // 성의 팀과 현재 팀이 같을 경우
            {
                _hpText.text = $"성 체력: {_currentHp} / {_maxHp}"; // UI 적용
            }
        }

        // 성 강화 함수(최대 체력 1증가)
        public void CastleUpgrade()
        {
            _maxHp++; // 최대 체력 증가
            _currentHp++; // 현재 체력 증가

            _hpText.text = $"성 체력: {_currentHp} / {_maxHp}"; // UI 적용
        }
    }
}
// 마지막 작성 일자: 2025.10.17