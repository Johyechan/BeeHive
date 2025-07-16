using InGame.MyObject.MyObjectInterface;
using DG.Tweening;
using UnityEngine;

namespace InGame.MyObject
{
    // 작성자: 조혜찬
    // 기물들의 기본적인 기능을 가지는 부모 클래스
    public abstract class PieceBase : MonoBehaviour, IClickObject
    {
        [SerializeField] private float _animationDuration; // 애니메이션 지속시간

        // 기물들을 지정 위치로 이동 시키는 함수
        public void MoveToPlacePlane(Transform parent, Vector3 targetPos)
        {
            transform.SetParent(parent); // 부모 변경
            float yPos = targetPos.y * 1.5f; // 이후 배치할 때 애니메이션 효과를 위해 1.5배를 하여 조금 더 높이 올려준다
            Sequence sequence = DOTween.Sequence() // 시퀀스를 통해서 차례대로 순차적으로 실행
                .Append(transform.DOLocalMoveY(yPos, _animationDuration)) // 높이 먼저 올리기
                .Append(transform.DOLocalMove(new Vector3(targetPos.x, yPos, targetPos.z), _animationDuration)) // 지정한 위치로 이동
                .Append(transform.DOLocalMoveY(targetPos.y, _animationDuration)); // 이후 높이 맞추기
        }

        // 오브젝트가 마우스로 클릭되었을 때 실행될 함수
        public abstract void ObjectClicked();
    }
}
// 마지막 작성 일자: 2025.07.16