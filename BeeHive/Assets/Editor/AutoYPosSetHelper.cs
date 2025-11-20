#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

// 작성자: 조혜찬
// 자동으로 y축 간격을 맞춰주는 헬퍼 스크립트

public class AutoYPosSetHelper : EditorWindow
{
    public Transform parent; // 부모 객체

    public float yInterval; // y축 간격

    [MenuItem("Tools/Auto Y Pos Set Helper")]
    public static void ShowWindow()
    {
        GetWindow<AutoYPosSetHelper>("Auto Y Pos Set Helper");
    }

    private void OnGUI()
    {
        parent = (Transform)EditorGUILayout.ObjectField("parent", parent, typeof(Transform), true);

        yInterval = EditorGUILayout.FloatField("Y Interval", yInterval);

        if(GUILayout.Button("Auto Set"))
        {
            for(int i = 0; i < parent.childCount; i++)
            {
                Transform child = parent.GetChild(i);
                child.localPosition = new Vector3(child.localPosition.x, i * yInterval, child.localPosition.z);
            }
        }
    }
}
// 마지막 작성 일자: 2025.11.20
#endif