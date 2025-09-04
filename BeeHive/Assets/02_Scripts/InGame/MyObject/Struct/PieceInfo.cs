using System;
using UnityEngine;

namespace InGame.MyObject
{
    // 작성자: 조혜찬
    // 기물 이동 또는 생성에 필요한 값을 가지는 구조체
    [Serializable]
    public struct PieceInfo
    {
        public string roomID; // 현재 방 ID
        public int pieceID; // 기물 객체 ID
        public int placePlaneID; // 기물 객체가 배치된 칸 객체 ID
        public string parentName; // 부모 객체 명
        public int placedObjectType; // 기물 객체 타입
        public Vector3 targetPos; // 기물의 최종 위치
        public bool isMove; // 생성인지 이동인지 여부
    }
}
// 마지막 작성 일자: 2025.09.04