using InGame.MyUI.MyUIInterface;
using DG.Tweening;
using UnityEngine;

namespace InGame.MyUI.MyUIButton
{
    // �ۼ���: ������
    // �����ִ� �ִϸ��̼��� �����Ű�� ��ư�� �θ� Ŭ����
    public abstract class ShowButtonBase : MonoBehaviour, IUIButton
    {
        [SerializeField] private float _delayForNext; // ���� �Լ��� �����ϱ���� ��� �ð�
        [SerializeField] private float _animationDelay; // �ִϸ��̼� ���� �ð� ����

        // Ŭ�� �� ����� �Լ�
        public abstract void OnUIButtonClick();

        // �����ִ� �ִϸ��̼� �Լ� - �Ű������� ������ UI�� RectTransform�� ���� UI�� RectTransform�� �޴´�
        protected void ShowAnimationY(RectTransform showUI, RectTransform showDownUI, float showYPos, float showDownYPos)
        {
            Sequence sequence = DOTween.Sequence()
                .Append(showDownUI.DOAnchorPosY(showDownYPos, _animationDelay)) // �� �Ʒ��θ� ������ ���̱� ������ ��Ŀ ������ Y�����θ� �����̴� DOTWEEN �Լ� ��� - �켱 showDownYPos ��ġ�� _animationDelay ���� Y�� �̵�
                .Insert(_delayForNext, showUI.DOAnchorPosY(showYPos, _animationDelay)); // �� �Ʒ��θ� ������ ���̱� ������ ��Ŀ ������ Y�����θ� �����̴� DOTWEEN �Լ� ��� - showYPos ��ġ�� _delayForNext �� �� _animationDelay ���� Y�� �̵�
        }
    }
}

