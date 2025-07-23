using InGame.MyObject.MyObjectEnum;
using InGame.MySystem;
using MyUtil;
using DG.Tweening;
using UnityEngine;
using InGame.MyEvent;

namespace InGame.MyManager.MyPlacePlane
{
    // 작성자: 조혜찬
    // 배치가 가능한 배치 판들을 저장하는 싱글톤 클래스
    public class PlacePlaneManager : MonoSingleton<PlacePlaneManager>
    {
        [SerializeField] private Transform _placePlaneParent; // 배치 판들의 부모

        private PlacePlaneMap _placePlaneMap; // 전체 기물 판을 가지는 클래스 변수
        public PlacePlaneMap PlacePlaneMap => _placePlaneMap; // get만 가지는 _placePlaneMap 프로퍼티

        private HighLightHandler _highLightHandler; // 하이라이트를 키고 끄는 기능을 가지는 클래스 변수
        public HighLightHandler HighLightHandler => _highLightHandler; // get만 가지는 _highLightHandler 프로퍼티

        private FindCanPlacePlaneSystem _findCanPlacePlaneSystem; // 배치 가능한 배치 판들을 찾는 시스템 클래스 변수

        // 변수 초기화
        protected override void Awake()
        {
            base.Awake();

            _placePlaneMap = new PlacePlaneMap();
            _highLightHandler = new HighLightHandler();
            _findCanPlacePlaneSystem = new FindCanPlacePlaneSystem();

            _placePlaneMap.PlacePlaneSet(_placePlaneParent); // 전체 배치 판 저장
        }

        private void OnEnable()
        {
            // 기물 배치 하이라이트 이벤트에 기물 하이라이트 on/off 기능 구독
            HighLightEvents.OnPiecePlacementHighLight += _highLightHandler.PieceHighLight;
            // 도로 배치 하이라이트 이벤트에 도로 하이라이트 on/off 기능 구독
            HighLightEvents.OnRoadPlacementHighLight += _highLightHandler.RoadHighLight;
            // 기물 이동 하이라이트 이벤트에 기물 하이라이트 on/off 기능 구독
            HighLightEvents.OnPieceMovementHighLight += _highLightHandler.PieceHighLight;
        }

        private void OnDisable()
        {
            // 이벤트 구독 해제
            HighLightEvents.OnPiecePlacementHighLight -= _highLightHandler.PieceHighLight; 
            HighLightEvents.OnRoadPlacementHighLight -= _highLightHandler.RoadHighLight;
            HighLightEvents.OnPieceMovementHighLight -= _highLightHandler.PieceHighLight;
        }

        private void Update()
        {
            // 임시
            if(Input.GetKeyDown(KeyCode.F))
            {
                Sequence sequence = DOTween.Sequence()
                    .AppendCallback(() => _findCanPlacePlaneSystem.ResetPlacePlanes())
                    .AppendCallback(() => _findCanPlacePlaneSystem.FindCanPiecePlacePlane(TeamType.Team1))
                    .AppendCallback(() => _findCanPlacePlaneSystem.FindCanRoadPlacePlane(TeamType.Team1));
            }
        }
    }
}
// 마지막 작성 일자: 2025.07.21
