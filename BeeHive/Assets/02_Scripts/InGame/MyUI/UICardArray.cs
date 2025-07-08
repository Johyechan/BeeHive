using System.Collections.Generic;
using DG.Tweening;
using InGame.MyManager.MyCard;
using MyUtil.MyEvent;
using Unity.VisualScripting;
using UnityEngine;

namespace InGame.MyUI
{
    // �ۼ���: ������
    // �����ϰ� �ִ� UI ī���� ��ġ �� ���� ���� Ŭ����
    public class UICardArray : MonoBehaviour
    {
        [SerializeField] private int _maxCount; // �ִ� ���� ���� ī�� ��

        [SerializeField] private float _maxAngle; // ��ü ��ä�� ����
        [SerializeField] private float _xPosPerCard; // ī�尣�� x�� ����
        [SerializeField] private float _yPosPerCard; // ī�尣�� y�� ����
        [SerializeField] private float _cardBaseYPos; // �⺻ ī���� Y�� ��ġ

        private RectTransform _rectTransform; // �� ��ũ��Ʈ�� ������ ��ü�� RectTransform - ���� ���� UI ī��� ��, �ڽ��� ���� �˱� ���� �ʿ��� ����

        private float _anglePerCard; // ī�尣�� ���� ����
        
        
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>(); // ī����� �θ� RectTransform�� �����´�
        }

        private void OnEnable()
        {
            DrawEventSystem.OnDraw += ChangeUICardsRotateAndPosition; // ��ο� �̺�Ʈ ����
        }

        // ���� �ڽ�(ī��)���� ȸ�� ���� �����ϴ� �Լ�
        private void ChangeUICardsRotateAndPosition()
        {
            int cardCount = _rectTransform.childCount; // ī���� �� ����

            DrawManager.Instance.CanDraw = () => cardCount == _maxCount ? false : true; // ���� ���� ī�� ���� �ִ��� ��ο� �Ұ� ���� �ƴ϶�� ���� ����

            if (cardCount <= 0 || cardCount > _maxCount) // ���� ���� ī�尡 0����� �Ǵ� �ִ� ���� ���� �� �ʰ����
                return; // �׳� ��ȯ

            if (cardCount == 1) // ���� ���� ī�尡 1�� ���
            {
                RectTransform uiCardRectTransform = _rectTransform.GetChild(cardCount - 1).GetComponent<RectTransform>(); // ���� ī���� RectTransform �Ҵ� - ���� ���� -1�� ���� �ʾƾ� ������ �ε����� Ȱ���� ���̱� ������ -1�� �Ͽ� �迭 ũ�� �ʰ� ������ ����
                uiCardRectTransform.DOAnchorPos(new Vector3(0, _cardBaseYPos, 0), 0.1f, true); // ���� Y�� ��ġ�� �̵� + snapping�� Ȱ��ȭ�Ͽ� ���������� ���������� ����
                return;
            }
                

            _anglePerCard = _maxAngle / (cardCount - 1); // ī�尣�� ���� ����
            float xPosPerCard = _xPosPerCard * (cardCount - 1); // ī�尣�� x�� ����
            float yPosPerCard = _yPosPerCard * (cardCount - 1); // ī�尣�� y�� ����

            for(int i = 0; i < cardCount; i++)
            {
                float indexFromCenter = (float)i - ((float)cardCount - 1) / 2; // �߾� �� 0���� ���� �󸶳� ������ �������� Ȯ��
                float t = (float)i / ((float)cardCount - 1); // 0~1 ��

                float angle = indexFromCenter * _anglePerCard; // ī�� ȸ�� ��(�߾����κ��� ������ �� * ī�� ���� ���� ����)

                float xPos = indexFromCenter * xPosPerCard; // ī�� x�� ��(�߾����κ��� ������ �� * ī�� ���� x�� ����)
                float yPos = Mathf.Sin(Mathf.PI * t) * yPosPerCard; // ī�� y�� ��(���� �Լ�(���� �Լ��� n ����� �����ϰ� Ȱ��) * ī�� ���� y�� ����)

                RectTransform uiCardRectTransform = _rectTransform.GetChild(i).GetComponent<RectTransform>(); // ���� �ڽ�(ī��)�� RectTransform�� ��������

                uiCardRectTransform.DORotate(new Vector3(0, 0, -angle), 0.1f); // angle��ŭ ȸ�� (-�� �� ������ �ݴ�� �Ǿ� ���� ���� ���ؼ�)
                uiCardRectTransform.DOAnchorPos(new Vector3(xPos, _cardBaseYPos + yPos, 0), 0.1f, true); // xPos, yPos��ŭ �̵� + snapping�� Ȱ��ȭ�Ͽ� ���������� ���������� ����
            }
        }
    }
}
// ������ �ۼ� ����: 2025.07.08