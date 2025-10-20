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
        [SerializeField] private TMP_Text _myHpText; // 자기 성의 체력을 알려주는 UI
        [SerializeField] private TMP_Text _otherHpText; // 상대 성의 체력을 알려주는 UI

        [SerializeField] private TeamType _castleTeamType; // 성의 팀 타입

        [SerializeField] private int _hp; // 체력

        private int _currentHp; // 현재 체력
        public int CurrentHp { get => _currentHp; } // 위 변수 프로퍼티

        private void Awake()
        {
            _currentHp = _hp; // 현재 체력을 최대 체력으로 할당

            if(_castleTeamType == TeamManager.Instance.CurrentTeamType) // 성의 팀과 현재 팀이 같을 경우
            {
                _myHpText.text = $"성 체력: {_currentHp}"; // UI 적용
            }
            else // 성의 팀과 현재 팀이 다를 경우
            {
                _otherHpText.text = $"성 체력: {_currentHp}"; // UI 적용
            }
        }

        // 성 강화 함수(최대 체력 1증가)
        public void CastleUpgrade(int currentHp = 0)
        {
            if(_castleTeamType == TeamManager.Instance.CurrentTeamType) // 자신의 성이라면
            {
                NetworkManager.Instance.Socket.Emit("debug", "자기 성 체력 올리기");
                _currentHp++; // 현재 체력 증가

                _myHpText.text = $"성 체력: {_currentHp}"; // UI 적용
            }
            else // 상대의 성이라면
            {
                NetworkManager.Instance.Socket.Emit("debug", "상대 성 체력 올리기");

                _otherHpText.text = $"성 체력: {currentHp}"; // UI 적용
            }
        }
    }
}
// 마지막 작성 일자: 2025.10.20