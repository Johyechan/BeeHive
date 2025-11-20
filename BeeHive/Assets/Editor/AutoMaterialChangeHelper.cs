#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

// 작성자: 조혜찬
// 자신이 원하는 객체들의 머티리얼을 변경시켜주는 에디터 스크립트

public class AutoMaterialChangeHelper : EditorWindow
{
    public Transform changeTransformsParent; // 변경할 객체들의 부모

    public Material changeMaterial; // 변경 머티리얼

    [MenuItem("Tools/Auto Material Change Helper")]
    public static void ShowWindow() // 메뉴에서 호출 가능한 정적 함수
    {
        GetWindow<AutoMaterialChangeHelper>("Auto Material Change Helper"); // Unity 에디터에서 AutoMaterialChangeHelper 타입의 커스텀 윈도우를 생성 + Auto Material Change Helper - 타이틀
    }

    private void OnGUI() // 매 프레임마다 UI를 그려주는 함수
    {
        changeMaterial = (Material)EditorGUILayout.ObjectField("Change Material", changeMaterial, typeof(Material), true);

        changeTransformsParent = (Transform)EditorGUILayout.ObjectField("Change Transforms Parent", changeTransformsParent, typeof(Transform), true);

        if(GUILayout.Button("Auto Change"))
        {
            for(int i = 0; i <  changeTransformsParent.childCount; i++) // 자식 순회
            {
                changeTransformsParent.GetChild(i).GetComponent<Renderer>().sharedMaterial = changeMaterial;

                PrefabUtility.SavePrefabAsset(changeTransformsParent.GetChild(i).gameObject);
            }
        }
    }
}
#endif
// 마지막 작성 일자: 2025.11.17