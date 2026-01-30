using InGame.MyEnum;
using InGame.MyEvent;
using InGame.MyManager;
using InGame.MyManager.MyPiece;
using InGame.MyManager.MyPlacePlane;
using InGame.MyManager.Turn;
using InGame.MyObject;
using InGame.MyObject.Piece;
using MyUtil;
using MyUtil.MyObjectPool;
using System.Threading.Tasks;
using UnityEngine;

namespace InGame.MySystem.Game.Handler
{
    // 작성자: 조혜찬
    // 도로 관련 소켓 이벤트 연결 핸들러 클래스
    public class RoadSocketEventHandler : BaseSocketEventHandler
    {
        private SetRoadHandle _setRoadHandle; // 도로 세팅 핸들러

        // 생성자(도로 세팅 핸들러)
        public RoadSocketEventHandler(SetRoadHandle setRoadHandle)
        {
            _setRoadHandle = setRoadHandle;
        }

        public override void OnConnect()
        {
            NetworkManager.Instance.Socket.On("roadAdded", (data) =>
            {
                if (NetworkManager.Instance.IsClientOver) // 클라이언트가 종료 되었다면
                    return; // 반환

                string json = data.GetValue().ToString(); // 문자열로 data 받기
                RoadAddedInfo roadAddedInfo = JsonUtility.FromJson<RoadAddedInfo>(json); // RoadAddedInfo로 변환

                MainThreadDispatcher.Enqueue(() =>
                {
                    Transform parent = GameObject.Find(roadAddedInfo.roadParentName).transform;
                    PieceEvents.OnGetRoad?.Invoke(roadAddedInfo.roadCount, (TeamType)roadAddedInfo.teamType, parent); // 이벤트 호출
                });
            });

            NetworkManager.Instance.Socket.On("roadDestroyed", data =>
            {
                if (NetworkManager.Instance.IsClientOver) // 클라이언트가 종료 되었다면
                    return; // 반환

                string json = data.GetValue().ToString(); // 문자열로 data 받기
                RoadDestroyedInfo roadDestroyedInfo = JsonUtility.FromJson<RoadDestroyedInfo>(json); // RoadAddedInfo로 변환
                MainThreadDispatcher.Enqueue(() =>
                {
                    Transform parent = GameObject.Find(roadDestroyedInfo.roadParentName).transform; // 파괴될 도로의 부모 객체
                    TeamType type = (TeamType)roadDestroyedInfo.teamType; // 파괴될 도로의 팀 타입
                    PieceEvents.OnDestroyLeftRoad?.Invoke(parent, type); // 이벤트 호출
                });
            });

            NetworkManager.Instance.Socket.On("setRoad", (data) =>
            {
                if (NetworkManager.Instance.IsClientOver) // 클라이언트가 종료 되었다면
                    return; // 반환

                string json = data.GetValue().ToString(); // 문자열로 data 받기
                SetRoadInfo setRoadInfo = JsonUtility.FromJson<SetRoadInfo>(json); // 도로 세팅에 필요한 값을 가지는 구조체로 변경
                MainThreadDispatcher.Enqueue(() =>
                {
                    _ = _setRoadHandle.SetRoad(setRoadInfo.roadID, setRoadInfo.placePlaneId, setRoadInfo.placedType, setRoadInfo.roadTeamType, setRoadInfo.roadParentName, setRoadInfo.targetParentName, setRoadInfo.targetPos, setRoadInfo.angle); // 도로 세팅
                });
            });

            NetworkManager.Instance.Socket.On("pieceChangedRoad", (data) =>
            {
                if (NetworkManager.Instance.IsClientOver) // 클라이언트가 종료 되었다면
                    return; // 반환

                string json = data.GetValue().ToString(); // 문자열로 data 받기
                PieceChangedRoadInfo pieceChangedRoadInfo = JsonUtility.FromJson<PieceChangedRoadInfo>(json); // 도로 변경에 필요한 값을 가지는 구조체로 변경

                MainThreadDispatcher.Enqueue(() =>
                {
                    GameObject piecePlacePlaneObj = ObjectIdManager.Instance.FindObject(pieceChangedRoadInfo.placePlaneID); // 배치 칸 객체 구하기
                    GameObject pieceObj = ObjectIdManager.Instance.FindObject(pieceChangedRoadInfo.pieceID); // 주위 도로를 변경하려는 기물 구하기

                    PieceBase pieceBase = pieceObj.GetComponent<PieceBase>(); // PieceBase 클래스 가져오기
                    PiecePlacePlaneObject piecePlacePlane = piecePlacePlaneObj.GetComponent<PiecePlacePlaneObject>(); // 기물 배치 칸 클래스 가져오기

                    PieceEvents.OnChangeNearRoad?.Invoke(pieceBase, (TeamType)pieceChangedRoadInfo.teamType, piecePlacePlane); // 주위 도로 변경 이벤트 호출

                    PieceManager.Instance.FindCanPlacePlane();
                });
            });

            NetworkManager.Instance.Socket.On("changedRoad", (data) =>
            {
                if (NetworkManager.Instance.IsClientOver) // 클라이언트가 종료 되었다면
                    return; // 반환

                int roadID = data.GetValue<int>();

                MainThreadDispatcher.Enqueue(() =>
                {
                    GameObject targetRoadObj = ObjectIdManager.Instance.FindObject(roadID); // 기존 도로
                    if(targetRoadObj == null)
                    {
                        NetworkManager.Instance.Socket.Emit("debug", "역시나 ObjectIdManager 쪽에서 오류가 생겼다");
                        return;
                    }

                    GameObject newRoadObj = null; // 변경된 도로

                    switch (TurnManager.Instance.CurrentTeamType) // 현재 턴의 팀
                    {
                        case TeamType.Team1:
                            newRoadObj = ObjectPoolManager.Instance.GetObject(ObjectPoolType.Team1Road, targetRoadObj.transform.parent);
                            break;
                        case TeamType.Team2:
                            newRoadObj = ObjectPoolManager.Instance.GetObject(ObjectPoolType.Team2Road, targetRoadObj.transform.parent);
                            break;
                        case TeamType.Team3:
                            newRoadObj = ObjectPoolManager.Instance.GetObject(ObjectPoolType.Team3Road, targetRoadObj.transform.parent);
                            break;
                    }

                    newRoadObj.transform.localPosition = targetRoadObj.transform.localPosition;
                    newRoadObj.transform.localRotation = targetRoadObj.transform.localRotation;

                    PieceBase targetPieceBase = targetRoadObj.GetComponent<PieceBase>();
                    PieceBase newPieceBase = newRoadObj.GetComponent<PieceBase>();

                    PlacePlaneManager.Instance.ChangePlacePlaneState(targetPieceBase.PieceVariable.currentRoadPlacePlane, newPieceBase, false);

                    PlacePlaneManager.Instance.FindCanPlacePlane();

                    switch (targetPieceBase.CurrentTeamType)
                    {
                        case TeamType.Team1:
                            ObjectPoolManager.Instance.ReturnObject(ObjectPoolType.Team1Road, targetRoadObj);
                            break;
                        case TeamType.Team2:
                            ObjectPoolManager.Instance.ReturnObject(ObjectPoolType.Team2Road, targetRoadObj);
                            break;
                        case TeamType.Team3:
                            ObjectPoolManager.Instance.ReturnObject(ObjectPoolType.Team3Road, targetRoadObj);
                            break;
                    }
                });
            });
        }

        public override void OnDisconnect()
        {
            NetworkManager.Instance.Socket.Off("roadAdded");
            NetworkManager.Instance.Socket.Off("roadDestroyed");
            NetworkManager.Instance.Socket.Off("setRoad");
            NetworkManager.Instance.Socket.Off("pieceChangedRoad");
            NetworkManager.Instance.Socket.Off("changedRoad");
        }
    }
}
// 마지막 작성 일자: 2026.01.30