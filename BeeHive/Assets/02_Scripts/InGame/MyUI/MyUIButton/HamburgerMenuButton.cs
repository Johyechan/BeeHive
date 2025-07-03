using InGame.MyUI.MyUIInterface;
using DG.Tweening;
using UnityEngine;

namespace InGame.MyUI.MyUIButton
{
    // �ۼ���: ������
    // �ܹ��� �޴� ��ư Ŭ����
    public class HamburgerMenuButton : MonoBehaviour, IUIButton
    {
        [SerializeField] private RectTransform _hamburgerMenuViewRectTransform; // �ܹ��� �޴� �並 �Ʒ��� ���� ���� ���� �÷� �ݱ� ���� �ʿ��� ����
        [SerializeField] private RectTransform _hamburgerMenuIconRectTransform; // �ܹ��� �޴� �������� RectTransform - ���⼭�� Icon�� ȸ���� ���� �ʿ��� ����

        [SerializeField] private float _animationDelay; // �ִϸ��̼� ���� �ð� ����
        [SerializeField] private float _hamburgerMenuOpenHeight; // �ܹ��� �޴� ������ �� �ܹ��� �޴� ���� ����
        [SerializeField] private float _hamburgerMenuOpenZRotationValue; // �ܹ��� �޴��� ������ �� �ܹ��� �޴� �������� z�� ȸ�� ��
        

        private float _originSizeWidth; // �ܹ��� �޴� ���� �ʺ��� ũ�� - �ʺ�� �������� ���� �����̱⿡ ����

        private bool _isOpen; // �ܹ��� �޴��� �����ִ� �������� Ȯ���ϱ� ���� ����

        private void Awake()
        {
            _isOpen = false; // �� ���� ���·� �ʱ�ȭ
            _originSizeWidth = _hamburgerMenuViewRectTransform.sizeDelta.x; // �ܹ��� �޴� ���� �ʺ� ����
        }

        // Ŭ�� �� ����� �Լ�
        public void OnUIButtonClick()
        {
            // Ŭ�� �� �ִϸ��̼� ���� - ������ ȸ��, �Ʒ��� �� ��������
            if(!_isOpen) // �ܹ��� �޴��� ������ ���� ���¶��
            {
                ClickAnimation(_hamburgerMenuOpenHeight, _hamburgerMenuOpenZRotationValue, true); // �ܹ��� �޴� ���� ���̴� _hamburgerMenuOpenHeight, �ܹ��� �޴� �������� z�� ȸ�� ���� _hamburgerMenuOpenZRotationValue, �ܹ��� �޴��� ���� ���·� ����
            }
            else // �ܹ��� �޴��� ���� ���¶��
            {
                ClickAnimation(0, 0, false); // �ܹ��� �޴� ���� ���̴� 0, �ܹ��� �޴� �������� z�� ȸ�� ���� 0, �ܹ��� �޴��� ���� ���·� ����
            }
        }

        private void ClickAnimation(float height, float rotationValue, bool isOpen)
        {
            _hamburgerMenuViewRectTransform.DOSizeDelta(new Vector2(_originSizeWidth, height), _animationDelay, true); // _animationDelay ���� �ܹ��� �޴� ���� ���� ����
            _hamburgerMenuIconRectTransform.DORotate(new Vector3(0, 0, rotationValue), _animationDelay); // _animationDelay ���� �ܹ��� �޴� ������ ȸ��
            _isOpen = isOpen; // �ܹ��� �޴� ���� ���� ����
        }
    }
}
// ������ �ۼ� ����: 2025.07.01