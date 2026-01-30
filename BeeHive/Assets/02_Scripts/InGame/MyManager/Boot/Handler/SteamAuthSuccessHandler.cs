using DG.Tweening;
using InGame.MyUI;
using MyUtil;
using SocketIOClient;
using System.Threading.Tasks;
using UnityEngine;

namespace InGame.MyManager.Boot.Handler
{
    // 작성자: 조혜찬
    // 스팀 인증 성공 핸들러
    public class SteamAuthSuccessHandler
    {
        private CanvasGroup _makeNickNameCanvasGroup; // 닉네임 생성 UI

        private float _fadeDuration; // 페이드 인, 아웃에 걸리는 시간

        public SteamAuthSuccessHandler(CanvasGroup makeNickNameCanvasGroup, float fadeDuration)
        {
            _makeNickNameCanvasGroup = makeNickNameCanvasGroup;
            _fadeDuration = fadeDuration;
        }

        // 스팀 인증 성공 시 실행될 함수
        public void OnSteamAuthSuccess(SocketIOResponse response)
        {
            if (NetworkManager.Instance.IsClientOver) // 클라이언트가 종료 되었다면
                return; // 반환

            string strNickName = response.GetValue<string>();

            if (string.IsNullOrEmpty(strNickName)) // 닉네임이 비어있을 경우
            {
                BootingManager.Instance.Variables.isNewUser = true; // 신규 유저 입니다

                MainThreadDispatcher.Enqueue(() =>
                {
                    _makeNickNameCanvasGroup.gameObject.SetActive(true); // 닉네임 생성 UI 활성화
                    _makeNickNameCanvasGroup.DOFade(1, _fadeDuration); // 닉네임 생성 UI 페이드 인
                });
            }
            else // 닉네임이 있을 경우
                NetworkManager.Instance.CurrentClientName = strNickName; // 닉네임 저장
        }
    }
}
// 마지막 작성 일자: 2026.01.30