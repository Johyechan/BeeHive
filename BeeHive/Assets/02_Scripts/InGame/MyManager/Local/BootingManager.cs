using InGame.MyManager.Boot.Struct;
using InGame.MyManager.Global;
using InGame.MyManager.Local.Boot;
using InGame.MyManager.Local.Boot.Variable;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Localization.Settings;
using InGame.MyManager.Enum;

namespace InGame.MyManager.Local
{
    // 작성자: 조혜찬
    // 게임 시작 전 필요한 기능들이 전부 활성화 됐는지 + 필요한 검증이 끝났는지 확인하는 클래스
    public class BootingManager : MonoBehaviour
    {
        [SerializeField] private BootingMgrData _data; // Inspector 튜닝이 필요한 변수들을 가지는 구조체

        private BootingMgrVariables _variables = new BootingMgrVariables(); // 부팅 매니저에 필요한 변수들을 가지는 클래스
        public BootingMgrVariables Variables { get => _variables; } // 부팅 매니저에 필요한 변수들을 가지는 클래스 프로퍼티

        public void CreateSteamAuthEndTcs()
        {
            if (_variables.steamAuthEnd != null)
            {
                _variables.steamAuthEnd = null;
            }

            _variables.steamAuthEnd = new TaskCompletionSource<bool>();
        }

        public async Task<bool> WaitSteamAuth()
        {
            return await _variables.steamAuthEnd.Task; // 완료 될 때까지 대기, 결과 bool 반환
        }

        private async void Awake()
        {
            _variables.Init(this, _data.gameQuitTxt, _data.gameQuitUICanvasGroup, _data.languageSelectCanvasGroup, _data.fadeDuration); // 변수 초기화 함수

            _variables.globalManagersSetChecker = new GlobalManagersSetChecker(_data.managerList, _data.waitTime); // 글로벌 매니저 인스턴스 생성 검증
            _variables.steamIDChecker = new SteamIDChecker(this); // 스팀 ID 검증
            _variables.gpuChecker = new GpuChecker(_data.waitTime); // gpu 세팅 검증

            _variables.checkerQueue.Enqueue(_variables.globalManagersSetChecker); // 1 순위 체크
            _variables.checkerQueue.Enqueue(_variables.steamIDChecker); // 2 순위 체크
            _variables.checkerQueue.Enqueue(_variables.gpuChecker); // 3 순위 체크

            LocalizationSettings.InitializationOperation.WaitForCompletion(); // 번역 테이블 대기

            await NetworkManager.Instance.WaitSocketConnected(); // 서버 연결 대기

            NetworkManager.Instance.Socket.On("steamAuthFailed", msg =>
            {
                _variables.steamAuthEnd?.TrySetResult(false); // 스팀 인증 실패

                _variables.steamAuthFailedHandler.OnSteamAuthFailed(msg);
            }); // 스팀 인증 실패 구독

            NetworkManager.Instance.Socket.On("steamAuthSuccess", response =>
            {
                _variables.steamAuthSuccessHandler.OnSteamAuthSuccess(response);
            }); // 스팀 인증 성공 구독

            foreach (var checker in _variables.checkerQueue) // 게임 실행을 위한 검증 실행
            {
                _variables.result = await checker.Init(); // 각 검증 완료 대기

                if (!this || !gameObject) // 자기 자신이 없을 경우
                {
                    Application.Quit(); // 강제종료
                    return;
                }

                if (!_variables.result) // 검증을 실패 했다면
                    break; // 반복문 탈출
            }

            if(!_variables.isNewUser && _variables.result) // 신규 유저가 아니면서 검증에 성공했다면
            {
                if(!_variables.isTutorialOver) // 튜토리얼이 끝나지 않았을 경우
                {
                    SceneMgr.Instance.ChangeCurrentSceneFlow(SceneFlowType.GoTutorial); // 튜토리얼 씬으로 이동하는 흐름으로 변경
                    SceneMgr.Instance.LoadScene(); // 씬 전환
                    return;
                }

                SceneMgr.Instance.ChangeCurrentSceneFlow(SceneFlowType.GoLobby); // 로비 씬으로 이동하는 흐름으로 변경
                SceneMgr.Instance.LoadScene(); // 씬 전환
                return;
            }

            if(!_variables.result)
            {
                Application.Quit();
            }
        }

        private void OnDisable()
        {
            NetworkManager.Instance.Socket.Off("steamAuthFailed"); // 스팀 인증 실패 구독 해제
            NetworkManager.Instance.Socket.Off("steamAuthSuccess"); // 스팀 인증 성공 구독 해제
        }
    }
}
// 마지막 작성 일자: 2026.04.19