using InGame.MyEnum;
using InGame.MyEvent;
using InGame.MyManager;
using InGame.MyManager.MyPiece;
using InGame.MyManager.MyPlacePlane;
using InGame.MyObject.Piece.Data;
using MyUtil.MyObjectPool;
using System.Threading.Tasks;
using UnityEngine;

namespace InGame.MyObject.Piece.ObjectPieces
{
    // 작성자: 조혜찬
    // 도로 기물 클래스
    public class Road : PieceBase
    {
        public override async void ObjectClicked()
        {
            if (!await WarningEvent.OnCheckCurrentTurn.Invoke(TurnType.MainTurn, "메인 턴이 아니라서 기물을 이동할 수 없습니다."))
                return; // 반환

            GameObject roadObj = null;
            switch (TurnManager.Instance.CurrentTeamType) // 현재 턴의 팀 타입에 따라
            {
                case TeamType.Team1:
                    roadObj = await ObjectPoolManager.Instance.GetObject(ObjectPoolType.Team1Road, transform.parent);
                    break;
                case TeamType.Team2:
                    roadObj = await ObjectPoolManager.Instance.GetObject(ObjectPoolType.Team2Road, transform.parent);
                    break;
                case TeamType.Team3:
                    roadObj = await ObjectPoolManager.Instance.GetObject(ObjectPoolType.Team3Road, transform.parent);
                    break;
            }

            roadObj.transform.localPosition = transform.localPosition;
            roadObj.transform.localRotation = transform.localRotation;

            PieceBase pieceBase = roadObj.GetComponent<PieceBase>();

            ChangeRoadInfo changeRoadInfo = new ChangeRoadInfo()
            {
                roomID = SceneMgr.Instance.CurrentRoomID, // 현재 방 ID
                roadID = PieceVariable.id // 바뀔 도로 기물 ID
            };

            string json = JsonUtility.ToJson(changeRoadInfo);
            NetworkManager.Instance.Socket.Emit("changeRoad", json);

            foreach (var canChangeRoad in PieceManager.Instance.CanChangeRoadList) // 변환 가능한 도로 리스트를 순회
            {
                Road road = canChangeRoad as Road; // Road 클래스로 변환
                if (road != null) // 성공적으로 변환이 되었다면
                {
                    _ = road.ChangeMaterial(true); // 도로를 기본 상태로 전환
                }
            }

            await PlacePlaneManager.Instance.ChangePlacePlaneState(PieceVariable.currentRoadPlacePlane, pieceBase, false);

            await PlacePlaneManager.Instance.FindCanPlacePlane();

            await ChangeMaterial(true); // 기본 머티리얼 상태로 전환

            switch (CurrentTeamType)
            {
                case TeamType.Team1:
                    ObjectPoolManager.Instance.ReturnObject(ObjectPoolType.Team1Road, gameObject);
                    break;
                case TeamType.Team2:
                    ObjectPoolManager.Instance.ReturnObject(ObjectPoolType.Team2Road, gameObject);
                    break;
                case TeamType.Team3:
                    ObjectPoolManager.Instance.ReturnObject(ObjectPoolType.Team3Road, gameObject);
                    break;
            }
        }
    }
}
// 마지막 작성 일자: 2025.10.01