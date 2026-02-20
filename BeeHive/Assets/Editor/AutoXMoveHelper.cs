#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

// 작성자: 조혜찬
// 자동으로 x축 이동을 해주는 헬퍼 스크립트

public class AutoXMoveHelper : EditorWindow
{
    public Transform parent; // 부모 객체

    public float xValue; // 이동시킬 값

    [MenuItem("Tools/Auto X Move Helper")]
    public static void ShowWindow()
    {
        GetWindow<AutoXMoveHelper>("Auto X Move Helper");
    }

    private void OnGUI()
    {
        parent = (Transform)EditorGUILayout.ObjectField("parent", parent, typeof(Transform), true);

        xValue = EditorGUILayout.FloatField("X Value", xValue);

        if (GUILayout.Button("Auto Set"))
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                Transform child = parent.GetChild(i);
                child.localPosition = new Vector3(child.localPosition.x + xValue, child.localPosition.y, child.localPosition.z);
            }
        }
    }
}
// 마지막 작성 일자: 2026.02.20
#endif