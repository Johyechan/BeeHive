using InGame.MyEnum;
using InGame.MyEvent;
using InGame.MyManager;
using InGame.MyManager.Global;
using InGame.MyManager.Local;
using InGame.MyObject.Handler;
using InGame.MyObject.Piece;
using MyUtil.GameMode;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tutorial;
using Tutorial.MyEnum;
using UnityEngine;

namespace InGame.MyObject
{
    // 작성자: 조혜찬
    // 기물 배치 칸의 기능 클래스
    public class PiecePlacePlaneObject : PlacePlaneObjectBase
    {
        [SerializeField] private PiecePlacePlaneObject _frontPiecePlacePlaneObject; // 앞에 있는 기물 배치 칸
        
        public List<RoadPlacePlaneObject> nearRoadPlaceTransformList = new(); // 가깝게 붙어있는 도로 칸을 저장하는 리스트

        private Transform _minerParent; // 광부 기물들의 부모
        private Transform _soldierParent; // 보병 기물들의 부모
        private Transform _tankParent; // 전차 기물들의 부모

        private Dictionary<ObjectType, Transform> _pieceMap = new(); // 타입에 따라 필요한 객체를 가지는 부모를 찾기 위한 맵

        private PiecePlaceReturnCheckHandler _pieceReturnCheckHandler; // 기물 배치 가능 여부 체크 핸들러

        protected override void Awake()
        {
            base.Awake();

            _pieceReturnCheckHandler = new PiecePlaceReturnCheckHandler(this);
        }

        private async void Start()
        {
            await GameReady.Gate.WaitAsync(); // 게임 준비 완료 대기

            if (NetworkManager.Instance.IsClientOver) // 클라이언트가 종료 되었다면
                return; // 반환

            ParentSet();
        }

        // 부모 초기화 함수
        private void ParentSet()
        {
            _minerParent = TeamManager.Instance.GetMinerTransform(TeamManager.Instance.CurrentTeamType); // 광부 기물들의 부모 탐색 후 할당
            _soldierParent = TeamManager.Instance.GetSoldierTransform(TeamManager.Instance.CurrentTeamType); // 보병 기물들의 부모 탐색 후 할당
            _tankParent = TeamManager.Instance.GetTankTransform(TeamManager.Instance.CurrentTeamType); // 전차 기물들의 부모 탐색 후 할당

            _pieceMap.Clear(); // 맵 비우기
            _pieceMap.Add(ObjectType.Miner, _minerParent); // 광부 추가
            _pieceMap.Add(ObjectType.Soldier, _soldierParent); // 보병 추가
            _pieceMap.Add(ObjectType.Tank, _tankParent); // 전차 추가
        }

        public void HighLightOffEvent()
        {
            HighLightEvents.OnPiecePlacementHighLight?.Invoke(false, true); // 기물 칸 하이라이트를 끄는 매개변수로 이벤트 콜(하이라이트 키기 여부, 배치 칸 이동 칸 여부 - true는 배치칸, false는 이동칸)
            HighLightEvents.OnPieceMovementHighLight?.Invoke(false, false); // 기물 칸 하이라이트를 끄는 매개변수로 이벤트 콜(하이라이트 키기 여부, 배치 칸 이동 칸 여부 - true는 배치칸, false는 이동칸)
            PieceEvents.OnHideCanAttackPieces?.Invoke(true); // 공격 가능한 기물들 하이라이트 끄기
        }

        // 마우스로 클릭 시 실행될 함수
        public override void ObjectClicked()
        {
            if(GameModeManager.Instance.CurrentGameMode.IsTutorial()) // 현재 게임 모드가 튜토리얼 일때
            {
                TutorialManager.Instance.SetTutorialPanel(false);
            }

            if (isNearToCastle) // 성과 근접한 배치칸이면서
            {
                if(_frontPiecePlacePlaneObject.PlacedObjectType != ObjectType.None)// 앞에 있는 기물 배치칸에 배치된 기물이 있다면 
                {
                    if (_frontPiecePlacePlaneObject.TeamType != TeamManager.Instance.CurrentTeamType) // 앞에 있는 기물 배치칸에 배치된 기물이 내 팀이 아닐 경우
                    {
                        UIManager.Instance.WarningUIMake("상대가 해당 배치 칸의 앞 칸을 점령 했습니다"); // UI 경고문 생성
                        HighLightOffEvent(); // 하이라이트 끄기
                        return; // 반환
                    }
                }
            }

            if (InGameContext.Current.Data.GameManager.CurrentMovePiece != null) // 현재 이동 가능한 객체 있다면
            {
                if (!WarningEvent.OnCheckCurrentTurnTeam()) // 현재 턴이 자신의 턴이 아닐 경우
                {
                    HighLightOffEvent(); // 하이라이트 끄기
                    return; // 반환
                }

                ObjectMove(); // 기물 이동 함수 실행
            }
            else // 현재 이동 가능한 객체가 없다면
            {
                if (_pieceReturnCheckHandler.IsReturn(_leftPieceCount, _cost)) // 기물 배치가 불가능 할 경우
                    return; // 반환

                ObjectPlace(); // 기물 배치 함수 실행
            }
        }

