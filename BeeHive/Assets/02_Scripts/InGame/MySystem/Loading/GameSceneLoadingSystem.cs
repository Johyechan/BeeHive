using DG.Tweening;
using InGame.MyEnum;
using InGame.MyManager;
using InGame.MyManager.Global;
using InGame.MyManager.Local;
using InGame.MyManager.Team;
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
            UIManager.Instance.CanInteractionUI = true; // UI 상호작용 가능 상태로 초기화
            TeamManager.Instance.FirstTurn = true; // 첫 턴 상태로 할당

            InGameContext.Current.Data.GameMapManager.SetNetworkID(); // 네트워크 ID 할당

            await LocalManagerReady.Gate.WaitAsync(); // 씬 내 매니저 세팅 대기

            if(!SceneMgr.Instance.IsTutorial) // 튜토리얼 씬이 아닐 때
                await TeamManager.Instance.TeamSetTcs.Task; // 팀이 정해질 때까지 대기

            TeamReady.Gate.Completed(); // 팀 할당 완료

            await EventReady.WaitAsync(); // 이벤트 준비 완료까지 대기

            if (NetworkManager.Instance.IsClientOver) // 클라이언트가 종료 되었다면
                return; // 반환

            TeamManagerEvents.OnNeedTeamManagerEvent?.Invoke(); // 팀 매니저가 필요한 함수들 실행 이벤트 호출

            await InGameContext.Current.Data.CameraManager.SetCamera(TeamManager.Instance.CurrentTeamType); // 카메라 세팅

            await _loadingCanvasGroup.DOFade(0, _animationDuration).OnComplete(() => _loadingCanvasGroup.gameObject.SetActive(false)).AsyncWaitForCompletion(); // 로딩 ui 닫기

            if(TeamManager.Instance.CurrentTeamType == TeamType.Team1 && !SceneMgr.Instance.IsTutorial) // 팀 1이 턴 실행 요청(중복 방지) + 튜토리얼 씬이 아닐 경우
            {
                NetworkManager.Instance.Socket.Emit("turnStart", SceneMgr.Instance.CurrentRoomID);
            }

            GameReady.Gate.Completed(); // 게임 준비 완료
        }
    }
}
// 마지막 작성 일자: 2026.03.09