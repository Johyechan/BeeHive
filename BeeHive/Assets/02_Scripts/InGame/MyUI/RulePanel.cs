using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization.Settings;

namespace InGame.MyUI
{
    // 작성자: 조혜찬
    // 게임 설명서 패널
    public class RulePanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _ruleTmp; // 규칙 텍스트

        // 활성화 시
        private void OnEnable()
        {
            // 언어 적용
            // 처음 규칙으로 보여주기
            string rule = LocalizationSettings.StringDatabase.GetLocalizedString(
                "Rule",
                "Rule_Text_HowToWin"
            );

            _ruleTmp.text = rule;
        }
    }
}
// 마지막 작성 일자: 2026.06.09