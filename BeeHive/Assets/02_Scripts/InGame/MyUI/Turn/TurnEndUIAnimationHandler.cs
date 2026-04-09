using DG.Tweening;
using InGame.MyEvent;
using InGame.MyManager;
using InGame.MyManager.Global;
using InGame.MyManager.Local;
using InGame.MySystem;
using MyUtil.GameMode;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace InGame.MyUI.Turn
{
    // 작성자: 조혜찬
    // 턴 종료에 나올 UI 애니메이션 클래스
    public class TurnEndUIAnimationHandler : TurnUIAnimationHandlerBase
    {
        public TurnEndUIAnimationHandler(CanvasGroup canvasGroup, TMP_Text tmpText, float animationDuration) : base(canvasGroup, tmpText, animationDuration)
        {
        }

        public override async Task UIAnimationPlay()
        {
            string turnEnd = LocalizationSettings.StringDatabase.GetLocalizedString(
                "Game",
                "Game_UI_TurnEnd"
            );
            await DOTween.Sequence()
                .AppendCallback(() => TurnEvents.OnSetInteractable?.Invoke(false)) // 턴 넘기기 버튼 상화작용 비활성화
                .AppendCallback(() => _tmpText.text = turnEnd) // 무슨 턴인지 텍스트로 보여주기
                .AsyncWaitForCompletion(); // 이후 동일하게 실행되어야 할 기능 수행

            await base.UIAnimationPlay();

            await DOTween.Sequence()
                .AppendCallback(() =>
                {
                    Transform parent = null;

                    if (GameModeManager.Instance.CurrentGameMode.IsTutorial()) // 튜토리얼 일 경우
                    {
                        parent = TeamManager.Instance.GetRoadTransform(InGameContext.Current.Data.TurnManager.CurrentTeamType); // 턴 매니저가 현재 팀을 판단
                        PieceEvents.OnDestroyLeftRoad?.Invoke(parent, InGameContext.Current.Data.TurnManager.CurrentTeamType);
                    }
                    else // 튜토리얼이 아닐 경우 
                    {
                        parent = TeamManager.Instance.GetRoadTransform(TeamManager.Instance.CurrentTeamType); // 팀 매니저가 현재 팀을 판단(자기 자신만 판단)
                        PieceEvents.OnDestroyLeftRoad?.Invoke(parent, TeamManager.Instance.CurrentTeamType);
                    }

                    DestroyLeftRoadInfo destroyLeftRoadInfo = new DestroyLeftRoadInfo()
                    {
                        roomID = SceneMgr.Instance.CurrentRoomID, // 현재 방 ID
                        roadParentName = parent.name, // 사용하지 않아 파괴될 도로 객체들의 부모 객체 명
                        teamType = (int)TeamManager.Instance.CurrentTeamType // 파괴되는 도로의 팀 타입
                    };

                    string json = JsonUtility.ToJson(destroyLeftRoadInfo); // Json으로 변환

                    if (GameModeManager.Instance.CurrentGameMode.UseServer())
                        NetworkManager.Instance.Socket.Emit("destroyLeftRoad", json); // 서버에 이벤트 전달
                }).AsyncWaitForCompletion(); // 사용하지 않은 도로 전부 삭제

        }
    }
}
// 마지막 작성 일자: 2026.04.09