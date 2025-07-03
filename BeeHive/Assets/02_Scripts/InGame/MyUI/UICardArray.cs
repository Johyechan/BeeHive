using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

namespace InGame.MyUI
{
    // �ۼ���: ������
    // �����ϰ� �ִ� UI ī���� ��ġ �� ���� ���� Ŭ����
    public class UICardArray : MonoBehaviour
    {
        [SerializeField] private float _maxAngle; // ��ü ��ä�� ����
        [SerializeField] private float _xPosPerCard; // ī�尣�� x�� ����
        [SerializeField] private float _yPosPerCard; // ī�尣�� y�� ����

        private RectTransform _rectTransform; // �� ��ũ��Ʈ�� ������ ��ü�� RectTransform - ���� ���� UI ī��� ��, �ڽ��� ���� �˱� ���� �ʿ��� ����

        private float _anglePerCard; // ī�尣�� ���� ����
        
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>(); // ī����� �θ� RectTransform�� �����´�
        }

        void Update()
        {
            ChangeUICardsRotateAndPosition();
        }

        // ���� �ڽ�(ī��)���� ȸ�� ���� �����ϴ� �Լ�
        private void ChangeUICardsRotateAndPosition()
        {
            int cardCount = _rectTransform.childCount; // ī���� �� ����

            if (cardCount <= 1)
                return;

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
                uiCardRectTransform.DOAnchorPos(new Vector3(xPos, yPos, 0), 0.1f); // xPos, yPos��ŭ �̵�
            }
        }
    }
}
// ������ �ۼ� ����: 2025.07.03