using UnityEngine;

namespace InGame.MyObject.Piece.Variable
{
    // 작성자: 조혜찬
    // 변경이 잦거나 Inspector창에서 할당 받을 필요가 없는 변수들을 가지는 클래스
    public class PieceVariable
    {
        public Transform parent; // 기물을 모아두는 부모

        public PiecePlacePlaneObject currentPlacePlane; // 현재 기물이 존재하고 있는 배치 칸

        public RoadPlacePlaneObject currentRoadPlacePlane; // 현재 도로가 존재하고 있는 배치 칸

        public bool isSelected = false; // 선택 여부를 확인하는 변수

        public bool isFirePowerAttackTarget = false; // 원거리 공격 대상인지 확인하는 변수

        public int id = 0; // 객체 id
    }
}
// 마지막 작성 일자: 2025.10.21