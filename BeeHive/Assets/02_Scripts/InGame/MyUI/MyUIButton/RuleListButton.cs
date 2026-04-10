using InGame.MyUI.MyUIInterface;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace InGame.MyUI.MyUIButton
{
    // 작성자: 조혜찬
    // 규칙 리스트에 있는 게임 구성 버튼
    public class RuleListButton : MonoBehaviour, IUIClick
    {
        [SerializeField] private TMP_Text _ruleTxt; // 규칙 텍스트

        [SerializeField] private string _key; // 키 값

        public void OnUIClick()
        {
            string rule = LocalizationSettings.StringDatabase.GetLocalizedString(
                "Rule",
                _key
            );

            _ruleTxt.text = rule;
        }
    }
}
// 마지막 작성 일자: 2026.04.10