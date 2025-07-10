#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

// 작성자: 조혜찬
// 자동으로 객체를 배치해주는 에디터 스크립트
public class AutoPlaceHelper : EditorWindow
{
    public GameObject prefab; // 자동으로 생성 시킬 객체

    public Transform parentTransform; // 자동으로 생성될 객체의 부모
    public Transform beforeParentTransform; // 이전에 자동으로 생성한 객체의 부모 - 객체를 구분하기 위한 이름을 추가할 때 사용할 변수

    public Vector3 startPos; // 자동으로 생성을 시작할 위치

    public int rows = 1; // 가로
    public int cols = 1; // 세로

    public float xSpacing = 0f; // 자동 생성 객체 x축 간의 간격
    public float ySpacing = 0f; // 자동 생성 객체 y축 간의 간격

    [MenuItem("Tools/Auto Place Helper")] // 메뉴명
    public static void ShowWindow() // 메뉴에서 호출 가능한 정적 함수
    {
        GetWindow<AutoPlaceHelper>("Auto Place Helper"); // Unity 에디터에서 AutoPlaceHelper 타입의 커스텀 윈도우를 생성 + Auto Place Helper - 타이틀
    }

    private void OnGUI() // 매 프레임마다 UI를 그려주는 함수
    {
        prefab = (GameObject)EditorGUILayout.ObjectField("Auto Place Object", prefab, typeof(GameObject), false); // 자동으로 생성 시킬 프리팹을 선택할 수 있는 필드 - Auto Place Object를 이름으로 가지는 필드이며 GameObject형식만을 받을 수 있으며, 씬 오브젝트는 선택 불가, prefab을 할당하면서 2번째 매개변수에 prefab을 다시 넣는 이유는 필드에서 변경된 값을 prefab이 받고 prefab에 다시 할당해줌으로써 코드내에서도 변경된 prefab의 값을 사용할 수 있게 됨
        parentTransform = (Transform)EditorGUILayout.ObjectField("Auto Place Parent", parentTransform, typeof(Transform), true); // Auto Place Parent라는 이름을 가지는 필드이며 Transform형식만을 받을 수 있으며, 씬 오브젝트 선택이 가능하다
        beforeParentTransform = (Transform)EditorGUILayout.ObjectField("Before Auto Place Parent", beforeParentTransform, typeof(Transform), true); // Before Auto Place Parent라는 이름을 가지는 필드이며 Transform형식만을 받을 수 있으며, 씬 오브젝트 선택이 가능하다
        startPos = EditorGUILayout.Vector3Field("Start Position", startPos); // Start Position이라는 이름을 가지는 필드
        rows = EditorGUILayout.IntField("Rows", rows); // Rows라는 이름을 가지는 필드
        cols = EditorGUILayout.IntField("Columns", cols); // Columns라는 이름을 가지는 필드
        xSpacing = EditorGUILayout.FloatField("X Spacing", xSpacing); // X Spacing이라는 이름을 가지는 필드
        ySpacing = EditorGUILayout.FloatField("Y Spacing", ySpacing); // Y Spacing이라는 이름을 가지는 필드

        if(GUILayout.Button("Auto Place")) // Auto Place라는 이름을 가지는 버튼 + 이 버튼이 눌리면
        {
            AutoPlaceObjects(); // 함수 실행
        }
    }

    // 자동으로 객체들을 배치하는 함수
    private void AutoPlaceObjects()
    {
        if(prefab == null || parentTransform == null) // 만약 자동 생성시킬 객체도 없고, 객체의 부모도 필드에서 할당되지 않았다면
        {
            Debug.Log("프리팹 또는 부모 객체가 없음"); // 디버그 남기기
            return; // 반환
        }

        for(int i = parentTransform.childCount - 1; i >= 0; i--) // 기존 부모 객체의 자식들을 전부 역순회 - 역순회 하는 이유는 기본 순회를 하였을 때 윗부분 먼저 지우면서 다시 자식들이 위로 올라오면서 0번째 객체가 채워지게 되고 결과적으로 잔여 자식들이 남게됨
        {
            DestroyImmediate(parentTransform.GetChild(i).gameObject); // 에디터에서 즉시 오브젝트를 지울 때 사용하는 함수 - Destroy는 다음 프레임 끝에 삭제하지만 이 함수는 즉시 삭제
        }

        for(int row = 0; row < rows; row++) // 가로열만큼 반복
        {
            for(int col = 0; col < cols; col++) // 세로열만큼 반복
            {
                Debug.Log(row);
                Vector3 pos = startPos + new Vector3(row * xSpacing, 0, col * ySpacing); // 위치는 (처음 위치 * n번째, 0, 처음 위치 * n번째)
                GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(prefab, parentTransform); // GameObjec 타입으로 프리팹 생성 (기본 Instantiate은 프리팹이 끊긴 상태로 생성하지만 이 함수는 프리팹 상태로 생성 그래서 프리팹 변경사항을 씬 오브젝트에 반영 가능 + 비교 가능, 또 에디터 전용 함수)

                if(beforeParentTransform != null) // 이전에 자동으로 생성했었던 객체가 있다면
                    obj.name += $"{beforeParentTransform.childCount + row + 1 + col}"; // 객체를 구분하기 위해서 이름에 이전에 자동으로 생성했었던 객체들의 수 + 가로열 + 세로열 값을 문자열로 추가(+1을 해줌으로 0으로 시작하지 않도록 설정)
                else //아니라면
                    obj.name += $"{row + 1 + col}";// 객체를 구분하기 위해서 이름에 가로열 + 세로열 값을 문자열로 추가(+1을 해줌으로 0으로 시작하지 않도록 설정)

                obj.transform.localPosition = pos; // 새롭게 생성한 GameObject의 위치를 위에서 지정한 Vector3값으로 이동
                obj.SetActive(true); // 새롭게 생성한 GameObject 활성화
            }
        }

        // rows = 1
        // cols = 2 일때

        // 0
        // 0

        // 위와 같은 형식으로 만들어진다

        // 혹시 몰라 남겨두는 간격 수치
        // 0.01375 이게 도로가 2개씩 필요할 때
        // 0.0274 이게 도로는 1개씩, 기물 필요할 때

        Debug.Log("Auto Place End"); // 끝을 알리는 디버그
    }
}
#endif
// 마지막 작성 일자: 2025.07.09