using System.Collections.Generic;
using UnityEngine;

namespace MyUtil.MyObjectPool
{
    // �ۼ���: ������
    // ������Ʈ Ǯ�� �̱��� Ŭ����
    public class ObjectPoolManager : MonoSingleton<ObjectPoolManager>
    {
        [SerializeField] private List<ObjectPoolData> _poolDataList; // �ν����Ϳ��� Ǯ���� �����͸� ��� ����Ʈ ����

        private Dictionary<ObjectPoolType, ObjectPoolData> _poolDataMap = new(); // Ǯ�� �� - Ÿ�Կ� �´� Ǯ�� ����Ʈ�� �Ҵ�
        private Dictionary<ObjectPoolType, Queue<GameObject>> _pool = new(); // ���� Ǯ - ���⿡ Ǯ�� ��ü�� Ǯ�� Ÿ�Կ� �°� �߰�

        protected override void Awake()
        {
            Init(); // Ǯ ����
        }

        // Ǯ ���� �Լ�
        private void Init()
        {
            foreach(var data in _poolDataList) // Ǯ���� �����Ͱ� ��� ����Ʈ ��ȸ
            {
                _poolDataMap.Add(data.poolType, data); // Ǯ�� �ʿ� ����Ʈ�� ����ִ� �������� ������ ������ Ǯ�� Ÿ�԰� �����͸� �߰�
            }

            foreach(var data in _poolDataMap) // Ǯ�� �� ��ȸ
            {
                var poolType = data.Key; // Ǯ�� Ÿ�� ���� ����
                var poolData = data.Value; // Ǯ�� ������ ���� ����

                _pool.Add(poolType, new Queue<GameObject>()); // Ǯ�� Ǯ�� Ÿ�԰� ���ο� ť �߰�

                for(int i = 0; i < poolData.poolCount; i++) // Ǯ�� �����Ϳ��� ������ Ǯ�� ���� ��ü�� ����ŭ �ݺ�
                {
                    GameObject poolObject = CreateObject(poolType); // ���ο� Ǯ�� ��ü�� ����
                    _pool[poolType].Enqueue(poolObject); // ������ Ǯ�� ��ü�� Ǯ�� Ÿ���� ť�� �߰�
                }
            }
        }

        // ���ο� Ǯ�� ��ü ���� �Լ�(�Ű������� Ǯ�� Ÿ���� �޴´�)
        private GameObject CreateObject(ObjectPoolType type)
        {
            GameObject newObject = Instantiate(_poolDataMap[type].poolObject, transform); // ���ο� ��ü�� Ǯ�� �ʿ��� Ǯ�� Ÿ���� ��ü�� ������ �� �θ� Ǯ �Ŵ����� ����
            newObject.transform.position = Vector3.zero; // ���� ������ ��ü ��ġ �ʱ�ȭ
            newObject.transform.rotation = Quaternion.identity; // ���� ������ ��ü ȸ�� �ʱ�ȭ
            newObject.SetActive(false); // ���� ������ ��ü ��Ȱ��ȭ
            return newObject; // ���� ������ ��ü ��ȯ
        }

        // �ܺο��� Ǯ���� ��ü�� ������ �� �θ��� �Լ�(�Ű� ������ Ǯ�� Ÿ��, �θ� = �⺻ �� null�� �޴´�)
        public GameObject GetObject(ObjectPoolType type, Transform parent = null)
        {
            if (_pool[type].Count > 0) // Ǯ�� Ÿ���� Ǯ�� ��ü�� �����Ѵٸ�
            {
                GameObject obj = _pool[type].Dequeue(); // Ǯ�� Ÿ���� Ǯ�� �ִ� ��ü�� �����´�.
                obj.transform.SetParent(parent); // Ǯ���� ���� ��ü�� �θ� �Ҵ�
                obj.SetActive(true); // Ǯ���� ���� ��ü Ȱ��ȭ
                return obj; // Ǯ���� ���� ��ü ��ȯ
            }
            else // ���� Ǯ�� Ÿ���� Ǯ�� ��ü�� �������� �ʴ´ٸ�
            {
                GameObject newObj = CreateObject(type); // ���Ӱ� Ǯ�� Ÿ�Կ� �´� ��ü ����
                newObj.transform.SetParent(parent); // ���Ӱ� ������ ��ü�� �θ� �Ҵ�
                newObj.SetActive(true); // ���Ӱ� ������ ��ü Ȱ��ȭ
                return newObj; // ���Ӱ� ������ ��ü ��ȯ
            }
        }

        // �ܺο��� ����ߴ� ��ü�� �ٽ� Ǯ�� ���� �� �θ��� �Լ�(�Ű� ������ Ǯ�� Ÿ��, ��ȯ�� ��ü�� �޴´�)
        public void ReturnObject(ObjectPoolType type, GameObject returnObj)
        {
            returnObj.transform.SetParent(transform); // ��ȯ�ϴ� ��ü�� �θ� Ǯ �Ŵ����� ����
            returnObj.transform.position = Vector3.zero; // ��ȯ�ϴ� ��ü�� ��ġ �ʱ�ȭ
            returnObj.transform.rotation = Quaternion.identity; // ��ȯ�ϴ� ��ü�� ȸ�� �ʱ�ȭ

            returnObj.SetActive(false); // ��ȯ�ϴ� ��ü ��Ȱ��ȭ

            _pool[type].Enqueue(returnObj); // Ǯ�� Ÿ���� Ǯ�� ��ü �߰�
        }
    }
}
// ������ �ۼ� ����: 2025.07.08