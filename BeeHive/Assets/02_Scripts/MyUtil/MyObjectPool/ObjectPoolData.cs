using System;
using UnityEngine;

namespace MyUtil.MyObjectPool
{
    // �ۼ���: ������
    // ����ȭ�Ǿ� �ν����Ϳ����� ���̰� ������ ������ ������ Ŭ����
    [Serializable] // Serializable�� ���� ����ȭ - �ν����Ϳ��� ���� + �Ҵ�/���� ����
    public class ObjectPoolData
    {
        public ObjectPoolType poolType; // Ǯ�� Ÿ��

        public int poolCount; // ó�� �����Ͽ� Ǯ�� ��Ƶ� ��ü�� ��

        public GameObject poolObject; // Ǯ���� ��ü
    }
}
// 2025.07.08