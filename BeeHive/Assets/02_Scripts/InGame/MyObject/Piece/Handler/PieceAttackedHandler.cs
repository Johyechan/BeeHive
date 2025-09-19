using InGame.MyEnum;
using InGame.MyManager;
using InGame.MyManager.MyPiece;
using InGame.MyObject.Piece.Data;
using System.Threading.Tasks;
using UnityEngine;

namespace InGame.MyObject.Piece.Handler
{
    // 작성자: 조혜찬
    // 공격 받는 기능 핸들러 클래스
    public class PieceAttackedHandler
    {
        private PieceBase _pieceBase; // 기물 클래스

        private PieceData _pieceData; // 불변 변수를 가지는 구조체

        public PieceAttackedHandler(PieceBase pieceBase, PieceData pieceData)
        {
            _pieceBase = pieceBase;
            _pieceData = pieceData;
        }

        public async Task PieceAttacked()
        {
            PieceBase attackPieceBase = GameManager.Instance.CurrentMovePiece.GetComponent<PieceBase>(); // 공격한 객체의 PieceBase 가져오기

            int attackObjID = attackPieceBase.PieceVariable.id; // 공격한 객체의 ID
            int returnObjID = _pieceBase.PieceVariable.id; // 공격 받은 객체의 ID

            Transform returnParent = null; // 공격 받은 기물의 부모 객체
            Transform returnPieceTrans = ObjectIdManager.Instance.FindObject(returnObjID).transform; // 공격 받은 기물의 트랜스폼

            switch (_pieceData.currentObjectType) // 배치 가능한 타입(즉 객체 타입)
            {
                case ObjectType.Miner:
                    returnParent = TeamManager.Instance.GetMinerTransform(_pieceData.teamType); // 기물의 팀 타입의 부모 할당
                    break;
                case ObjectType.Soldier:
                    returnParent = TeamManager.Instance.GetSoldierTransform(_pieceData.teamType); // 기물의 팀 타입의 부모 할당
                    break;
                case ObjectType.Tank:
                    returnParent = TeamManager.Instance.GetTankTransform(_pieceData.teamType); // 기물의 팀 타입의 부모 할당
                    break;
            }

            Vector3 returnPos = new Vector3(_pieceData.xInterval * returnParent.childCount, 0, 0); // 공격 당한 기물의 목적지
            Vector3 attackPos = returnPieceTrans.localPosition; // 공격한 기물의 목적지

            AttackInfo attackInfo = new AttackInfo()
            {
                roomID = SceneMgr.Instance.CurrentRoomID, // 현재 방 ID
                returnPieceID = returnObjID, // 공격 당한 기물 ID
                returnPos = returnPos, // 공격 당한 기물의 목적지
                returnParentName = returnParent.name, // 공격 당한 기물의 부모 객체 명
                attackPieceID = attackObjID, // 공격한 기물 ID
                attackPos = attackPos // 공격한 기물의 목적지
            };

            string json = JsonUtility.ToJson(attackInfo);

            NetworkManager.Instance.Socket.Emit("attackPiece", json);

            NetworkManager.Instance.Socket.Emit("debug", $"공격 당한 기물 ID: {returnObjID}, 공격 당한 기물의 목적지: {returnPos}, 공격 당한 기물의 부모 객체 명: {returnParent.name}, 공격한 기물 ID: {attackObjID}, 공격한 기물의 목적지: {attackPos}");
            await PieceManager.Instance.AttackRelatedPiecesMove(_pieceBase, attackPieceBase, returnParent, returnPieceTrans.parent, returnPos, attackPos); // 공격 당한 기물과 공격한 기물이 이동하는 함수
        }
    }
}
// 마지막 작성 일자: 2025.09.16