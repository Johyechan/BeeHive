using InGame.MyEnum;
using InGame.MyEvent;
using InGame.MyManager;
using InGame.MyManager.Global;
using InGame.MyManager.Local;
using MyUtil.GameMode;
using System.Threading.Tasks;
using Tutorial;
using Tutorial.MyEnum;
using UnityEditor;
using UnityEngine;

namespace InGame.MySystem.Game
{
    // 작성자: 조혜찬
    // 생산 턴에 필요한 객체를 추가하는 기능들을 관리하는 클래스
    public class MakeTurnAddSystem
    {
        // 초기화 함수
        public void Init()
        {
            TurnEvents.OnMakeTurn.Add(GetGoldBar); // 생산 이벤트에 금괴 획득 함수 큐에 추가
            TurnEvents.OnMakeTurn.Add(GetRoad); // 생산 이벤트에 도로 추가 함수 큐에 추가
        }

        // 구독 해제 함수
        public void Disable()
        {
            TurnEvents.OnMakeTurn.Remove(GetGoldBar); // 생산 이벤트에 금괴 획득 함수 큐에서 제거
            TurnEvents.OnMakeTurn.Remove(GetRoad); // 생산 이벤트에 도로 추가 함수 큐에서 제거
        }

        // 금괴 획득 함수
        private async Task GetGoldBar()
        {
            if(GameModeManager.Instance.CurrentGameMode.IsTutorial())
            {
                switch (TutorialManager.Instance.CurrentTutorialState)
                {
                    case TutorialState.Turn1_Player: // 첫 번째 턴(플레이어 턴)
                        WalletEvent.OnGetGoldBar?.Invoke(2, false); // 금괴 2개 획득
                        break;
                    case TutorialState.Turn1_AI: // 첫 번째 턴(AI 턴)
                        WalletEvent.OnGetGoldBar?.Invoke(4, false); // 금괴 4개 획득
                        break;
                    default: // 다른 상태에서는
                        WalletEvent.OnGetGoldBar?.Invoke(2, false); // 금괴 2개 획득
                        break;
                }
            }
            else
            {
                if (IsReturn()) return; // 반환해야할 조건을 충족했을 경우 반환

                if (InGameContext.Current.Data.TurnManager.CurrentTeamType == TeamType.Team2) // 현재 차례인 팀이 블루팀(팀2)이라면
                {
                    if (TeamManager.Instance.Team2FirstTurn) // 블루팀(팀2)의 첫 번째 턴이라면
                    {
                        WalletEvent.OnGetGoldBar?.Invoke(4, false); // 금괴 4개 획득
                        TeamManager.Instance.Team2FirstTurn = false; // 블루팀(팀2)의 첫 번째 턴 상태 종료
                    }
                    else // 첫 번째 턴이 아니라면
                    {
                        WalletEvent.OnGetGoldBar?.Invoke(2, false); // 금괴 2개 획득
                    }
                }
                else // 레드팀(팀1)이라면
                {
                    WalletEvent.OnGetGoldBar?.Invoke(2, false); // 금괴 2개 획득
                }
                
            }

            await Task.CompletedTask;
        }

        private async Task GetRoad()
        {
            if(!GameModeManager.Instance.CurrentGameMode.IsTutorial()) // 튜토리얼이 아닐 때
            {
                if (IsReturn()) return; // 반환해야할 조건을 충족했을 경우 반환

                Transform roadParent = TeamManager.Instance.GetRoadTransform(TeamManager.Instance.CurrentTeamType);
                PieceEvents.OnGetRoad?.Invoke(2, TeamManager.Instance.CurrentTeamType, roadParent); // 도로 2개 획득

                AddRoadInfo addRoadInfo = new AddRoadInfo()
                {
                    roomID = SceneMgr.Instance.CurrentRoomID, // 현재 방 ID
                    roadCount = roadParent.transform.childCount, // 도로의 객체 수
                    teamType = (int)TeamManager.Instance.CurrentTeamType, // 팀 타입을 int형 강제형변환 후 저장
                    roadParentName = roadParent.name, // 현재 팀의 도로 객체 부모 명
                };

                string json = JsonUtility.ToJson(addRoadInfo); // 구조체를 Json 형태로 변환
                if (GameModeManager.Instance.CurrentGameMode.UseServer())
                    NetworkManager.Instance.Socket.Emit("addRoad", json); // 서버에 이벤트 전달
            }
            else // 튜토리얼 일 때
            {
                Transform roadParent = TeamManager.Instance.GetRoadTransform(InGameContext.Current.Data.TurnManager.CurrentTeamType); // 현재 턴 팀의 도로 부모 가져오기
                PieceEvents.OnGetRoad?.Invoke(2, InGameContext.Current.Data.TurnManager.CurrentTeamType, roadParent); // 도로 2개 획득(현재 턴의 팀)
            }

            if(InGameContext.Current.Data.TurnManager.RoadCreateCompletionTcs != null) // 도로 생성 완료 tcs가 null 아닐 때
            {
                await InGameContext.Current.Data.TurnManager.RoadCreateCompletionTcs.Task; // 도로 생성 완료 대기
            }
            else // null일 경우
            {
                await Task.CompletedTask; // 바로 넘기기
            }
        }

        // 반환 여부 확인 함수
        private bool IsReturn()
        {
            if (InGameContext.Current.Data.TurnManager.CurrentTeamType != TeamManager.Instance.CurrentTeamType) // 현재 턴의 팀과 내 팀이 다르다면
            {
                return true; // 반환
            }

            return false;
        }
    }
}
// 마지막 작성 일자: 2026.06.29