        // 객체를 이동하는 기능 함수
        private async void ObjectMove()
        {
            if (!WarningEvent.OnCanMovePiece.Invoke(CanPlacePieceType, false)) // 같은 타입의 기물이 이동 했었다면
            {
                HighLightEvents.OnPieceMovementHighLight?.Invoke(false, false); // 이동 가능한 판 하이라이트 끄기
                PieceEvents.OnHideCanAttackPieces?.Invoke(true); // 공격 가능한 기물들 하이라이트 끄기
                return;
            }

            InGameContext.Current.Data.GameManager.PieceCanMoveMap[CanPlacePieceType] = false; // 현재 이동하는 타입의 기물을 이후로는 같은 타입의 기물 이동이 불가한 상태로 할당
            await PlacePiece(InGameContext.Current.Data.GameManager.CurrentMovePiece, true); // 기물 이동
        }

        // 객체를 배치하는 기능 함수
        private async void ObjectPlace()
        {
            InGameContext.Current.Data.GameManager.CanMakePiece = false;
            Transform pieceParent = _pieceMap[CanPlacePieceType]; // 현재 배치 가능한 타입의 객체 부모
            int pieceCount = pieceParent.childCount; // 현재 보유 중인 배치 가능한 타입의 기물 수

            await PlacePiece(pieceParent.GetChild(pieceCount - 1).gameObject, false); // 기물 배치
        }

