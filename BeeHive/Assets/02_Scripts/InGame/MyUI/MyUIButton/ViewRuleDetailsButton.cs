using InGame.MyEnum;
using InGame.MyManager.Global;
using InGame.MyUI.MyUIInterface;
using UnityEngine;

namespace InGame.MyUI.MyUIButton
{
    // 작성자: 조혜찬
    // 세부 규칙을 볼 수 있도록 규칙이 있는 구글 드라이브로 이동하는 버튼
    public class ViewRuleDetailsButton : MonoBehaviour, IUIClick
    {
        private const string RULE_KR_URL = "https://drive.google.com/file/d/1wGsynop8qizWIKxT7mt7M_Oey0aJVwZN/view?usp=sharing"; // 한글 설명서 URL
        private const string RULE_EN_URL = "https://drive.google.com/file/d/15r0VAUeSkP_Fq3Vn9grAtJZbwxVsYaBq/view?usp=sharing"; // 영어 설명서 URL

        public void OnUIClick()
        {
            switch(LanguageManager.Instance.CurrentLanguage)
            {
                case LanguageType.Korea:
                    Application.OpenURL(RULE_KR_URL);
                    break;
                case LanguageType.English:
                    Application.OpenURL(RULE_EN_URL);
                    break;
            }
        }
    }
}
// 마지막 작성 일자: 2026.06.29