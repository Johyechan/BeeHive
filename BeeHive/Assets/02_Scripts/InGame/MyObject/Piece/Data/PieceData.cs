using InGame.MyEnum;
using InGame.MyObject.Handler;
using InGame.MyObject.Piece.Handler;
using InGame.MyUI;
using MyUtil.MyObjectPool;
using System;
using UnityEngine;

namespace InGame.MyObject.Piece.Data
{
    // 작성자: 조혜찬
    // 기물이 값 형식으로 가져야할 변수들을 가지는 구조체
    [Serializable] // 직렬화를 통한 Inspector창에서도 변수 할당이 가능하도록 설정
    public struct PieceData
    {
        public MaterialData materialData; // 머티리얼 변경과 관련된 변수를 가지는 구조체

        // 내부에서 생성할 것이기 때문에 Inspector창에서는 숨기기
        [HideInInspector] public ChangeMaterialHandler changeMaterialHandler; // 머티리얼 변경 핸들러

        [HideInInspector] public PieceMoveHandler pieceMoveHandler; // 기물 이동 핸들러

        [HideInInspector] public PieceDeselectHandler pieceDeselectHandler; // 기물 선택 해제 핸들러

        [HideInInspector] public PieceSelectHandler pieceSelectHandler; // 기물 선택 핸들러

        [HideInInspector] public PieceAttackedHandler pieceAttackedHandler; // 공격 받는 기능 핸들러

        public TeamType teamType; // 객체의 팀 타입

        public ObjectType currentObjectType; // 현재 객체의 타입

        public float animationDuration; // 애니메이션 지속시간
        public float zInterval; // 기물 배열의 x축 간격
        public float moveAnimationYvalue; // 이동 애니메이션에서 기물이 공중에 뜰 때 필요한 y 값

        [HideInInspector] public ConfirmUI confirmUI; // 확인 UI(전차로 원거리 공격 시 화력 카드 사용 여부를 묻기 위한 클래스)
    }
}
// 마지막 작성 일자: 2026.03.31