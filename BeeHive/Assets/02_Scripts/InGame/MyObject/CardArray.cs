using MyUtil.MyEvent;
using UnityEngine;

namespace InGame.MyObject
{
    // �ۼ���: ������
    // ī�� �迭�� �����ϴ� Ŭ����
    public class CardArray : ObjectArrayBase
    {
        private void OnEnable()
        {
            DrawEventSystem.OnDraw += ObjectRePlace; // ��ο� �̺�Ʈ�� ����
        }
    }
}
// ������ �ۼ� ����: 2025.07.08