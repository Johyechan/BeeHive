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

        private bool _isHighLightOn; // ��ġ ĭ�� ���̶���Ʈ�� �����ִ��� Ȯ���ϴ� ����

        private void Awake()
        {
            _isHighLightOn = false; // �����ִ� ���·� �ʱ�ȭ
        }

        // Ŭ�� �� ����� �Լ�
        public void OnUIButtonClick()
        {
            foreach(Transform trans in  _highLightPosList) // ����Ʈ �ȿ� �ִ� �� ��ȸ
            {
                PlacePlaneObjectBase highLightObjectBase = trans.GetComponent<PlacePlaneObjectBase>(); // ���̶���Ʈ ��ũ��Ʈ ��������

                if (highLightObjectBase.PlacedObjectType != ObjectType.None) // ��ġ�� ���¶��
                {
                    highLightObjectBase.HighLightOff();
                    continue; // �Ʒ� �ڵ� ����
                }

                if (!_isHighLightOn) // ��ġ ĭ�� ���̶���Ʈ�� �����ִٸ�
                {
                    highLightObjectBase.HighLightOn(); // ��ġ�� �� �ִ� ��ġ�� �����ִ� ���̶���Ʈ ������Ʈ Ȱ��ȭ
                }
                else // �ƴ϶��
                {
                    highLightObjectBase.HighLightOff(); // ��ġ�� �� �ִ� ��ġ�� �����ִ� ���̶���Ʈ ������Ʈ Ȱ��ȭ
                }
            }

            _isHighLightOn = _isHighLightOn == false ? true : false; // ���̶���Ʈ�� �����ִٸ� true�� �����ִٸ� false�� ����
        }
    }
}
// ������ �ۼ� ����: 2025.07.10