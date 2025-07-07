using InGame.MyUI.MyUIInterface;
using UnityEngine;

namespace InGame.MyUI.MyUIButton
{
    // �ۼ���: ������
    // UI���� �⹰���� �����ְ� ����� UI ��ư Ŭ����
    public class PieceShowButton : ShowButtonBase
    {
        [SerializeField] private RectTransform _cardsUI; // ī�� UI RectTransform ���� - ��Ȱ��ȭ�� ���� �ʿ�
        [SerializeField] private RectTransform _piecesUI; // �⹰ UI RectTransform ���� - Ȱ��ȭ�� ���� �ʿ�

        [SerializeField] private float _showYPos; // �����ֱ� ���� Y�� ��
        [SerializeField] private float _showDownYPos; // �� �����ֱ� ���� Y�� ��

        public override void OnUIButtonClick()
        {
            ShowAnimationY(_piecesUI, _cardsUI, _showYPos, _showDownYPos);
        }
    }
}
// ������ �ۼ� ����: 2025.07.07