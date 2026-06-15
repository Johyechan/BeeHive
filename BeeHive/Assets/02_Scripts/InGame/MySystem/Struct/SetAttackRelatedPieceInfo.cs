using UnityEngine;

namespace InGame.MySystem
{
    // 작성자: 조혜찬
    // 공격과 관련된 기물들을 세팅할 때 필요한 값을 가지는 구조체
    public struct SetAttackRelatedPieceInfo
    {
        public int returnPieceID; // 공격 당한 기물 ID
        public Vector3 returnPos; // 공격 당한 기물의 이동 위치
        public string returnParentName; // 공격 당한 기물의 부모 객체 명
        public int attackPieceID; // 공격한 기물 ID
        public Vector3 attackPos; // 공격한 기물의 이동 위치
        public int isFirePowerAttack; // 원거리 공격 여부
    }
}
// 마지막 작성 일자: 2026.06.15