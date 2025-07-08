using DG.Tweening;
using UnityEngine;

namespace InGame.MyObject
{
    // �ۼ���: ������
    // ������Ʈ �迭 ���� Ŭ������ �θ� Ŭ����
    public abstract class ObjectArrayBase : MonoBehaviour
    {
        [SerializeField] protected int _maxChild; // �ִ� �ڽ� ��

        [SerializeField] private float _xPosPerChild; // x�� ����
        [SerializeField] private float _animationDelay; // �ִϸ��̼� ���� �ð�

        protected Transform _objectParentTransform; // Ư�� ������Ʈ�� �θ� Transform ���� - �ڽ� ������Ʈ���� �迭 ������ ���� �ʿ��� ����

        protected virtual void Awake()
        {
            _objectParentTransform = transform; // ���� �Ҵ�
        }

        // �ڽ� ��ü�� ���ġ �Լ�
        protected void ObjectRePlace()
        {
            int objectCount = _objectParentTransform.childCount; // ���� �ڽ� �� - �� �����ϰ� �ִ� ��ü ��

            if (objectCount <= 0 || objectCount > _maxChild) // ���� ���� ��ü ���� 0���϶�� �Ǵ� �ִ� ���� ���� �ʰ����
                return; // ��ȯ

            for(int i = 0; i < objectCount; i++)
            {
                float currentYPos = _objectParentTransform.GetChild(i).transform.position.y; // ���� ��ü�� y�� ��ġ�� ���� - x��� z���� ���� �̵� �� y���� �����̱� ����

                Transform trans = _objectParentTransform.GetChild(i); // �ڽ� ��ü�� Transform�� ����

                Sequence sequence = DOTween.Sequence() // �������� ���� �� �Լ��� ������ ����ǰ� ���� �Լ��� ����
                    .Append(trans.DOLocalMove(new Vector3(_xPosPerChild * i, currentYPos, 0), _animationDelay)) // �ڽ� ��ü�� x��� z���� �Űܾ� �� ��ġ�� �̵�
                    .Append(trans.DOLocalMoveY(0, _animationDelay)); // y���� 0���� �̵�
            }
        }
    }
}
// ������ �ۼ� ����: 2025.07.08