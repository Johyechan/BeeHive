using InGame.MyUI.MyUIInterface;
using UnityEngine;

namespace InGame.MyUI.MyUIButton
{
    // �ۼ���: ������
    // UI���� ������ ī����� �����ְ� ����� UI ��ư Ŭ����
    public class CardShowButton : ShowButtonBase
    {
        [SerializeField] private RectTransform _cardsUI; // ī�� UI RectTransform ���� - ��ġ ������ ���� �ʿ�
        [SerializeField] private RectTransform _piecesUI; // �⹰ UI RectTransform ���� - ��ġ ������ ���� �ʿ�

        [SerializeField] private float _showYPos; // �����ֱ� ���� Y�� ��
        [SerializeField] private float _showDownYPos; // �� �����ֱ� ���� Y�� ��

        public override void OnUIButtonClick()
        {
            ShowAnimationY(_cardsUI, _piecesUI, _showYPos, _showDownYPos);
        }
    }
}
// ������ �ۼ� ����: 2025.07.07
