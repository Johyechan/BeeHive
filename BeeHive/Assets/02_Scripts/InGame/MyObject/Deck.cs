using DG.Tweening;
using InGame.MyManager.MyCard;
using InGame.MyObject.MyObjectInterface;
using MyUtil.MyEvent;
using MyUtil.MyObjectPool;
using UnityEngine;

namespace InGame.MyObject
{
    // �ۼ���: ������
    // �� Ŭ���� - Ŭ���Ǿ��� �� ī�带 �߰� ��Ű�� ����� ������ Ŭ����
    public class Deck : MonoBehaviour, IClickObject
    {
        [SerializeField] private Transform _playerCardsParent; // �÷��̾� ī����� �θ� Transform ����
        [SerializeField] private Transform _otherCardsParent; // �� ī����� �θ� Transform ����

        [SerializeField] private RectTransform _playerUICardsParent; // �÷��̾� UI ī����� �θ� RectTransform ����

        private Transform _deckTransform; // �� Transform ���� - ���� ���� �ִ� ī���� ���� �˱� ���� ����

        private int _currentDeckCardCount; // ���� ���� �ִ� ī�� ��

        // ���� �ʱ�ȭ
        private void Awake()
        {
            _deckTransform = GetComponent<Transform>();

            _currentDeckCardCount = _deckTransform.childCount; 
        }

        private void Update()
        {
            // �ӽ�
            if(Input.GetKeyDown(KeyCode.D))
            {
                if (!DrawManager.Instance.IsCanDraw) // ���� Draw�� �Ұ����ϴٸ�
                    return; // ��ȯ

                Sequence sequence = DOTween.Sequence()
                    .AppendCallback(() => DrawCard())
                    .AppendCallback(() => DrawEventSystem.OnDraw?.Invoke());
            }
        }

        public void DrawCard(bool isPlayerDraw = true)
        {
            if (isPlayerDraw) // ���� �÷��̾ ��ο��ϴ� ���¶��
            {
                _deckTransform.GetChild(_currentDeckCardCount - 1).SetParent(_playerCardsParent); // ���� �ִ� ī�带 �÷��̾��� ī��� ���� - ���� ���� -1�� ���� �ʾƾ� ������ �ε����� Ȱ���� ���̱� ������ -1�� �Ͽ� �迭 ũ�� �ʰ� ������ ����
                GameObject uiCard = ObjectPoolManager.Instance.GetObject(ObjectPoolType.UIcard, _playerUICardsParent); // UI ī�带 �߰��Ͽ� �÷��̾� UI ī�忡 �߰�
            }
            else // ���� ���� ��ο��ϴ� ���¶��
            {
                _deckTransform.GetChild(_currentDeckCardCount - 1).SetParent(_otherCardsParent); // ���� �ִ� ī�带 ���� ī��� ���� - ���� ���� -1�� ���� �ʾƾ� ������ �ε����� Ȱ���� ���̱� ������ -1�� �Ͽ� �迭 ũ�� �ʰ� ������ ����
            }

            _currentDeckCardCount--; // ���� ���� �ִ� ī�� �� ����
        }
        
        // Ŭ���Ǿ��� �� ����� �Լ�
        public void ObjectClicked()
        {
            DrawCard(); // �÷��̾ ī�带 ȹ���ϴ� ���·� ��ο� �Լ� ����
        }
    }
}
// ������ �ۼ� ����: 2025.07.08