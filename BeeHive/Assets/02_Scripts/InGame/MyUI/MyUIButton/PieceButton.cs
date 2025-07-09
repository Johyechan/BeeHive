using InGame.MyObject;
using InGame.MyObject.MyObjectEnum;
using InGame.MyUI.MyUIInterface;
using System.Collections.Generic;
using UnityEngine;

namespace InGame.MyUI.MyUIButton
{
    // �ۼ���: ������
    // �⹰ UI ��ư Ŭ����
    public class PieceButton : MonoBehaviour, IUIButton
    {
        [SerializeField] private List<Transform> _highLightPosList; // ��ġ�� �� �ִ� ��ġ���� ������ ����Ʈ

        // Ŭ�� �� ����� �Լ�
        public void OnUIButtonClick()
        {
            foreach(Transform trans in  _highLightPosList) // ����Ʈ �ȿ� �ִ� �� ��ȸ
            {
                PlacePlaneObjectBase highLightObjectBase = trans.GetComponent<PlacePlaneObjectBase>(); // ���̶���Ʈ ��ũ��Ʈ ��������

                if (highLightObjectBase.PlacedObjectType != ObjectType.None) // ��ġ�� ���¶��
                    continue; // �Ʒ� �ڵ� ����

                highLightObjectBase.HighLightOn(); // ��ġ�� �� �ִ� ��ġ�� �����ִ� ���̶���Ʈ ������Ʈ Ȱ��ȭ
            }
        }
    }
}
