using TMPro;
using UnityEngine;

namespace InGame.MyUI.MyUIInputField
{
    // 작성자: 조혜찬
    // InputField의 기본 기능을 가지는 클래스
    public class MyInputFieldBase : MonoBehaviour
    {
        private TMP_InputField _currentInputField; // 현재 InputField

        private void Awake()
        {
            _currentInputField = GetComponent<TMP_InputField>();
        }

        private void OnDisable()
        {
            _currentInputField.SetTextWithoutNotify(""); // 이벤트를 호출하지 않고 빈칸으로 변경
            _currentInputField.caretPosition = 0; // 입력 커서 위치 초기화
        }
    }
}
// 마지막 작성 일자: 2025.12.19