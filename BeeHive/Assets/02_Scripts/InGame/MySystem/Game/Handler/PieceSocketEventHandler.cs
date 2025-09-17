using InGame.MyManager;
using InGame.MyManager.MyPiece;
using InGame.MyObject;
using InGame.MyObject.Piece;
using UnityEngine;

namespace InGame.MySystem.Game.Handler
{
    // 작성자: 조혜찬
    // 기물 관련 소켓 이벤트 연결 핸들러 클래스
    public class PieceSocketEventHandler : BaseSocketEventHandler
    {
        private SetPieceHandle _setPieceHandle; // 기물 세팅 핸들러

        // 생성자(기물 세팅 핸들러)
        public PieceSocketEventHandler(SetPieceHandle setPieceHandle)
        {
            _setPieceHandle = setPieceHandle;
        }

        public override void OnConnect()
        {
            NetworkManager.Instance.Socket.On("setPiece", async (data) =>
            {
                string json = data.GetValue().ToString(); // 문자열로 data 받기
                SetPieceInfo setPieceInfo = JsonUtility.FromJson<SetPieceInfo>(json); // 기물 세팅에 필요한 값을 가지는 구조체로 변경
                await _setPieceHandle.SetPiece(setPieceInfo.pieceID, setPieceInfo.placePlaneID, setPieceInfo.parentName, setPieceInfo.placedObjectType, setPieceInfo.targetPos, setPieceInfo.isMove); // 기물 세팅
            });

            NetworkManager.Instance.Socket.On("attackedPiece", async (data) =>
            {
                NetworkManager.Instance.Socket.Emit("debug", "공격 받음 들어옴(PIeceSocketEventHandler: 32)");
                string json = data.GetValue().ToString(); // 문자열로 data 받기
                SetAttackRelatedPieceInfo setInfo = JsonUtility.FromJson<SetAttackRelatedPieceInfo>(json); // 공격 관련 기물들을 세팅할 때 필요한 값을 가지는 구조체로 변경

                GameObject returnPieceObj = ObjectIdManager.Instance.FindObject(setInfo.returnPieceID); // 공격 받은 기물 탐색
                PieceBase returnPiece = returnPieceObj.GetComponent<PieceBase>(); // 공격 받은 기물의 PieceBase 가져오기

                GameObject attackPieceObj = ObjectIdManager.Instance.FindObject(setInfo.attackPieceID); // 공격한 기물 탐색
                PieceBase attackPiece = attackPieceObj.GetComponent<PieceBase>(); // 공격한 기물의 PieceBase 가져오기

                Transform returnParent = GameObject.Find(setInfo.returnParentName).transform; // 공격 받은 기물의 부모 객체
                Transform attackParent = attackPieceObj.transform.parent; // 공격한 기물의 부모 객체

                await PieceManager.Instance.MoveAttackRelatedPieces(returnPiece, attackPiece, returnParent, attackParent, setInfo.returnPos, setInfo.attackPos); // 공격 받은 기물 및 공격한 기물 이동 함수
            });
        }
    }
}
// 마지막 작성 일자: 2025.09.16