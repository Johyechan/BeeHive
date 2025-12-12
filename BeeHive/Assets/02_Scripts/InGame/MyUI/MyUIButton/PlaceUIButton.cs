using InGame.MyEnum;
using InGame.MyEvent;
using InGame.MyManager;
using InGame.MyUI.MyUIInterface;
using MyUtil;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace InGame.MyUI.MyUIButton
{
    // 작성자: 조혜찬
    // 배치할 기물을 선택하는 버튼의 부모 클래스
    public abstract class PlaceUIButton : MonoBehaviour, IUIClick
    {
        [SerializeField] protected ObjectType _canPlaceType; // 배치 가능한 객체 타입 변수

        protected Transform _objectParent; // 배치하려고 하는 기물의 부모

        protected bool _isHighLightOn; // 하이라이트가 켜졌는지 확인하는 변수

        [SerializeField] protected int _cost; // 가격

        [SerializeField] private TMP_Text _leftPieceCountText;

        private void Awake()
        {
            _isHighLightOn = false; // 하이라이트 꺼짐 상태로 초기화
        }

        private void Start()
        {
            switch(_canPlaceType) // 배치할 객체의 타입에 따라
            {
                case ObjectType.Miner: // 광부를 배치할 수 있다면
                    _objectParent = TeamManager.Instance.GetMinerTransform(TeamManager.Instance.CurrentTeamType); // 광부 객체들의 부모를 할당
                    break;
                case ObjectType.Soldier: // 보병을 배치할 수 있다면
                    _objectParent = TeamManager.Instance.GetSoldierTransform(TeamManager.Instance.CurrentTeamType); // 보병 객체들의 부모를 할당
                    break;
                case ObjectType.Tank: // 전차를 배치할 수 있다면
                    _objectParent = TeamManager.Instance.GetTankTransform(TeamManager.Instance.CurrentTeamType); // 전차 객체들의 부모를 할당
                    break;
                case ObjectType.Road: // 도로를 배치할 수 있다면
                    _objectParent = TeamManager.Instance.GetRoadTransform(TeamManager.Instance.CurrentTeamType); // 도로 객체들의 부모를 할당
                    break;
            }
        }

        private void OnEnable()
        {
            HighLightEvents.OnPiecePlacementHighLight += HightLightOff; // 기물 전용 이벤트 구독
            HighLightEvents.OnRoadPlacementHighLight += HightLightOff; // 도로 전용 이벤트 구독
            UIEvents.OnSetLeftPieceText += SetText; // 남은 기물 수 세팅하는 이벤트 구독
        }

        private void OnDisable()
        {
            HighLightEvents.OnPiecePlacementHighLight -= HightLightOff; // 기물 전용 이벤트 구독 해제
            HighLightEvents.OnRoadPlacementHighLight -= HightLightOff; // 도로 전용 이벤트 구독 해제
            UIEvents.OnSetLeftPieceText -= SetText; // 남은 기물 수 세팅하는 이벤트 구독
        }

        // 하이라이트가 꺼질 때 현재 하이라이트 활성화 여부를 끄는 함수 - 기물용
        private void HightLightOff(bool isOn, bool isPlace = true)
        {
            if (!isOn) // 꺼져있는 상태라면
            {
                _isHighLightOn = isOn; // 현재 하이라이트 활성화 여부를 꺼져있는 상태로 할당
            }
        }

        // 하이라이트가 꺼질 때 현재 하이라이트 활성화 여부를 끄는 함수 - 도로용
        private void HightLightOff(bool isOn)
        {
            if (!isOn) // 꺼져있는 상태라면
            {
                _isHighLightOn = isOn; // 현재 하이라이트 활성화 여부를 꺼져있는 상태로 할당
            }
        }

        // UI 텍스트 변경 함수
        private void SetText()
        {
            MainThreadDispatcher.Enqueue(() =>
            {
                _leftPieceCountText.text = $"사용 가능 개수: {_objectParent.childCount}";
            });
        }

        public abstract void OnUIClick();
    }
}
// 마지막 작성 일자: 2025.09.15