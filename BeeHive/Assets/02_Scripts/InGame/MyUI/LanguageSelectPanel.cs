using InGame.MyEnum;
using InGame.MyManager.Global;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.MyUI
{
    // 작성자: 조혜찬
    // 맨 처음 언어 선택 창 클래스
    public class LanguageSelectPanel : MonoBehaviour
    {
        [SerializeField] private Toggle _koreaToggle; // 한글 토글
        [SerializeField] private Toggle _englishToggle; // 영어 토글

        private void OnEnable()
        {
            SetLanguageToggle(); // 선택된 언어 토글 초기화
        }

        // 선택된 언어 토글 초기화 함수
        private void SetLanguageToggle()
        {
            switch (LanguageManager.Instance.CurrentLanguage)
            {
                case LanguageType.Korea:
                    _koreaToggle.isOn = true;
                    break;
                case LanguageType.English:
                    _englishToggle.isOn = true;
                    break;
            }
        }
    }
}
// 마지막 작성 일자: 2026.04.14