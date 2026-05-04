using InGame.MyEnum;
using InGame.MyManager;
using InGame.MyManager.Global;
using InGame.MyManager.Local;
using MyUtil.GameMode;
using MyUtil.MyObjectPool;
using UnityEngine;

namespace InGame.MyObject.Piece.ObjectPieces
{
    // 작성자: 조혜찬
    // 도로 기물 클래스
    public class Road : PieceBase
    {
        public override void ObjectClicked()
        {
            if(!InGameContext.Current.Data.CardManager.CardUsed) // 카드 사용으로 변경하는 것이 아니라면
            {
                return; // 반환
            }
            else // 카드 사용으로 변경하는 것이라면
            {
                if(!InGameContext.Current.Data.PieceManager.CanChangeRoadList.Contains(this)) // 변형 가능한 도로가 아닐 경우
                {
                    return; // 반환
                }

                InGameContext.Current.Data.CardManager.CardUsed = false; // 카드 사용 끝내기
            }

            ObjectPoolType poolType = ObjectPoolType.None;

            switch (InGameContext.Current.Data.TurnManager.CurrentTeamType) // 현재 턴의 팀 타입에 따라
            {
                case TeamType.Team1:
                    poolType = ObjectPoolType.Team1Road;
                    break;
                case TeamType.Team2:
                    poolType = ObjectPoolType.Team2Road;
                    break;
                case TeamType.Team3:
                    poolType = ObjectPoolType.Team3Road;
                    break;
            }

            float angle = transform.rotation.eulerAngles.y;
            ObjectPoolManager.Instance.MakeObject(poolType, transform.localPosition, transform.parent, true, PieceVariable.currentRoadPlacePlane.NetworkId, angle);

            ChangeRoadInfo changeRoadInfo = new ChangeRoadInfo()
            {
                roomID = SceneMgr.Instance.CurrentRoomID, // 현재 방 ID
                roadID = NetworkId // 바뀔 도로 기물 ID
            };

            string json = JsonUtility.ToJson(changeRoadInfo);
            if (GameModeManager.Instance.CurrentGameMode.UseServer())
                NetworkManager.Instance.Socket.Emit("changeRoad", json);

            foreach (var canChangeRoad in InGameContext.Current.Data.PieceManager.CanChangeRoadList) // 변환 가능한 도로 리스트를 순회
            {
                Road road = canChangeRoad as Road; // Road 클래스로 변환
                if (road != null) // 성공적으로 변환이 되었다면
                {
                    road.ChangeMaterial(true); // 도로를 기본 상태로 전환
                }
            }

            ChangeMaterial(true); // 기본 머티리얼 상태로 전환

            switch (CurrentTeamType)
            {
                case TeamType.Team1:
                    ObjectPoolManager.Instance.ReturnObject(ObjectPoolType.Team1Road, gameObject, true);
                    break;
                case TeamType.Team2:
                    ObjectPoolManager.Instance.ReturnObject(ObjectPoolType.Team2Road, gameObject, true);
                    break;
            }
        }
    }
}
// 마지막 작성 일자: 2026.04.20