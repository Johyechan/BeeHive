using System;
using UnityEngine;

namespace InGame.MyObject
{
    [Serializable]
    public struct AttackInfo
    {
        public string roomID; // 현재 방 ID
        public int returnPieceID; // 공격 당한 기물 ID
        public Vector3 returnPos; // 공격 당한 기물이 가야할 위치
        public string returnParentName; // 공격 당한 기물의 부모 객체 명
        public int attackPieceID; // 공격한 기물 ID
        public Vector3 attackPos; // 공격한 기물이 가야할 위치
    }
}
// 마지막 작성 일자: 2025.09.12