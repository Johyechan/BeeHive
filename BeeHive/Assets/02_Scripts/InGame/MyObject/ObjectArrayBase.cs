using DG.Tweening;
using UnityEngine;

namespace InGame.MyObject
{
    // 작성자: 조혜찬
    // 오브젝트 배열 관리 클래스의 부모 클래스
    public abstract class ObjectArrayBase : MonoBehaviour
    {
        [SerializeField] protected int _maxChild; // 최대 자식 수

        [SerializeField] private float _xPosPerChild; // x축 간격
        [SerializeField] private float _animationDelay; // 애니메이션 실행 시간

        protected Transform _objectParentTransform; // 특정 오브젝트의 부모 Transform 변수 - 자식 오브젝트들의 배열 관리를 위해 필요한 변수

        protected virtual void Awake()
        {
            _objectParentTransform = transform; // 변수 할당
        }

        // 자식 객체들 재배치 함수
        protected void ObjectRePlace()
        {
            int objectCount = _objectParentTransform.childCount; // 현재 자식 수 - 즉 보유하고 있는 객체 수

            if (objectCount <= 0 || objectCount > _maxChild) // 보유 중인 객체 수가 0이하라면 또는 최대 보유 개수 초과라면
                return; // 반환

            for(int i = 0; i < objectCount; i++)
            {
                float currentYPos = _objectParentTransform.GetChild(i).transform.position.y; // 현재 객체의 y축 위치를 저장 - x축과 z축을 전부 이동 후 y축을 움직이기 위함

                Transform trans = _objectParentTransform.GetChild(i); // 자식 객체의 Transform을 저장

                Sequence sequence = DOTween.Sequence() // 시퀀스를 통해 한 함수가 실행이 종료되고 다음 함수가 실행
                    .Append(trans.DOLocalMove(new Vector3(_xPosPerChild * i, currentYPos, 0), _animationDelay)) // 자식 객체를 x축과 z축은 옮겨야 할 위치로 이동
                    .Append(trans.DOLocalMoveY(0, _animationDelay)); // y축을 0으로 이동
            }
        }
    }
}
// 마지막 작성 일자: 2025.07.08