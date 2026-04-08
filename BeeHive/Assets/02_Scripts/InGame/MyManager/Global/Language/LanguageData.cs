using InGame.MyEnum;
using System;
using UnityEngine;

namespace InGame.MyManager.Global.Language
{
    // 작성자: 조혜찬
    // 언어 리스트가 가져야할 값의 데이터 구조체
    [Serializable] // 직렬화 - 인스펙터 창에서 값을 받기 위함
    public struct LanguageData
    {
        public LanguageType languageType;
        public string localeCode;
    }
}
// 마지막 작성 일자: 2026.04.08