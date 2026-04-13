#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using TMPro;

// 작성자: 조혜찬
// 폰트를 하나로 통일하기 위한 TMP_text 컴포넌트를 가지는 텍스트들을 전부 탐색하는 에디터 코드
public class AutoSelectAllTMPText
{
    [MenuItem("Tools/Select All TMP Text")]
    static void SelectAllTMP()
    {
        var texts = Object.FindObjectsByType<TMP_Text>(FindObjectsSortMode.None); // 정렬하지 않고 TMP_Text를 가지는 객체들을 저장
        Selection.objects = texts; // 찾은 TMP_Text 객체들을 선택된 상태로 변경
    }
}
#endif
// 마지막 작성 일자: 2026.04.13