        private async Task PlacePiece(GameObject pieceObj, bool isMove)
        {
            PieceBase pieceBase = pieceObj.GetComponent<PieceBase>(); // 객체의 PieceBase를 가져오기

            if (pieceBase != null) // null 체크
            {
                UIManager.Instance.CanInteractionUI = false; // UI 상호작용 불가능 상태로 할당

                InGameContext.Current.Data.PlacePlaneManager.ChangePlacePlaneState(this, pieceBase, isMove); // 현재 배치칸 상태 변경

                if(GameModeManager.Instance.CurrentGameMode.UseServer()) // 현재 게임 모드가 서버를 사용하는 게임 모드라면
                {
                    PieceInfo pieceInfo = new PieceInfo()
                    {
                        roomID = SceneMgr.Instance.CurrentRoomID, // 현재 방 ID
                        pieceID = pieceBase.NetworkId, // 기물 객체 ID
                        placePlaneID = NetworkId, // 배치 칸 ID
                        parentName = transform.parent.name, // 부모 객체 명
                        placedObjectType = (int)CanPlacePieceType, // 기물 객체 타입
                        targetPos = transform.localPosition, // 기물 객체 최종 위치
                        isMove = isMove // 생성인지 이동인지 여부
                    };
                    string json = JsonUtility.ToJson(pieceInfo); // Json으로 변환
                    if (GameModeManager.Instance.CurrentGameMode.UseServer())
                        NetworkManager.Instance.Socket.Emit("movePiece", json); // 서버에 movePiece 이벤트 전달
                }

                HighLightOffEvent(); // 하이라이트 끄기

                await pieceBase.MoveToPlacePlane(transform.parent, transform.localPosition, isMove); // 기물을 현재 배치판의 부모 자식으로 변경, 기물을 현재 배치할 배치 판의 위치로 이동, 이동인지 생산인지 여부

                if (NetworkManager.Instance.IsClientOver) // 클라이언트가 종료 되었다면
                    return; // 반환

                if (pieceBase.CurrentObjectType == ObjectType.Soldier)
                {
                    if (pieceBase.CurrentTeamType == TeamManager.Instance.CurrentTeamType) // 이동한 기물이 현재 팀의 기물일 경우에만
                    {
                        PieceChangeRoadInfo pieceChangeRoadInfo = new PieceChangeRoadInfo
                        {
                            roomID = SceneMgr.Instance.CurrentRoomID, // 현재 방 ID
                            teamType = (int)pieceBase.CurrentTeamType, // 이동한 기물 팀 타입
                            placePlaneID = pieceBase.PieceVariable.currentPlacePlane.NetworkId, // 이동한 기물의 목적지 칸의 ID
                            pieceID = pieceBase.NetworkId // 주위 도로를 변경 시킬 기물 ID
                        };

                        string pieceChangeRoadJson = JsonUtility.ToJson(pieceChangeRoadInfo);
                        if (GameModeManager.Instance.CurrentGameMode.UseServer())
                            NetworkManager.Instance.Socket.Emit("pieceChangeRoad", pieceChangeRoadJson);

                        PieceEvents.OnChangeNearRoad?.Invoke(pieceBase, pieceBase.CurrentTeamType, pieceBase.PieceVariable.currentPlacePlane); // 도로 변경 이벤트 호출
                    }
                }

                if (isNearToCastle) // 성 주위 배치칸일 때
                {
                    if (currentPlayerTeamType != TeamManager.Instance.CurrentTeamType) // 현재 배치칸이 우리팀 배치칸이 아닐 때
                    {
                        Castle castle = TeamManager.Instance.GetCastle(currentPlayerTeamType); // 상대 성 가져오기
                        castle.CastleHit(pieceBase.Damage); // 상대 성 공격

                        CastleAttackInfo castleAttackInfo = new CastleAttackInfo()
                        {
                            roomID = SceneMgr.Instance.CurrentRoomID, // 현재 방 ID
                            attackedCaslteType = (int)currentPlayerTeamType, // 공격 받은 성의 타입
                            damage = pieceBase.Damage, // 데미지
                            objectID = pieceBase.NetworkId // 공격한 기물 객체 ID
                        };

                        string castleAttackJson = JsonUtility.ToJson(castleAttackInfo); // Json 형태로 변환
                        if (GameModeManager.Instance.CurrentGameMode.UseServer())
                            NetworkManager.Instance.Socket.Emit("castleAttack", castleAttackJson); // 서버로 성 공격 신호 보내기
                        pieceBase.PieceDestroy(); // 공격한 기물 파괴
                    }
                }
                
                if(GameModeManager.Instance.CurrentGameMode.IsTutorial()) // 튜토리얼 일 때
                {
                    switch(TutorialManager.Instance.CurrentTutorialState) // 현재 튜토리얼 상태가
                    {
                        case TutorialState.Turn1_Player: // 첫 번째 턴(플레이어 턴)일 때
                            switch(pieceBase.CurrentObjectType) // 대상 기물이
                            {
                                case ObjectType.Miner: // 광부일 때
                                    if(isMove) // 이동된 경우
                                    {
                                        TutorialManager.Instance.SetTutorialPanel(true, "다음 턴을 눌러 턴을 종료합시다.", "버튼 클릭", 0.18f, 0.008f, new Vector4(0.92f, 0.095f), new Vector4(0.66f, 0.4f));
                                    }
                                    else // 배치된 경우
                                    {
                                        TutorialManager.Instance.SetTutorialPanel(true, "광부를 클릭해서 이동을 합시다. \n (배치 이후 바로 이동이 가능합니다.)", "대상 클릭", 0.08f, 0.008f, new Vector4(0.475f, 0.383f), new Vector4(0.3f, 0.3f), new Vector2(0, 110f));
                                    }
                                    break;
                            }
                            break;
                        case TutorialState.Turn2_Player: // 두 번째 턴(플레이어 턴) 일 경우
                            switch(pieceBase.CurrentObjectType) // 기물이
                            {
                                case ObjectType.Miner: // 전차일 경우
                                    if(isMove) // 이동된 경우
                                    {
                                        TutorialManager.Instance.SetTutorialPanel(true, "다음 턴을 눌러 턴을 종료합시다.", "버튼 클릭", 0.18f, 0.008f, new Vector4(0.92f, 0.095f), new Vector4(0.66f, 0.4f));
                                    }
                                    else // 배치된 경우
                                    {
                                        TutorialManager.Instance.SetTutorialPanel(true, "광부를 클릭해서 이동을 합시다. \n (배치 이후 바로 이동이 가능합니다.)", "대상 클릭", 0.08f, 0.008f, new Vector4(0.475f, 0.383f), new Vector4(0.3f, 0.3f), new Vector2(0, 110f));
                                    }
                                    break;
                            }
                            break;
                        case TutorialState.Turn4_Player: // 네 번째 턴(플레이어 턴)일 경우
                            switch(pieceBase.CurrentObjectType) // 현재 기물이
                            {
                                case ObjectType.Miner: // 광부일 경우
                                    TutorialManager.Instance.SetTutorialPanel(true, "이제 전차를 사용하여 상대 보병을 파괴해봅시다.", "버튼 클릭", 0.1f, 0.008f, new Vector4(0.651f, 0.123f), new Vector4(0.3f, 0.3f));
                                    break;
                                case ObjectType.Tank: // 전차일 경우
                                    if(isMove) // 이동일 경우
                                    {
                                        TutorialManager.Instance.SetTutorialPanel(true, "전차를 다시 선택하여 원거리 공격을 합시다.\n(전차는 이동 후 원거리 공격이 가능합니다.)", "대상 클릭", 0.08f, 0.008f, new Vector4(0.401f, 0.452f), new Vector4(0.3f, 0.3f), new Vector2(0, 110f));
                                    }
                                    else // 생성일 경우
                                    {
                                        TutorialManager.Instance.SetTutorialPanel(true, "전차를 이동합시다.", "대상 클릭", 0.08f, 0.008f, new Vector4(0.475f, 0.383f), new Vector4(0.3f, 0.3f), new Vector2(0, 110f));
                                    }
                                    break;
                            }
                            break;
                        case TutorialState.Turn6_Player:
                            switch (pieceBase.CurrentObjectType) // 현재 기물이
                            {
                                case ObjectType.Miner: // 광부일 경우
                                    TutorialManager.Instance.SetTutorialPanel(true, "다음 턴을 눌러 턴을 종료합시다.", "버튼 클릭", 0.18f, 0.008f, new Vector4(0.92f, 0.095f), new Vector4(0.66f, 0.4f));
                                    break;
                                case ObjectType.Soldier: // 보병일 경우
                                    if(isMove) // 보병 이동일 경우
                                    {
                                        TutorialManager.Instance.SetTutorialPanel(true, "도로를 연결합시다.", "대상 클릭", 0.1f, 0.008f, new Vector4(0.356f, 0.123f), new Vector4(0.5f, 0.3f));
                                    }
                                    else // 보병 생성일 경우
                                    {
                                        TutorialManager.Instance.SetTutorialPanel(true, "보병을 이동합시다.", "대상 클릭", 0.08f, 0.008f, new Vector4(0.475f, 0.383f), new Vector4(0.3f, 0.3f), new Vector2(0, 110f));
                                    } 
                                    break;
                                case ObjectType.Tank: // 전차일 경우
                                    if (isMove) // 이동일 경우
                                    {
                                        TutorialManager.Instance.SetTutorialPanel(true, "이제 상대 광부를 공격합시다.", "대상 클릭", 0.08f, 0.008f, new Vector4(0.542f, 0.684f), new Vector4(0.3f, 0.3f));
                                    }
                                    break;
                            }
                            break;
                        case TutorialState.Turn7_Player:
                            switch (pieceBase.CurrentObjectType) // 현재 기물이
                            {
                                case ObjectType.Soldier: // 보병일 경우
                                    if (isMove) // 보병 이동일 경우
                                    {
                                        TutorialManager.Instance.SetTutorialPanel(true, "다음 턴을 눌러 턴을 종료합시다.", "버튼 클릭", 0.18f, 0.008f, new Vector4(0.92f, 0.095f), new Vector4(0.66f, 0.4f));
                                    }
                                    break;
                            }
                            break;
                        case TutorialState.Turn8_Player:
                            switch (pieceBase.CurrentObjectType) // 현재 기물이
                            {
                                case ObjectType.Tank: // 전차일 경우
                                    if (isMove) // 전차 이동일 경우
                                    {
                                        TutorialManager.Instance.SetTutorialPanel(true, "보병을 이동합시다.", "대상 클릭", 0.08f, 0.008f, new Vector4(0.543f, 0.684f), new Vector4(0.3f, 0.3f), new Vector2(0, 450f));
                                    }
                                    else // 전차 생성일 경우
                                    {
                                        TutorialManager.Instance.SetTutorialPanel(true, "전차를 이동합시다.", "대상 클릭", 0.08f, 0.008f, new Vector4(0.475f, 0.383f), new Vector4(0.3f, 0.3f), new Vector2(0, 110f));
                                    }
                                    break;
                            }
                            break;
                    }
                }

                InGameContext.Current.Data.PieceManager.FindCanPlacePlane();
            }
        }
    }
}
// 마지막 작성 일자: 2026.03.30