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
        private SteamIDChecker _steamIDChecker; // 스팀ID 검증 클래스

        private GpuChecker _gpuChecker; // gpu 관련 검증 클래스

        private Queue<CheckerBase> _checkerQueue = new Queue<CheckerBase>(); // 검증 클래스 큐

        [SerializeField] private CanvasGroup _gameQuitUICanvasGroup; // 게임 강제 종료 UI
        [SerializeField] private CanvasGroup _makeNickNameCanvasGroup; // 닉네임 생성 UI
        [SerializeField] private TMP_Text _gameQuitTxt; // 게임 강제 종료 이유 text

        [SerializeField] private float _fadeDuration; // 페이드 지속 시간

        private TaskCompletionSource<bool> _steamAuthEnd;

        private int _sceneNumber = 1; // 넘어갈 씬 번호 - 로비 씬(1)으로 이동

        private bool _isNewUser = false; // 신규 유저 여부
        private bool _result; // 검증 실패 여부

        public void CreateSteamAuthEndTcs()
        {
            if (_steamAuthEnd != null)
            {
                _steamAuthEnd = null;
            }

            _steamAuthEnd = new TaskCompletionSource<bool>();
        }

        public async Task<bool> WaitSteamAuth()
        {
            return await _steamAuthEnd.Task; // 완료 될 때까지 대기, 결과 bool 반환
        }

        protected override async void Awake()
        {
            _steamIDChecker = new SteamIDChecker();
            _gpuChecker = new GpuChecker();

            _checkerQueue.Enqueue(_steamIDChecker);
            _checkerQueue.Enqueue(_gpuChecker);

            await NetworkManager.Instance.WaitSocketConnected(); // 서버 연결 대기

            NetworkManager.Instance.Socket.On("steamAuthFailed", msg =>
            {
                if (NetworkManager.Instance.IsClientOver) // 클라이언트가 종료 되었다면
                    return; // 반환

                _steamAuthEnd?.SetResult(false); // 스팀 인증 종료

                MainThreadDispatcher.Enqueue(() =>
                {
                    _gameQuitTxt.text = msg.GetValue<string>(); // 게임 강제 종료 이유 할당
                    _gameQuitUICanvasGroup.gameObject.SetActive(true); // 게임 강제 종료 UI 활성화
                    _gameQuitUICanvasGroup.DOFade(1, _fadeDuration); // 게임 강제 종료 UI 페이드 인
                });
            });

            NetworkManager.Instance.Socket.On("steamAuthSuccess", nickName => // 스팀 인증 성공 시
            {
                if (NetworkManager.Instance.IsClientOver) // 클라이언트가 종료 되었다면
                    return; // 반환

                _steamAuthEnd?.SetResult(true); // 스팀 인증 종료
                string strNickName = nickName.GetValue<string>();
                if(string.IsNullOrEmpty(strNickName)) // 닉네임이 비어있을 경우
                {
                    _isNewUser = true;
                    MainThreadDispatcher.Enqueue(() =>
                    {
                        _makeNickNameCanvasGroup.gameObject.SetActive(true); // 닉네임 생성 UI 활성화
                        _makeNickNameCanvasGroup.DOFade(1, _fadeDuration); // 닉네임 생성 UI 페이드 인
                    });
                }
                else // 닉네임이 있을 경우
                    NetworkManager.Instance.CurrentClientName = strNickName; // 닉네임 저장
            });

            foreach(var checker in _checkerQueue) // 게임 실행을 위한 검증 실행
            {
                _result = await checker.Init(); // 각 검증 완료 대기

                if (!this || !gameObject) // 자기 자신이 없을 경우
                    return; // 반환

                if (!_result) // 검증을 실패 했다면
                    break; // 반복문 탈출
            }

            if(!_isNewUser && _result) // 신규 유저가 아니면서 검증에 성공했다면
            {
                SceneManager.LoadScene(_sceneNumber); // 로비 씬으로 이동
            }
        }
    }
}
// 마지막 작성 일자: 2026.01.22