using InGame.MyObject.MyObjectInterface;
using DG.Tweening;
using UnityEngine;
using InGame.MyManager;
using InGame.MyObject.MyObjectEnum;
using InGame.MyEvent;

namespace InGame.MyObject
{
    // 작성자: 조혜찬
    // 기물들의 기본적인 기능을 가지는 부모 클래스
    public abstract class PieceBase : MonoBehaviour, IClickObject
    {
        public TeamType teamType; // 객체의 팀 타입

        [SerializeField] private float _animationDuration; // 애니메이션 지속시간

        protected Transform _parent; // 기물을 모아두는 부모

        protected bool _isSelected; // 선택 여부를 확인하는 변수

        protected virtual void OnEnable()
        {
            HighLightEventSystem.OnPieceHighLightObjAction += HighLightOff;
        }

        protected virtual void OnDisable()
        {
            HighLightEventSystem.OnPieceHighLightObjAction -= HighLightOff;
        }

        // 외부에서 하이라이트를 끌 때 현재 스크립트에서 하이라이트 활성화 여부를 끔 상태로 만들어주는 함수
        private void HighLightOff(bool isOn, bool isMove = true) // 켜졌는지 여부, 이동 상태를 위해 켜졌는지 여부
        {
            if(!isOn) // 끄는 상태일 때
            {
                _isSelected = false;
            }
        }

        // 기물들을 지정 위치로 이동 시키는 함수
        public void MoveToPlacePlane(Transform parent, Vector3 targetPos, float angle = 0)
        {
            transform.SetParent(parent); // 부모 변경
            float yPos = targetPos.y * 1.5f; // 이후 배치할 때 애니메이션 효과를 위해 1.5배를 하여 조금 더 높이 올려준다
            Sequence sequence = DOTween.Sequence() // 시퀀스를 통해서 차례대로 순차적으로 실행
                .Append(transform.DOLocalMoveY(yPos, _animationDuration)) // 높이 먼저 올리기
                .Append(transform.DOLocalMove(new Vector3(targetPos.x, yPos, targetPos.z), _animationDuration)) // 지정한 위치로 이동
                .Append(transform.DOLocalRotate(new Vector3(0, angle, 0), _animationDuration)) // 회전 값만큼 y축 회전
                .Append(transform.DOLocalMoveY(targetPos.y, _animationDuration)) // 이후 높이 맞추기
                .AppendCallback(() => UIManager.Instance.CanInteractionUI = true); // UI 상호작용 가능 상태로 초기화
        }

        // 오브젝트가 마우스로 클릭되었을 때 실행될 함수
        public abstract void ObjectClicked();
    }
}
// 마지막 작성 일자: 2025.07.21