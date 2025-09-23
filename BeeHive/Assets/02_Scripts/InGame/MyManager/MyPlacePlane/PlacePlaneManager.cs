using MyUtil;
using DG.Tweening;
using UnityEngine;
using InGame.MyEvent;
using InGame.MyEnum;
using InGame.MyObject;
using System.Collections.Generic;
using InGame.MySystem.Game;
using InGame.MyObject.Piece;
using InGame.MyObject.Piece.ObjectPieces;
using InGame.MyManager.MyPlacePlane.Handler;
using InGame.MyManager.MyPlacePlane.Variable;
using System.Threading.Tasks;

namespace InGame.MyManager.MyPlacePlane
{
    // 작성자: 조혜찬
    // 배치가 가능한 배치 판들을 저장하는 싱글톤 클래스
    public class PlacePlaneManager : MonoSingleton<PlacePlaneManager>
    {
        [SerializeField] private Transform _placePlaneParent; // 배치 판들의 부모

        [SerializeField] private List<RoadPlacePlaneObject> _team1NearRoads = new List<RoadPlacePlaneObject>(); // Team1의 성에 근접한 도로 저장 배열
        [SerializeField] private List<RoadPlacePlaneObject> _team2NearRoads = new List<RoadPlacePlaneObject>(); // Team2의 성에 근접한 도로 저장 배열
        [SerializeField] private List<RoadPlacePlaneObject> _team3NearRoads = new List<RoadPlacePlaneObject>(); // Team3의 성에 근접한 도로 저장 배열

        private PlacePlaneManagerVariable _variable; // 매니저에 필요한 변수를 가지는 클래스
        public PlacePlaneManagerVariable Variable { get => _variable; } // 위 변수 프로퍼티

        // 변수 초기화
        protected override void Awake()
        {
            base.Awake();

            _variable = new PlacePlaneManagerVariable();

            _variable.placePlaneMap = new PlacePlaneMap();
            _variable.highLightHandler = new HighLightHandler();
            _variable.findCanPlacePlaneSystem = new FindCanPlacePlaneSystem();
            _variable.placePlaneStateHandler = new PlacePlaneStateHandler();
            _variable.setNearRoadHandler = new SetNearRoadHandler();

            _variable.placePlaneMap.PlacePlaneSet(_placePlaneParent); // 전체 배치 판 저장

            _variable.setNearRoadHandler.Setting(_team1NearRoads, _team2NearRoads, _team3NearRoads); // 주위 도로 세팅
        }

        private void OnEnable()
        {
            // 기물 배치 하이라이트 이벤트에 기물 하이라이트 on/off 기능 구독
            HighLightEvents.OnPiecePlacementHighLight += _variable.highLightHandler.PieceHighLight;
            // 도로 배치 하이라이트 이벤트에 도로 하이라이트 on/off 기능 구독
            HighLightEvents.OnRoadPlacementHighLight += _variable.highLightHandler.RoadHighLight;
            // 기물 이동 하이라이트 이벤트에 기물 하이라이트 on/off 기능 구독
            HighLightEvents.OnPieceMovementHighLight += _variable.highLightHandler.PieceHighLight;
        }

        private void OnDisable()
        {
            // 이벤트 구독 해제
            HighLightEvents.OnPiecePlacementHighLight -= _variable.highLightHandler.PieceHighLight; 
            HighLightEvents.OnRoadPlacementHighLight -= _variable.highLightHandler.RoadHighLight;
            HighLightEvents.OnPieceMovementHighLight -= _variable.highLightHandler.PieceHighLight;
        }

        public async Task FindCanPlacePlane()
        {
            await _variable.findCanPlacePlaneSystem.ResetPlacePlanes();

            await _variable.findCanPlacePlaneSystem.FindCanPlacePiecePlane(TeamManager.Instance.CurrentTeamType);

            await _variable.findCanPlacePlaneSystem.FindCanPlaceRoadPlane(TeamManager.Instance.CurrentTeamType);
        }

        // 배치 칸 상태 변경 함수(상태 변경될 배치칸, 배치할 기물, 이동 여부)
        public async Task ChangePlacePlaneState(PlacePlaneObjectBase currentPlacePlane, PieceBase placedPiece, bool isMove)
        {
            await _variable.placePlaneStateHandler.ChangePlacePlaneState(currentPlacePlane, placedPiece, isMove);
        }
    }
}
// 마지막 작성 일자: 2025.09.23
