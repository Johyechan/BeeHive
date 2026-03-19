using InGame.MyEnum;
using InGame.MyEvent;
using InGame.MyManager;
using InGame.MyManager.Global;
using InGame.MyManager.Local;
using InGame.MyManager.MyPiece;
using InGame.MyObject;
using InGame.MyObject.Piece;
using InGame.MyUI;
using MyUtil;
using MyUtil.GameMode;
using System.Threading.Tasks;
using UnityEngine;

namespace InGame.MySystem.Game.Handler
{
    // 작성자: 조혜찬
    // 게임 관련 소켓 이벤트 연결 핸들러 클래스
    public class GameSocketEventHandler : BaseSocketEventHandler
    {
        public override void OnConnect()
        {
            NetworkManager.Instance.Socket.On("drought", (value) =>
            {
                if (NetworkManager.Instance.IsClientOver) // 클라이언트가 종료 되었다면
                    return; // 반환

                int isDrought = value.GetValue<int>();
                MainThreadDispatcher.Enqueue(() =>
                {
                    InGameContext.Current.Data.PieceManager.IsDrought = isDrought == 1; // 가뭄 여부 변경 - isDrought가 1일 경우 참, 1이 아닐 경우 거짓 할당
                });
            });

            NetworkManager.Instance.Socket.On("castleHpChanged", (value) =>
            {
                if (NetworkManager.Instance.IsClientOver) // 클라이언트가 종료 되었다면
                    return; // 반환

                string json = value.GetValue().ToString();
                CastleHpChangeInfo castleHpChangeInfo = JsonUtility.FromJson<CastleHpChangeInfo>(json); // Json 값 변환

                MainThreadDispatcher.Enqueue(() =>
                {
                    TeamType hpChangedCastleTeamType = (TeamType)castleHpChangeInfo.changeTeamType; // 서버에서 받은 int형식 변수를 TeamType enum 값으로 변경
                    Castle hpChangedCastle = TeamManager.Instance.GetCastle(hpChangedCastleTeamType); // 체력이 올라간 팀에 맞는 성 가져오기
                    hpChangedCastle.CastleUpgrade(castleHpChangeInfo.changedHp); // 체력 증가
                });
            });

            NetworkManager.Instance.Socket.On("isGameOver", value =>
            {
                if (NetworkManager.Instance.IsClientOver) // 클라이언트가 종료 되었다면
                    return; // 반환

                int loseTeamType = value.GetValue<int>();

                MainThreadDispatcher.Enqueue(() =>
                {
                    Time.timeScale = 0; // 시간 멈춤
                    GameOverEvent.OnGameOver?.Invoke();
                    InGameContext.Current.Data.GameManager.GameIsOver((TeamType)loseTeamType); // 게임 오버
                });
            });

            NetworkManager.Instance.Socket.On("castleHit", (value) =>
            {
                if (NetworkManager.Instance.IsClientOver) // 클라이언트가 종료 되었다면
                    return; // 반환

                string json = value.GetValue().ToString();
                CastleHitInfo castleHitInfo = JsonUtility.FromJson<CastleHitInfo>(json);

                MainThreadDispatcher.Enqueue(() =>
                {
                    Castle castle = TeamManager.Instance.GetCastle((TeamType)castleHitInfo.attackedCaslteType); // 공격 받은 성 받아오기
                    castle.CastleHit(castleHitInfo.damage); // 성 공격
                    GameObject attackObj = ObjectIdManager.Instance.FindObject(castleHitInfo.objectID); // 공격한 기물 탐색
                    PieceBase pieceBase = attackObj.GetComponent<PieceBase>(); // 공격한 기물에게서 pieceBase 가져오기
                    pieceBase.PieceDestroy(); // 공격한 기물 파괴
                });
            });

            NetworkManager.Instance.Socket.On("tankAttacked", async (value) =>
            {
                if (NetworkManager.Instance.IsClientOver) // 클라이언트가 종료 되었다면
                    return; // 반환

                TaskCompletionSource<bool> confirmResultTcs = new TaskCompletionSource<bool>();

                MainThreadDispatcher.Enqueue(() =>
                {
                    if (InGameContext.Current.Data.CardManager.HaveFirePowerCard) // 화력 카드를 가지고 있다면
                    {
                        ConfirmUI confirmUI = Object.FindAnyObjectByType<ConfirmUI>(FindObjectsInactive.Include); // 확인 UI 가져오기

                        confirmUI.gameObject.SetActive(true); // 확인 UI 활성화
                        confirmUI.Confirm(result =>
                        {
                            confirmUI.ConfirmEnd();
                            confirmResultTcs.TrySetResult(result);
                        },
                        "상대 전차에게 공격 당했습니다. \n 화력 카드를 사용하여 방어 하시겠습니까?");
                    }
                    else // 화력 카드를 가지고 있지 않을 경우
                    {
                        confirmResultTcs.SetResult(false);
                    }
                });

                bool result = await confirmResultTcs.Task; // 결과 대기

                if (result) // 화력 카드를 사용해서 방어를 선택했다면
                {
                    if (GameModeManager.Instance.CurrentGameMode.UseServer())
                        NetworkManager.Instance.Socket.Emit("chooseDefense", SceneMgr.Instance.CurrentRoomID); // 방어하지 않는 것을 선택했다고 서버 호출
                }
                else // 화력 카드를 사용하지 않아 방어를 선택하지 않았다면
                {
                    if (GameModeManager.Instance.CurrentGameMode.UseServer())
                        NetworkManager.Instance.Socket.Emit("chooseNoDefense", SceneMgr.Instance.CurrentRoomID); // 방어하지 않는 것을 선택했다고 서버 호출
                }
            });
        }

        public override void OnDisconnect()
        {
            NetworkManager.Instance.Socket.Off("drought");
            NetworkManager.Instance.Socket.Off("castleHpChanged");
            NetworkManager.Instance.Socket.Off("isGameOver");
            NetworkManager.Instance.Socket.Off("castleHit");
            NetworkManager.Instance.Socket.Off("tankAttacked");
        }
    }
}
// 마지막 작성 일자: 2026.03.19