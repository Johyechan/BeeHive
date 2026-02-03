using InGame.MyEnum;
using InGame.MyManager;
using InGame.MyManager.Global;
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

        private async void Awake()
        {
            _currentHp = _hp; // 현재 체력을 최대 체력으로 할당

            await TeamReady.Gate.WaitAsync(); // 팀 할당 대기

            if (NetworkManager.Instance.IsClientOver) // 클라이언트가 종료 되었다면
                return; // 반환

            if (_castleTeamType == TeamManager.Instance.CurrentTeamType) // 성의 팀과 현재 팀이 같을 경우
            {
                _myHpText.text = $"내 체력: {_currentHp}"; // UI 적용
            }
            else // 성의 팀과 현재 팀이 다를 경우
            {
                _otherHpText.text = $"상대 체력: {_currentHp}"; // UI 적용
            }
        }

        public void CastleHit(int damage)
        {
            _currentHp -= damage;
            if (_castleTeamType == TeamManager.Instance.CurrentTeamType) // 성의 팀과 현재 팀이 같을 경우
            {
                _myHpText.text = $"성 체력: {_currentHp}"; // UI 적용
            }
            else // 성의 팀과 현재 팀이 다를 경우
            {
                _otherHpText.text = $"성 체력: {_currentHp}"; // UI 적용
            }

            if(_currentHp <= 0 && TeamManager.Instance.CurrentTeamType == _castleTeamType) // 현재 체력이 0 이하라면 그리고 같은 팀의 성일 경우
            {

                GameOverInfo gameOverInfo = new GameOverInfo()
                {
                    roomID = SceneMgr.Instance.CurrentRoomID, // 현재 방 ID
                    loseTeamType = (int)_castleTeamType, // 패배 팀 타입
                };
                string json = JsonUtility.ToJson(gameOverInfo); // Json으로 변환
                NetworkManager.Instance.Socket.Emit("gameOver", json);
            }
        }

        // 성 강화 함수(최대 체력 1증가)
        public void CastleUpgrade(int currentHp = 0)
        {
            if(_castleTeamType == TeamManager.Instance.CurrentTeamType) // 자신의 성이라면
            {
                _currentHp++; // 현재 체력 증가
                _myHpText.text = $"내 체력: {_currentHp}"; // UI 적용
            }
            else // 상대의 성이라면
            {
                _otherHpText.text = $"상대 체력: {currentHp}"; // UI 적용
            }
        }
    }
}
// 마지막 작성 일자: 2026.02.03