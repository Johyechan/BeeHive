using InGame.MyManager;
using InGame.MyObject;
using UnityEngine;

namespace InGame.MySystem.Game
{
    // 작성자: 조혜찬
    // 기물 및 도로 위치 이동 핸들
    public class MoveObjectSetHandle
    {
        public void MoveObject(int id, string parentName, Vector3 targetPos, float angle = 0)
        {
            GameObject obj = ObjectIdManager.Instance.FindObject(id);
            PieceBase pieceBase = obj.GetComponent<PieceBase>(); // 기물 또는 도로 이동을 위해서 객체에서 PieceBase 클래스 가져오기
            GameObject parent = GameObject.Find(parentName); // 부모 객체 찾기
            pieceBase.MoveToPlacePlane(parent.transform, targetPos, angle); // 기물 또는 도로 이동
        }
    }
}
// 마지막 작성 일자: 2025.09.02