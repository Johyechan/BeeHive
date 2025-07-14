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
        public void MoveToPlacePlane(Vector3 targetPos)
        {
            Sequence sequence = DOTween.Sequence() // 시퀀스를 통해서 차례대로 순차적으로 실행
                .Append(transform.DOMoveY(targetPos.y * 1.5f, _animationDuration)) // 높이 먼저 올리기
                .Append(transform.DOMove(new Vector3(targetPos.x, transform.position.y, targetPos.z), _animationDuration)) // 지정한 위치로 이동
                .Append(transform.DOMoveY(targetPos.y, _animationDuration)); // 이후 높이 맞추기
        }

        // 오브젝트가 마우스로 클릭되었을 때 실행될 함수
        public abstract void ObjectClicked();
    }
}
// 마지막 작성 일자: 2025.07.14