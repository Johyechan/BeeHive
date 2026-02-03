using InGame.MyManager.Boot.Handler;
using InGame.MyManager.Local.Boot.Handler;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace InGame.MyManager.Local.Boot.Variable
{
    // 작성자: 조혜찬
    // 부팅 매니저에 필요한 변수들을 가지는 클래스
    public class BootingMgrVariables
    {
        public GlobalManagersSetChecker globalManagersSetChecker; // 글로벌 싱글톤 매니저 세팅 완료 검증 클래스

        public SteamIDChecker steamIDChecker; // 스팀ID 검증 클래스
         
        public GpuChecker gpuChecker; // gpu 관련 검증 클래스
         
        public Queue<CheckerBase> checkerQueue = new Queue<CheckerBase>(); // 검증 클래스 큐
         
        public TaskCompletionSource<bool> steamAuthEnd;
         
        public int sceneNumber = 1; // 넘어갈 씬 번호 - 로비 씬(1)으로 이동
         
        public bool isNewUser = false; // 신규 유저 여부

        public bool result = false; // 검증 실패 여부

        public SteamAuthFailedHandler steamAuthFailedHandler; // 스팀 인증 실패 핸들러

        public SteamAuthSuccessHandler steamAuthSuccessHandler; // 스팀 인증 성공 핸들러

        // 초기화 함수
        public void Init(BootingManager bootingManager, TMP_Text gameQuitTxt, CanvasGroup gameQuitUICanvasGroup, CanvasGroup makeNickNameCanvasGroup, float fadeDuration)
        {
            steamAuthFailedHandler = new SteamAuthFailedHandler(gameQuitTxt, gameQuitUICanvasGroup, fadeDuration);
            steamAuthSuccessHandler = new SteamAuthSuccessHandler(bootingManager, makeNickNameCanvasGroup, fadeDuration);
        }
    }
}
// 마지막 작성 일자: 2026.02.03