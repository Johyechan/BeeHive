using DG.Tweening;
using InGame.MyEnum;
using InGame.MyManager;
using InGame.MyManager.Global;
using InGame.MyManager.Local;
using MyUtil.GameMode;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Tutorial;
using Tutorial.MyEnum;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace InGame.MyObject
{
    // 작성자: 조혜찬
    // 성 클래스(지켜야할 성)
    public class Castle : MonoBehaviour
    {
        [SerializeField] private TMP_Text _team1HpText; // Team1 성의 체력을 알려주는 UI
        [SerializeField] private TMP_Text _team2HpText; // Team2 성의 체력을 알려주는 UI

        [SerializeField] private GameObject _blockImage; // 클릭 방지 이미지

        [SerializeField] private TeamType _castleTeamType; // 성의 팀 타입

        [SerializeField] private int _hp; // 체력

        [SerializeField] private float _hitAnimationDuration; // 히트 애니메이션 지속 시간

        private List<Material> _castleMaterials = new List<Material>(); // 성 머티리얼 리스트

        private int _opponentHp; // 상대 체력
        private int _currentHp; // 현재 체력
        public int CurrentHp { get => _currentHp; } // 위 변수 프로퍼티

        private async void Awake()
        {
            // 현재 체력을 최대 체력으로 할당
            _currentHp = _hp;
            _opponentHp = _hp;

            await TeamReady.Gate.WaitAsync(); // 팀 할당 대기

            if (NetworkManager.Instance.IsClientOver) // 클라이언트가 종료 되었다면
                return; // 반환

            var renderers = transform.GetComponentsInChildren<Renderer>(); // 성 머티리얼을 가지는 객체들의 랜더러를 가져오기
            foreach(var renderer in  renderers)
            {
                _castleMaterials.Add(renderer.material); // 객체의 머티리얼(공용 머티리얼을 가져오지 않는다) 추가
            }

            if (GameModeManager.Instance.CurrentGameMode.IsTutorial()) // 튜토리얼 일 경우
            {
                if (_castleTeamType == TeamManager.Instance.CurrentTeamType) // 자기 성일 경우
                {
                    string me = LocalizationSettings.StringDatabase.GetLocalizedString(
                        "Tutorial",
                        "Tutorial_UI_MeHPText"
                    );
                    GetCastleHpTmpTxt().text = $"{me} - {_currentHp} HP"; // UI 적용
                }
                else // 자기 성이 아닐 경우
                {
                    string opponent = LocalizationSettings.StringDatabase.GetLocalizedString(
                        "Tutorial",
                        "Tutorial_UI_OpponentHPText"
                    );
                    GetCastleHpTmpTxt(false).text = $"{opponent} - {_opponentHp} HP"; // UI 적용
                }
            }
            else // 튜토리얼이 아닐 경우
            {
                if (_castleTeamType == TeamManager.Instance.CurrentTeamType) // 자기 성일 경우
                {
                    GetCastleHpTmpTxt().text = $"{NetworkManager.Instance.CurrentClientName} - {_currentHp} HP"; // UI 적용
                }
                else // 자기 성이 아닐 경우
                {
                    GetCastleHpTmpTxt(false).text = $"{SceneMgr.Instance.OtherNickName} - {_opponentHp} HP"; // UI 적용
                }
            }
        }

        public async void CastleHit(int damage)
        {
            bool isGameOver = false; // 게임 종료 여부

            if(_castleTeamType == TeamManager.Instance.CurrentTeamType) // 내 팀일 경우
            {
                _currentHp -= damage;
                if (_currentHp <= 0)
                {
                    _currentHp = 0;
                    isGameOver = true; // 게임 오버
                }
            }
            else // 상대 팀일 경우
            {
                _opponentHp -= damage;
                if (_opponentHp <= 0)
                {
                    _opponentHp = 0;
                    isGameOver = true;
                }
            }

            await HitAnimation(damage); // 히트 애니메이션 실행

            if(GameModeManager.Instance.CurrentGameMode.IsTutorial()) // 튜토리얼 일 경우
            {
                if (_castleTeamType == TeamManager.Instance.CurrentTeamType) // 자기 성일 경우
                {
                    string me = LocalizationSettings.StringDatabase.GetLocalizedString(
                        "Tutorial",
                        "Tutorial_UI_MeHPText"
                    );

                    GetCastleHpTmpTxt().text = $"{me} - {_currentHp} HP"; // UI 적용
                }
                else // 자기 성이 아닐 경우
                {
                    string opponent = LocalizationSettings.StringDatabase.GetLocalizedString(
                        "Tutorial",
                        "Tutorial_UI_OpponentHPText"
                    );

                    GetCastleHpTmpTxt(false).text = $"{opponent} - {_opponentHp}  HP"; // UI 적용
                }

                if(isGameOver) // 체력이 0 이하라면
                {
                    var tcs = InGameContext.Current.Data.CardManager.UsedCardMoveToUsedCardDeck;

                    if(tcs != null)
                    {
                        await tcs.Task; // 사용한 카드가 사용한 카드들을 모아두는 덱으로 갈 때까지 대기
                    }
                    else
                    {
                        NetworkManager.Instance.Socket.Emit("debug", "사용한 카드 이동 대기 tcs가 존재하지 않습니다.");
                    }

                    TutorialManager.Instance.ChangeTutorialState(TutorialState.End); // 튜토리얼 종료 상태로 이동
                    InGameContext.Current.Data.GameManager.GameIsOver(_castleTeamType); // 게임 오버
                }
            }
            else // 튜토리얼이 아닐 경우
            {
                if (_castleTeamType == TeamManager.Instance.CurrentTeamType) // 자기 성일 경우
                {
                    GetCastleHpTmpTxt().text = $"{NetworkManager.Instance.CurrentClientName} - {_currentHp} HP"; // UI 적용
                }
                else // 자기 성이 아닐 경우
                {
                    GetCastleHpTmpTxt(false).text = $"{SceneMgr.Instance.OtherNickName} - {_opponentHp}  HP"; // UI 적용
                }
            }

            if (isGameOver) // 현재 체력이 0 이하라면
            {
                var tcs = InGameContext.Current.Data.CardManager.UsedCardMoveToUsedCardDeck;

                if(tcs != null)
                {
                    await tcs.Task; // 사용한 카드가 사용한 카드들을 모아두는 덱으로 갈 때까지 대기
                }
                else
                {
                    NetworkManager.Instance.Socket.Emit("debug", "사용한 카드 이동 대기 tcs가 존재하지 않습니다.");
                }

                _blockImage.SetActive(true); // 클릭 방지 이미지 활성화

                int loseTeamType = 0;
                
                if(_currentHp <= 0) // 내 체력이 0 이하라면
                {
                    loseTeamType = (int)TeamManager.Instance.CurrentTeamType; // 현재 클라이언트의 팀이 패배
                }
                else if(_opponentHp <= 0) // 상대 체력이 0 이하라면
                {
                    switch(TeamManager.Instance.CurrentTeamType) // 내 팀이
                    {
                        case TeamType.Team1: // 팀 1일 때
                            loseTeamType = (int)TeamType.Team2; // 진 팀은 팀 2
                            break;
                        case TeamType.Team2: // 팀 2일 때
                            loseTeamType = (int)TeamType.Team1; // 진 팀은 팀 1
                            break;
                    }
                }

                GameOverInfo gameOverInfo = new GameOverInfo()
                {
                    roomID = SceneMgr.Instance.CurrentRoomID, // 현재 방 ID
                    loseTeamType = loseTeamType, // 패배 팀 타입
                    isSurrender = 0 // 항복 여부 (0 = false)
                };
                string json = JsonUtility.ToJson(gameOverInfo); // Json으로 변환
                if (GameModeManager.Instance.CurrentGameMode.UseServer())
                    NetworkManager.Instance.Socket.Emit("gameOver", json);
            }
        }

        private async Task HitAnimation(int damage)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>(); // 코루틴 완료 대기 tcs
            StartCoroutine(HitAnimationCo(tcs));
            await tcs.Task; // 코루틴 종료 대기
        }

        private IEnumerator HitAnimationCo(TaskCompletionSource<bool> tcs)
        {
            foreach (var material in _castleMaterials) // 성을 붉게 만들기
            {
                material.DOColor(Color.red, "_BaseColor", _hitAnimationDuration);
            }

            yield return new WaitForSeconds(_hitAnimationDuration);

            foreach (var material in _castleMaterials) // 성을 원상태로 돌리기
            {
                material.DOColor(Color.white, "_BaseColor", _hitAnimationDuration);
            }

            tcs.SetResult(true); // 코루틴 완료 tcs에 종료 할당
        }


        // 성 강화 함수(최대 체력 1증가)
        public void CastleUpgrade(int currentHp = 0)
        {
            if(_castleTeamType == TeamManager.Instance.CurrentTeamType) // 자신의 성이라면
            {
                _currentHp++; // 현재 체력 증가
                GetCastleHpTmpTxt().text = $"{NetworkManager.Instance.CurrentClientName} - {_currentHp}  HP"; // UI 적용
            }
            else // 상대의 성이라면
            {
                _opponentHp = currentHp; // 상대 체력을 변경
                GetCastleHpTmpTxt(false).text = $"{SceneMgr.Instance.OtherNickName} - {_opponentHp} HP"; // UI 적용
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
// 마지막 작성 일자: 2026.06.25