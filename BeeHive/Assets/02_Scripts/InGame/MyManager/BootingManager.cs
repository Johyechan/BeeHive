using DG.Tweening;
using InGame.MyManager.Boot;
using MyUtil;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace InGame.MyManager
{
    // 작성자: 조혜찬
    // 게임 시작 전 필요한 기능들이 전부 활성화 됐는지 + 필요한 검증이 끝났는지 확인하는 클래스
    public class BootingManager : MonoSingleton<BootingManager>
    {
        private SteamChecker _steamChecker; // 스팀 관련 검증 클래스

        private SteamIDChecker _steamIDChecker; // 스팀ID 검증 클래스

        private GpuChecker _gpuChecker; // gpu 관련 검증 클래스

        private Queue<CheckerBase> _checkerQueue = new Queue<CheckerBase>(); // 검증 클래스 큐

        [SerializeField] private CanvasGroup _gameQuitUICanvasGrop; // 게임 강제 종료 UI
        [SerializeField] private TMP_Text _gameQuitTxt; // 게임 강제 종료 이유 text

        [SerializeField] private float _fadeDuration; // 페이드 지속 시간

        private TaskCompletionSource<bool> _steamAuthEnd;

        public void CreateSteamAuthEndTcs()
        {
            if (_steamAuthEnd != null)
            {
                _steamAuthEnd = null;
            }

            _steamAuthEnd = new TaskCompletionSource<bool>();
        }

        public Task WaitSteamAuth() => _steamAuthEnd.Task;

        protected override async void Awake()
        {
            _steamChecker = new SteamChecker();
            _steamIDChecker = new SteamIDChecker();
            _gpuChecker = new GpuChecker();

            _checkerQueue.Enqueue(_steamChecker);
            _checkerQueue.Enqueue(_steamIDChecker);
            _checkerQueue.Enqueue(_gpuChecker);

            await NetworkManager.Instance.WaitSocketConnected(); // 서버 연결 대기

            NetworkManager.Instance.Socket.On("steamAuthFailed", msg =>
            {
                _steamAuthEnd?.SetResult(true); // 스팀 인증 종료

                MainThreadDispatcher.Enqueue(() =>
                {
                    _gameQuitTxt.text = msg.GetValue<string>(); // 게임 강제 종료 이유 할당
                    _gameQuitUICanvasGrop.gameObject.SetActive(true); // 게임 강제 종료 UI 활성화
                    _gameQuitUICanvasGrop.DOFade(1, _fadeDuration); // 게임 강제 종료 UI 페이드 인
                });
            });

            NetworkManager.Instance.Socket.On("steamAuthSuccess", nickName => // 스팀 인증 성공 시
            {
                _steamAuthEnd?.SetResult(true); // 스팀 인증 종료
                NetworkManager.Instance.CurrentClientName = nickName.GetValue<string>(); // 닉네임 저장
            });

            foreach(var checker in _checkerQueue)
            {
                await checker.Init();
            }

            SceneManager.LoadScene(1); // 로비 씬으로 이동
        }
    }
}
// 마지막 작성 일자: 2025.01.02