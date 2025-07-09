using InGame.MyObject.MyObjectEnum;
using System.Collections.Generic;
using UnityEngine;

namespace InGame.MyObject
{
    // �ۼ���: ������
    // ��ġ ĭ�� ��� Ŭ����
    public class PlacePlaneObjectBase : MonoBehaviour
    {
        [SerializeField] private float _intensity; // ��� ����

        private Renderer _renderer; // ��Ƽ������ ��� ���� ���� ����
        private Material _material; // ���̶���Ʈ ��Ƽ���� ����

        private ObjectType _placedObjectType; // � �⹰�� ��ġ�Ǿ��ִ��� �˱� ���� ����

        public ObjectType PlacedObjectType // �ܺο��� � �⹰�� ��ġ�Ǿ��ִ��� �˰�, � �⹰�� ��ġ�� ������ �����ϱ� ���� ������Ƽ
        {
            get
            {
                return _placedObjectType;
            }
            set
            {
                _placedObjectType = value;
            }
        }

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
            _material = _renderer.material; // ���� ��Ƽ������ �ƴ� �ν��Ͻ�ȭ�� ���� ���� ��Ƽ������ ������
        }

        // ���̶���Ʈ�� Ű�� �Լ�
        public void HighLightOn()
        {
            _material.color = new Color(_material.color.r, _material.color.g, _material.color.b, 1); // ���� ���� 1�� �ø��鼭 ���̵��� ����
            _material.SetColor("_EmissionColor", Color.white.linear * _intensity); // emission���� �����Ͽ� �������� ����(���̶���Ʈ Ű��)
        }

        // ���̶���Ʈ�� ���� �Լ�
        public void HighLightOff()
        {
            _material.color = new Color(_material.color.r, _material.color.g, _material.color.b, 0); // ���� ���� 0���� �ٲ� ������ �ʵ��� ����
            _material.SetColor("_EmissionColor", Color.white.linear * 0); // emission ���� *0�� �������ν� Intensity�� 0���� ����� ������ �ʵ��� ����(���̶���Ʈ ����)
        }
    }
}
// ������ �ۼ� ����: 2025.07.09