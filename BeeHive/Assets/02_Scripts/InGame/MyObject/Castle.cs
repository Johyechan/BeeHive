using InGame.MyEnum;
using InGame.MyEvent;
using InGame.MyManager;
using InGame.MyManager.Global;
using InGame.MyManager.Local;
using MyUtil.GameMode;
using TMPro;
using UnityEngine;

namespace InGame.MyObject
{
    // 작성자: 조혜찬
    // 성 클래스(지켜야할 성)
    public class Castle : MonoBehaviour
    {
        [SerializeField] private TMP_Text _team1HpText; // Team1 성의 체력을 알려주는 UI
        [SerializeField] private TMP_Text _team2HpText; // Team2 성의 체력을 알려주는 UI

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

            if(GameModeManager.Instance.CurrentGameMode.IsTutorial()) // 튜토리얼 일 경우
            {
                if (_castleTeamType == TeamManager.Instance.CurrentTeamType) // 자기 성일 경우
                {
                    GetCastleHpTmpTxt().text = $"나: {_currentHp} HP"; // UI 적용
                }
                else // 자기 성이 아닐 경우
                {
                    GetCastleHpTmpTxt(false).text = $"적: {_currentHp} HP"; // UI 적용
                }
            }
            else // 튜토리얼이 아닐 경우
            {
                if (_castleTeamType == TeamManager.Instance.CurrentTeamType) // 자기 성일 경우
                {
                    GetCastleHpTmpTxt().text = $"{NetworkManager.Instance.CurrentClientName}: {_currentHp} HP"; // UI 적용
                }
                else // 자기 성이 아닐 경우
                {
                    GetCastleHpTmpTxt(false).text = $"{SceneMgr.Instance.OtherNickName}: {_currentHp} HP"; // UI 적용
                }
            }
        }

        public void CastleHit(int damage)
        {
            _currentHp -= damage;

            if(GameModeManager.Instance.CurrentGameMode.IsTutorial()) // 튜토리얼 일 경우
            {
                if (_castleTeamType == TeamManager.Instance.CurrentTeamType) // 자기 성일 경우
                {
                    GetCastleHpTmpTxt().text = $"나: {_currentHp} HP"; // UI 적용
                }
                else // 자기 성이 아닐 경우
                {
                    GetCastleHpTmpTxt(false).text = $"적: {_currentHp}  HP"; // UI 적용
                }

                if(_currentHp < 0) // 체력이 0 이하라면
                {
                    Time.timeScale = 0; // 시간 멈춤
                    GameOverEvent.OnGameOver?.Invoke();
                    InGameContext.Current.Data.GameManager.GameIsOver(_castleTeamType); // 게임 오버
                }
            }
            else // 튜토리얼이 아닐 경우
            {
                if (_castleTeamType == TeamManager.Instance.CurrentTeamType) // 자기 성일 경우
                {
                    GetCastleHpTmpTxt().text = $"{NetworkManager.Instance.CurrentClientName} : {_currentHp} HP"; // UI 적용
                }
                else // 자기 성이 아닐 경우
                {
                    GetCastleHpTmpTxt(false).text = $"{SceneMgr.Instance.OtherNickName}: {_currentHp}  HP"; // UI 적용
                }
            }

            if(_currentHp <= 0 && TeamManager.Instance.CurrentTeamType == _castleTeamType) // 현재 체력이 0 이하라면 그리고 같은 팀의 성일 경우
            {
                GameOverInfo gameOverInfo = new GameOverInfo()
                {
                    roomID = SceneMgr.Instance.CurrentRoomID, // 현재 방 ID
                    loseTeamType = (int)_castleTeamType, // 패배 팀 타입
                };
                string json = JsonUtility.ToJson(gameOverInfo); // Json으로 변환
                if (GameModeManager.Instance.CurrentGameMode.UseServer())
                    NetworkManager.Instance.Socket.Emit("gameOver", json);
            }
        }

        // 성 강화 함수(최대 체력 1증가)
        public void CastleUpgrade(int currentHp = 0)
        {
            if(_castleTeamType == TeamManager.Instance.CurrentTeamType) // 자신의 성이라면
            {
                _currentHp++; // 현재 체력 증가
                GetCastleHpTmpTxt().text = $"{NetworkManager.Instance.CurrentClientName} : {_currentHp}  HP"; // UI 적용
            }
            else // 상대의 성이라면
            {
                GetCastleHpTmpTxt(false).text = $"{SceneMgr.Instance.OtherNickName}: {currentHp} HP"; // UI 적용
            }
        }

        // 특정 팀의 HP TMP_Text를 가져오는 함수(기본적으로 자기 팀의 성을 탐색하도록 매개변수 할당)
        private TMP_Text GetCastleHpTmpTxt(bool myCastle = true)
        {
            switch(TeamManager.Instance.CurrentTeamType)
            {
                case TeamType.Team1:
                    if (myCastle) // 내 팀의 성을 찾는 것이라면
                        return _team1HpText;
                    else
                        return _team2HpText;
                case TeamType.Team2:
                    if (myCastle) // 내 팀의 성을 찾는 것이라면
                        return _team2HpText;
                    else
                        return _team1HpText;
                default:
                    return null;
            }
        }
    }
}
// 마지막 작성 일자: 2026.03.25