using InGame.MyObject;
using InGame.MyObject.Piece;
using InGame.MyUI;
using System;
using TMPro;
using UnityEngine;

namespace Tutorial.Struct
{
    // 작성자: 조혜찬
    // 튜토리얼 매니저가 Inspector 창에서 할당 받아야 하는 변수들을 가지는 구조체
    [Serializable] // Inspector 창에서 값을 받을 수 있게 직렬화
    public struct TutorialManagerData
    {
        public CanvasGroup tutorialDimmer; // 튜토리얼 UI (클릭 가능한 대상을 알려주는 UI)
        public CanvasGroup tutorialBlockPanel; // 튜토리얼 UI (클릭을 완전히 방지하는 UI)

        public TMP_Text guideTxt; // 안내문
        public TMP_Text helpTxt; // 도움말

        public Material dimmerMat; // 튜토리얼 UI (클릭 가능한 대상을 알려주는 UI) 머티리얼

        public float animationDuration; // 애니메이션 지속시간
        public float inputDelay; // 인풋 딜레이

        public PieceBase firstTurnAIuseSoldier; // 첫 번째 턴(AI 턴)에 사용할 보병
        public PiecePlacePlaneObject firstTurnAISoldierCreatePlace; // 첫 번째 턴(AI 턴)에 보병을 생성 시킬 위치
        public PiecePlacePlaneObject firstTurnAISoldierMovePlace; // 첫 번째 턴(AI 턴)에 보병을 이동 시킬 위치

        public PieceBase secondTurnAIuseTank; // 첫 번째 턴(AI 턴)에 사용할 보병
        public PiecePlacePlaneObject secondTurnAITankCreatePlace; // 첫 번째 턴(AI 턴)에 보병을 생성 시킬 위치
        public PiecePlacePlaneObject secondTurnAITankMovePlace; // 첫 번째 턴(AI 턴)에 보병을 이동 시킬 위치

        public PieceBase thirdTurnAIuseSoldier; // 첫 번째 턴(AI 턴)에 사용할 보병
        public PiecePlacePlaneObject thirdTurnAISoldierCreatePlace; // 첫 번째 턴(AI 턴)에 보병을 생성 시킬 위치
        public PiecePlacePlaneObject thirdTurnAISoldierMovePlace; // 첫 번째 턴(AI 턴)에 보병을 이동 시킬 위치

        public ConfirmUI confirmUI; // 첫 번째 턴(AI 턴)에 보병을 공격 시킬 위치
    }
}
// 마지막 작성 일자: 2026.03.17