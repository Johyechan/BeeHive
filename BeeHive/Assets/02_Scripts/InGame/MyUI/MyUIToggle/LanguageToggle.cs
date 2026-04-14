using InGame.MyEnum;
using InGame.MyManager.Global;
using InGame.MyUI.MyUIInterface;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.MyUI.MyUIToggle
{
    // 작성자: 조혜찬
    // 언어 선택 토글
    public class LanguageToggle : MonoBehaviour, IToggleChanged
    {
        [SerializeField] private LanguageType _languageType; // 현재 토글의 언어 타입

        [SerializeField] private List<Toggle> _otherToggleList = new List<Toggle>(); // 현재 토글 이외의 토글 리스트

        public void OnToggleChanged(bool isOn)
        {
            if(isOn)
            {
                LanguageManager.Instance.LoadLocal(_languageType);
                foreach(var toggle in _otherToggleList) // 현재 토글 이외의 토글 리스트 순회
                {
                    toggle.isOn = false; // 비선택 상태로 전환
                }
            }
        }
    }
}
// 마지막 작성 일자: 2026.04.08