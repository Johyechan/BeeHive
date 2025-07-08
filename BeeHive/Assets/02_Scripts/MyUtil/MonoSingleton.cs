using UnityEngine;

namespace MyUtil
{
    // �ۼ���: ������
    // MonoBehaviour ��� �̱��� Ŭ����
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour // �⺻������ �� Ŭ������ �� Ŭ������ �ڽ��� MonoBehaviour�� ��� �ް� ������ T�� MonoBehaviour�� ��ӹް� �ִ� ���¿������� �� Ŭ������ ��� ����
    {
        // �ܺο��� ���� �Ұ����� �ν��Ͻ� 
        private static T _instance;

        // �ܺο��� ���� ������ �ν��Ͻ� ������Ƽ
        public static T Instance
        {
            get
            {
                if(_instance == null) // ���� _instance�� ���̶��
                {
                    _instance = FindFirstObjectByType<T>(); // �� ������ ���� ���� ã�� T Ÿ������ ����

                    if(_instance == null) // �� ���� T Ÿ���� �������� �ʾ� _instance�� ���� ���
                    {
                        GameObject newObj = new GameObject(typeof(T).Name); // ���ο� ��ü�� T Ÿ���� �̸����� ���Ӱ� ����
                        _instance = newObj.AddComponent<T>(); // ���ο� ��ü�� AddComponent�� T�� _instance�� �Ҵ�
                        DontDestroyOnLoad(newObj); // �� ���濡�� �������� �ʰ� ���Ӱ� ������ ��ü�� ����
                    }
                }

                return _instance; // �ν��Ͻ� ��ȯ
            }
        }

        protected virtual void Awake()
        {
            var instance = Instance; // �ν��Ͻ� ������Ƽ�� �޾ƿͼ� _instance�� ���� ���� ���Ӱ� ����, ������ ��� �׳� _instance�� �Ҵ�

            if(instance != this) // ���� Ÿ���� instance�� �̹� �����ϴ� ���¶��
            {
                Destroy(gameObject); // ���� �� ������Ʈ�� ����
            }
        }
    }
}
// ������ �ۼ� ����: 2025.07.08