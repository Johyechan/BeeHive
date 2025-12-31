using InGame.MyManager.Boot;
using MyUtil;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace InGame.MyManager
{
    // 작성자: 조혜찬
    // 게임 시작 전 필요한 기능들이 전부 활성화 됐는지 + 필요한 검증이 끝났는지 확인하는 클래스
    public class BootingManager : MonoSingleton<BootingManager>
    {
        private SteamChecker _steamChecker; // 스팀 관련 검증 클래스

        private GpuChecker _gpuChecker; // gpu 관련 검증 클래스

        private Queue<CheckerBase> _checkerQueue = new Queue<CheckerBase>(); // 검증 클래스 큐

        protected override async void Awake()
        {
            _steamChecker = new SteamChecker();
            _gpuChecker = new GpuChecker();

            _checkerQueue.Enqueue(_steamChecker);
            _checkerQueue.Enqueue(_gpuChecker);

            await NetworkManager.Instance.WaitSocketConnected(); // 서버 연결 대기

            foreach(var checker in _checkerQueue)
            {
                await checker.Init();
            }

            SceneManager.LoadScene(1); // 로비 씬으로 이동
        }
    }
}
// 마지막 작성 일자: 2025.12.29