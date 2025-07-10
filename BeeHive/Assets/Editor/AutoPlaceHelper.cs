#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

// �ۼ���: ������
// �ڵ����� ��ü�� ��ġ���ִ� ������ ��ũ��Ʈ
public class AutoPlaceHelper : EditorWindow
{
    public GameObject prefab; // �ڵ����� ���� ��ų ��ü

    public Transform parentTransform; // �ڵ����� ������ ��ü�� �θ�
    public Transform beforeParentTransform; // ������ �ڵ����� ������ ��ü�� �θ� - ��ü�� �����ϱ� ���� �̸��� �߰��� �� ����� ����

    public Vector3 startPos; // �ڵ����� ������ ������ ��ġ

    public int rows = 1; // ����
    public int cols = 1; // ����

    public float xSpacing = 0f; // �ڵ� ���� ��ü x�� ���� ����
    public float ySpacing = 0f; // �ڵ� ���� ��ü y�� ���� ����

    [MenuItem("Tools/Auto Place Helper")] // �޴���
    public static void ShowWindow() // �޴����� ȣ�� ������ ���� �Լ�
    {
        GetWindow<AutoPlaceHelper>("Auto Place Helper"); // Unity �����Ϳ��� AutoPlaceHelper Ÿ���� Ŀ���� �����츦 ���� + Auto Place Helper - Ÿ��Ʋ
    }

    private void OnGUI() // �� �����Ӹ��� UI�� �׷��ִ� �Լ�
    {
        prefab = (GameObject)EditorGUILayout.ObjectField("Auto Place Object", prefab, typeof(GameObject), false); // �ڵ����� ���� ��ų �������� ������ �� �ִ� �ʵ� - Auto Place Object�� �̸����� ������ �ʵ��̸� GameObject���ĸ��� ���� �� ������, �� ������Ʈ�� ���� �Ұ�, prefab�� �Ҵ��ϸ鼭 2��° �Ű������� prefab�� �ٽ� �ִ� ������ �ʵ忡�� ����� ���� prefab�� �ް� prefab�� �ٽ� �Ҵ��������ν� �ڵ峻������ ����� prefab�� ���� ����� �� �ְ� ��
        parentTransform = (Transform)EditorGUILayout.ObjectField("Auto Place Parent", parentTransform, typeof(Transform), true); // Auto Place Parent��� �̸��� ������ �ʵ��̸� Transform���ĸ��� ���� �� ������, �� ������Ʈ ������ �����ϴ�
        beforeParentTransform = (Transform)EditorGUILayout.ObjectField("Before Auto Place Parent", beforeParentTransform, typeof(Transform), true); // Before Auto Place Parent��� �̸��� ������ �ʵ��̸� Transform���ĸ��� ���� �� ������, �� ������Ʈ ������ �����ϴ�
        startPos = EditorGUILayout.Vector3Field("Start Position", startPos); // Start Position�̶�� �̸��� ������ �ʵ�
        rows = EditorGUILayout.IntField("Rows", rows); // Rows��� �̸��� ������ �ʵ�
        cols = EditorGUILayout.IntField("Columns", cols); // Columns��� �̸��� ������ �ʵ�
        xSpacing = EditorGUILayout.FloatField("X Spacing", xSpacing); // X Spacing�̶�� �̸��� ������ �ʵ�
        ySpacing = EditorGUILayout.FloatField("Y Spacing", ySpacing); // Y Spacing�̶�� �̸��� ������ �ʵ�

        if(GUILayout.Button("Auto Place")) // Auto Place��� �̸��� ������ ��ư + �� ��ư�� ������
        {
            AutoPlaceObjects(); // �Լ� ����
        }
    }

    // �ڵ����� ��ü���� ��ġ�ϴ� �Լ�
    private void AutoPlaceObjects()
    {
        if(prefab == null || parentTransform == null) // ���� �ڵ� ������ų ��ü�� ����, ��ü�� �θ� �ʵ忡�� �Ҵ���� �ʾҴٸ�
        {
            Debug.Log("������ �Ǵ� �θ� ��ü�� ����"); // ����� �����
            return; // ��ȯ
        }

        for(int i = parentTransform.childCount - 1; i >= 0; i--) // ���� �θ� ��ü�� �ڽĵ��� ���� ����ȸ - ����ȸ �ϴ� ������ �⺻ ��ȸ�� �Ͽ��� �� ���κ� ���� ����鼭 �ٽ� �ڽĵ��� ���� �ö���鼭 0��° ��ü�� ä������ �ǰ� ��������� �ܿ� �ڽĵ��� ���Ե�
        {
            DestroyImmediate(parentTransform.GetChild(i).gameObject); // �����Ϳ��� ��� ������Ʈ�� ���� �� ����ϴ� �Լ� - Destroy�� ���� ������ ���� ���������� �� �Լ��� ��� ����
        }

        for(int row = 0; row < rows; row++) // ���ο���ŭ �ݺ�
        {
            for(int col = 0; col < cols; col++) // ���ο���ŭ �ݺ�
            {
                Debug.Log(row);
                Vector3 pos = startPos + new Vector3(row * xSpacing, 0, col * ySpacing); // ��ġ�� (ó�� ��ġ * n��°, 0, ó�� ��ġ * n��°)
                GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(prefab, parentTransform); // GameObjec Ÿ������ ������ ���� (�⺻ Instantiate�� �������� ���� ���·� ���������� �� �Լ��� ������ ���·� ���� �׷��� ������ ��������� �� ������Ʈ�� �ݿ� ���� + �� ����, �� ������ ���� �Լ�)

                if(beforeParentTransform != null) // ������ �ڵ����� �����߾��� ��ü�� �ִٸ�
                    obj.name += $"{beforeParentTransform.childCount + row + 1 + col}"; // ��ü�� �����ϱ� ���ؼ� �̸��� ������ �ڵ����� �����߾��� ��ü���� �� + ���ο� + ���ο� ���� ���ڿ��� �߰�(+1�� �������� 0���� �������� �ʵ��� ����)
                else //�ƴ϶��
                    obj.name += $"{row + 1 + col}";// ��ü�� �����ϱ� ���ؼ� �̸��� ���ο� + ���ο� ���� ���ڿ��� �߰�(+1�� �������� 0���� �������� �ʵ��� ����)

                obj.transform.localPosition = pos; // ���Ӱ� ������ GameObject�� ��ġ�� ������ ������ Vector3������ �̵�
                obj.SetActive(true); // ���Ӱ� ������ GameObject Ȱ��ȭ
            }
        }

        // rows = 1
        // cols = 2 �϶�

        // 0
        // 0

        // ���� ���� �������� ���������

        // Ȥ�� ���� ���ܵδ� ���� ��ġ
        // 0.01375 �̰� ���ΰ� 2���� �ʿ��� ��
        // 0.0274 �̰� ���δ� 1����, �⹰ �ʿ��� ��

        Debug.Log("Auto Place End"); // ���� �˸��� �����
    }
}
#endif
// ������ �ۼ� ����: 2025.07.09