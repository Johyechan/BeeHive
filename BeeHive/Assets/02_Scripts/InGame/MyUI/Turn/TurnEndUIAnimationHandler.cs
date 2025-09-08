using DG.Tweening;
using InGame.MyEvent;
using InGame.MyManager;
using InGame.MySystem;
using TMPro;
using UnityEngine;

namespace InGame.MyUI.Turn
{
    // 작성자: 조혜찬
    // 턴 종료에 나올 UI 애니메이션 클래스
    public class TurnEndUIAnimationHandler : TurnUIAnimationHandlerBase
    {
        public TurnEndUIAnimationHandler(CanvasGroup canvasGroup, TMP_Text tmpText, float animationDuration) : base(canvasGroup, tmpText, animationDuration)
        {
        }

        public override Sequence UIAnimationPlay()
        {
            Sequence seq = DOTween.Sequence()
                .AppendCallback(() => TurnEvents.OnSetInteractable?.Invoke(false)) // 턴 넘기기 버튼 상화작용 비활성화
                .AppendCallback(() => _tmpText.text = "턴 종료") // 무슨 턴인지 텍스트로 보여주기
                .Append(base.UIAnimationPlay()) // 이후 동일하게 실행되어야 할 기능 수행
                .AppendCallback(() =>
                {
                    Transform parent = GameObject.Find(TeamManager.Instance.RoadParentName).transform;
                    PieceEvents.OnDestroyLeftRoad?.Invoke(parent, TeamManager.Instance.CurrentTeamType);

                    DestroyLeftRoadInfo destroyLeftRoadInfo = new DestroyLeftRoadInfo()
                    {
                        roomID = SceneMgr.Instance.CurrentRoomID, // 현재 방 ID
                        roadParentName = parent.name, // 사용하지 않아 파괴될 도로 객체들의 부모 객체 명
                        teamType = (int)TeamManager.Instance.CurrentTeamType // 파괴되는 도로의 팀 타입
                    };

                    string json = JsonUtility.ToJson(destroyLeftRoadInfo); // Json으로 변환

                    NetworkManager.Instance.Socket.Emit("destroyLeftRoad", json); // 서버에 이벤트 전달
                }); // 사용하지 않은 도로 전부 삭제 

            return seq;
        }
    }
}
// 마지막 작성 일자: 2025.09.02