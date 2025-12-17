using DG.Tweening;
using InGame.MyEnum;
using InGame.MyEvent;
using InGame.MyManager;
using UnityEngine;

namespace InGame.MySystem.Loading
{
    // 작성자: 조혜찬
    // 로딩 시스템
    public class GameSceneLoadingSystem : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _loadingCanvasGroup; // 로딩 UI

        [SerializeField] private float _animationDuration; // 애니메이션 지속 시간

        private async void Awake()
        {
            await TeamManager.Instance.TeamSetTcs.Task; // 팀이 정해질 때까지 대기

            await CameraManager.Instance.SetCamera(TeamManager.Instance.CurrentTeamType); // 카메라 세팅

            await _loadingCanvasGroup.DOFade(0, _animationDuration).OnComplete(() => _loadingCanvasGroup.gameObject.SetActive(false)).AsyncWaitForCompletion(); // 로딩 ui 닫기

            await TurnManager.Instance.TurnChange(TurnType.ChangeTeam, true); // 처음 팀을 알려주기 위해서 현재 팀으로 체인지

            GameReady.Completed(); // 게임 준비 완료
        }
    }
}
// 마지막 작성 일자: 2025.12.17