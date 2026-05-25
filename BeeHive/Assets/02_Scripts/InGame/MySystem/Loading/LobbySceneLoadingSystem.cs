using DG.Tweening;
using InGame.MyManager.Global;
using InGame.MySystem.Lobby;
using UnityEngine;

namespace InGame.MySystem.Loading
{
    // 작성자: 조혜찬
    // 로비 씬 로딩 관리

    public class LobbySceneLoadingSystem : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _loadingUICanvasGroup; // 로딩 UI 창

        [SerializeField] private float _animationDuration; // 애니메이션 지속 시간

        [SerializeField] private LobbySetting _lobbySetting; // 로비 세팅

        private async void Awake()
        {
            await LobbyReady.Gate.WaitAsync(); // 로비 준비 대기

            _lobbySetting.CheckAppLicense();

            await _loadingUICanvasGroup.DOFade(0, _animationDuration).AsyncWaitForCompletion(); // 로딩 창 닫기

            if (NetworkManager.Instance.IsClientOver) // 클라이언트가 종료 되었다면
                return; // 반환

            _loadingUICanvasGroup.gameObject.SetActive(false); // 비활성화
        }
    }
}
// 마지막 작성 일자: 2026.05.25