using InGame.MyEnum;
using InGame.MyManager.Global.Language;
using MyUtil;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace InGame.MyManager.Global
{
    public class LanguageManager : MonoSingleton<LanguageManager>
    {
        [SerializeField] private List<LanguageData> _languageList = new List<LanguageData>(); // 언어 리스트

        [SerializeField] private string _defaultLanguageValue; // 기본 언어 값

        private Dictionary<LanguageType, Locale> _languageMap = new Dictionary<LanguageType, Locale>(); // 언어 맵
        private Dictionary<string, LanguageType> _findLanguageTypeMap = new Dictionary<string, LanguageType>(); // 언어 타입 탐색용 맵

        private const string CURRENT_LANGUAGE = "CurrentLanguage";

        // 현재 언어 프로퍼티
        public LanguageType CurrentLanguage 
        {
            get
            {
                string currentLanguageCode = PlayerPrefs.GetString(CURRENT_LANGUAGE, _defaultLanguageValue);
                return _findLanguageTypeMap[currentLanguageCode];
            }
            set
            {
                Locale locale = _languageMap[value]; // 현재 장소
                PlayerPrefs.SetString(CURRENT_LANGUAGE, locale.Identifier.Code); // 현재 장소의 언어 코드 저장
            }
        } 

        protected override void Awake()
        {
            base.Awake();

            foreach(var data in _languageList)
            {
                LanguageType type = data.languageType; // 언어 타입
                Locale locale = LocalizationSettings.AvailableLocales.GetLocale(data.localeCode); // 현재 사용 가능한 장소 중에서 data의 localeCode와 일치하는 장소 가져오기
                string code = locale.Identifier.Code; // 현재 장소의 언어 코드

                if (locale != null) // 장소가 존재할 때
                {
                    if (!_languageMap.ContainsKey(type)) // type에 해당하는 언어 타입이 맵에 없다면
                    {
                        _languageMap.Add(type, locale); // 맵에 추가
                    }
                    if(!_findLanguageTypeMap.ContainsKey(code)) // 현재 장소의 언어 코드가 맵에 없다면
                    {
                        _findLanguageTypeMap.Add(code, type); // 맵에 추가
                    }
                }
            }

            LoadLocal(CurrentLanguage); // 현재 언어 저장

            Ready();
        }

        public void LoadLocal(LanguageType type)
        {
            string currentCode = LocalizationSettings.SelectedLocale.Identifier.Code;
            string typeCode = _languageMap[type].Identifier.Code;

            if (currentCode == typeCode) // 현재 언어와 변경하려는 언어가 같다면
            {
                return; // 반환
            }

            if(_languageMap.ContainsKey(type)) // 만약 type에 해당하는 언어 타입이 맵에 있다면
            {
                Locale locale = _languageMap[type];
                LocalizationSettings.SelectedLocale = locale; // 언어 변경
                CurrentLanguage = type; // 현재 언어 저장
            }
        }
    }
}
// 마지막 작성 일자: 2026.04.09