using DG.Tweening;
using MyUtil;
using SocketIOClient;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace InGame.MyManager.Boot.Handler
{
    // 작성자: 조혜찬
    // 스팀 인증 실패 핸들러
    public class SteamAuthFailedHandler
    {
        private TMP_Text _gameQuitTxt; // 게임 강제 종료 이유 텍스트

        private CanvasGroup _gameQuitUICanvasGroup; // 게임 강제 종료 UI

        private float _fadeDuration; // 페이드 인, 아웃에 걸리는 시간

        public SteamAuthFailedHandler(TMP_Text gameQuitTxt, CanvasGroup gameQuitUICanvasGroup, float fadeDuration)
        {
            _gameQuitTxt = gameQuitTxt;
            _gameQuitUICanvasGroup = gameQuitUICanvasGroup;
            _fadeDuration = fadeDuration;
        }

        // 스팀 인증 실패 시 실행될 함수
        public void OnSteamAuthFailed(SocketIOResponse response)
        {
            if (NetworkManager.Instance.IsClientOver) // 클라이언트가 종료 되었다면
                return; // 반환

            MainThreadDispatcher.Enqueue(() =>
            {
                _gameQuitTxt.text = response.GetValue<string>(); // 게임 강제 종료 이유 할당
                _gameQuitUICanvasGroup.gameObject.SetActive(true); // 게임 강제 종료 UI 활성화
                _gameQuitUICanvasGroup.DOFade(1, _fadeDuration); // 게임 강제 종료 UI 페이드 인
            });
        }
    }
}
// 마지막 작성 일자: 2026.01.30