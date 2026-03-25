using InGame.MyEvent;
using InGame.MyManager;
using InGame.MyManager.Global;
using InGame.MyManager.Local;
using InGame.MyObject.Piece;
using MyUtil.GameMode;
using System.Threading.Tasks;
using Tutorial;
using Tutorial.MyEnum;
using UnityEngine;

namespace InGame.MyObject.Handler
{
    // 작성자: 조혜찬
    // 도로 배치 기능 핸들러
    public class RoadPlaceHandler
    {
        public async Task Place(RoadPlacePlaneObject roadPlacePlane, PieceBase roadPiece, Transform roadParent, float roadAngle)
        {
            UIManager.Instance.CanInteractionUI = false; // UI 상호작용 불가능 상태로 할당

            InGameContext.Current.Data.PlacePlaneManager.ChangePlacePlaneState(roadPlacePlane, roadPiece, false); // 현재 배치칸 상태 변경

            if(GameModeManager.Instance.CurrentGameMode.UseServer()) // 서버를 사용하는 경우에만
            {
                RoadInfo roadInfo = new RoadInfo()
                {
                    roomID = SceneMgr.Instance.CurrentRoomID, // 현재 방 ID
                    roadID = roadPiece.NetworkId, // 도로 객체 ID
                    placePlaneId = roadPlacePlane.NetworkId, // 현재 객체 ID
                    placedType = (int)roadPlacePlane.CanPlacePieceType, // 배치 객체 타입
                    roadTeamType = (int)roadPlacePlane.TeamType, // 배치 객체 팀 타입
                    roadParentName = roadParent.name, // 부모 객체 이름
                    targetParentName = roadPlacePlane.transform.parent.name, // 부모 객체 이름
                    targetPos = roadPlacePlane.transform.localPosition, // 최종 위치
                    angle = roadAngle // 최종 각도
                };
                string json = JsonUtility.ToJson(roadInfo); // Json으로 변환
                if (GameModeManager.Instance.CurrentGameMode.UseServer())
                    NetworkManager.Instance.Socket.Emit("makeRoad", json);
            }

            HighLightEvents.OnRoadPlacementHighLight?.Invoke(false); // 도로 칸 하이라이트를 끄는 매개변수로 이벤트 콜

            await roadPiece.MoveToPlacePlane(roadPlacePlane.transform.parent, roadPlacePlane.transform.localPosition, false, roadAngle); // 기물을 현재 배치 판 부모의 자식으로 변경 + 현재 이 배치판 위치 이동 + 각도 회전

            InGameContext.Current.Data.PieceManager.FindCanPlacePlane();

            if(GameModeManager.Instance.CurrentGameMode.IsTutorial()) // 튜토리얼 일 경우
            {
                switch(TutorialManager.Instance.CurrentTutorialState)
                {
                    case TutorialState.Turn1_Player:
                        if (TutorialManager.Instance.TutorialRoadCreateCount <= 0) // 처음 도로를 생성하는 경우
                        {
                            TutorialManager.Instance.SetTutorialPanel(true, "도로를 한 번 더 생성합시다. \n (도로는 가지고 있는 도로 개수만큼 중복 생성 가능합니다.)", "버튼 클릭", 0.1f, 0.008f, new Vector4(0.356f, 0.123f), new Vector4(0.5f, 0.3f));
                            TutorialManager.Instance.TutorialRoadCreateCount++;
                        }
                        else // 두 번째 도로를 생성하는 경우
                        {
                            TutorialManager.Instance.SetTutorialPanel(true, "이번에는 광부를 생성합시다. \n (한 턴에 한 기물만 생성할 수 있습니다.)", "버튼 클릭", 0.1f, 0.008f, new Vector4(0.4476f, 0.123f), new Vector4(0.3f, 0.3f));
                            TutorialManager.Instance.TutorialRoadCreateCount = 0;
                        }
                        break;
                    case TutorialState.Turn2_Player:
                        if (TutorialManager.Instance.TutorialRoadCreateCount <= 0) // 처음 도로를 생성하는 경우
                        {
                            TutorialManager.Instance.SetTutorialPanel(true, "도로를 한 번 더 생성합시다.\n(도로는 가지고 있는 도로 개수만큼 중복 생성 가능합니다.)", "버튼 클릭", 0.1f, 0.008f, new Vector4(0.356f, 0.123f), new Vector4(0.5f, 0.3f));
                            TutorialManager.Instance.TutorialRoadCreateCount++;
                        }
                        else // 두 번째 도로를 생성하는 경우
                        {
                            TutorialManager.Instance.SetTutorialPanel(true, "이제 광부를 생성합시다.\n(한 턴에 한 기물만 생성할 수 있습니다.)", "버튼 클릭", 0.1f, 0.008f, new Vector4(0.4476f, 0.123f), new Vector4(0.3f, 0.3f));
                            TutorialManager.Instance.TutorialRoadCreateCount = 0;
                        }
                        break;
                    case TutorialState.Turn3_Player:
                        if (TutorialManager.Instance.TutorialRoadCreateCount <= 0) // 처음 도로를 생성하는 경우
                        {
                            TutorialManager.Instance.SetTutorialPanel(true, "도로를 한 번 더 생성합시다.\n(도로는 가지고 있는 도로 개수만큼 중복 생성 가능합니다.)", "버튼 클릭", 0.1f, 0.008f, new Vector4(0.356f, 0.123f), new Vector4(0.5f, 0.3f));
                            TutorialManager.Instance.TutorialRoadCreateCount++;
                        }
                        else // 두 번째 도로를 생성하는 경우
                        {
                            TutorialManager.Instance.SetTutorialPanel(true, "다음 턴을 눌러 턴을 종료합시다.", "버튼 클릭", 0.18f, 0.008f, new Vector4(0.92f, 0.095f), new Vector4(0.66f, 0.4f));
                            TutorialManager.Instance.TutorialRoadCreateCount = 0;
                        }
                        break;
                    case TutorialState.Turn6_Player:
                        if (TutorialManager.Instance.TutorialRoadCreateCount <= 0) // 처음 도로를 생성하는 경우
                        {
                            TutorialManager.Instance.SetTutorialPanel(true, "도로를 한 번 더 생성합시다.\n(도로는 가지고 있는 도로 개수만큼 중복 생성 가능합니다.)", "버튼 클릭", 0.1f, 0.008f, new Vector4(0.356f, 0.123f), new Vector4(0.5f, 0.3f));
                            TutorialManager.Instance.TutorialRoadCreateCount++;
                        }
                        else // 두 번째 도로를 생성하는 경우
                        {
                            TutorialManager.Instance.SetTutorialPanel(true, "전차를 이동합시다.", "대상 클릭", 0.18f, 0.008f, new Vector4(0.401f, 0.452f), new Vector4(0.3f, 0.3f), new Vector2(0, 250f));
                            TutorialManager.Instance.TutorialRoadCreateCount = 0;
                        }
                        break;
                }
                
            }

            if (NetworkManager.Instance.IsClientOver) // 클라이언트가 종료 되었다면
                return; // 반환
        }
    }
}
// 마지막 작성 일자: 2026.03.25