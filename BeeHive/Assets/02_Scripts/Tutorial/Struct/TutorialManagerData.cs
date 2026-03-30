using InGame.MyObject;
using InGame.MyObject.Piece;
using InGame.MyUI;
using NUnit.Framework;
using System;
using System.Collections.Generic;
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
        public RoadPlacePlaneObject firstTurnAIFirstRoadPlacePlane; // 첫 번째 턴(AI 턴)에 첫 번째 도로 배치 칸
        public RoadPlacePlaneObject firstTurnAISecondRoadPlacePlane; // 첫 번째 턴(AI 턴)에 두 번째 도로 배치 칸

        public PieceBase secondTurnAIuseSoldier; // 두 번째 턴(AI 턴)에 사용할 보병
        public PiecePlacePlaneObject secondTurnAISoldierMovePlace; // 두 번째 턴(AI 턴)에 보병을 이동 시킬 위치
        public RoadPlacePlaneObject secondTurnAIFirstRoadPlacePlane; // 두 번째 턴(AI 턴)에 첫 번째 도로 배치 칸
        public RoadPlacePlaneObject secondTurnAISecondRoadPlacePlane; // 두 번째 턴(AI 턴)에 두 번째 도로 배치 칸

        public PieceBase thirdTurnAIuseSoldier; // 세 번째 턴(AI 턴)에 사용할 보병
        public PiecePlacePlaneObject thirdTurnAISoldierMovePlace; // 세 번째 턴(AI 턴)에 보병을 이동 시킬 위치

        public PieceBase fourthTurnAIuseTank; // 네 번째 턴(AI 턴)에 사용할 전차
        public PiecePlacePlaneObject fourthTurnAITankCreatePlace; // 네 번째 턴(AI 턴)에 전차를 생성 시킬 위치
        public PiecePlacePlaneObject fourthTurnAITankMovePlace; // 네 번째 턴(AI 턴)에 전차를 이동 시킬 위치

        public PieceBase fifthTurnAIuseMiner; // 다섯 번째 턴(AI 턴)에 사용할 광부
        public PiecePlacePlaneObject fifthTurnAIMinerCreatePlace; // 다섯 번째 턴(AI 턴)에 광부를 생성 시킬 위치
        public PiecePlacePlaneObject fifthTurnAIMinerMovePlace; // 다섯 번째 턴(AI 턴)에 광부를 이동 시킬 위치
        public RoadPlacePlaneObject fifthTurnAIFirstRoadPlacePlane; // 다섯 번째 턴(AI 턴)에 첫 번째 도로 배치 칸
        public RoadPlacePlaneObject fifthTurnAISecondRoadPlacePlane; // 다섯 번째 턴(AI 턴)에 두 번째 도로 배치 칸

        public PieceBase sixthTurnAIuseSoldier; // 여섯 번째 턴(AI 턴)에 사용할 보병
        public PiecePlacePlaneObject sixthTurnAISoldierCreatePlace; // 여섯 번째 턴(AI 턴)에 보병을 생성 시킬 위치
        public PiecePlacePlaneObject sixthTurnAISoldierMovePlace; // 여섯 번째 턴(AI 턴)에 보병을 이동 시킬 위치

        public Transform roadParent; // ai 도로 부모 객체

        public ConfirmUI confirmUI; // 첫 번째 턴(AI 턴)에 보병을 공격 시킬 위치

        public UsedDeck usedDeck; // 사용한 카드들을 모아두는 덱

        public GameObject cardObj; // AI가 튜토리얼에서 기본적으로 가지는 화력 카드

        public List<PiecePlacePlaneObject> goldCoin1PlacePlanes; // 금화 1개 버는 위치 리스트
        public List<PiecePlacePlaneObject> goldCoin3PlacePlanes; // 금화 3개 버는 위치 리스트
        public List<PiecePlacePlaneObject> goldCoin5PlacePlanes; // 금화 5개 버는 위치 리스트
    }
}
// 마지막 작성 일자: 2026.03.30