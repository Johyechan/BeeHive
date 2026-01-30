using System;
using TMPro;
using UnityEngine;

namespace InGame.MyManager.Boot.Struct
{
    // 작성자: 조혜찬
    // 부팅 매니저에서 Inspector 튜닝이 필요한 변수를 가지는 구조체
    [Serializable] // 직렬화 - Inspector 튜닝을 위해서(Inspector 창에서 값 할당
    public struct BootingMgrData
    {
        public CanvasGroup _gameQuitUICanvasGroup; // 게임 강제 종료 UI
        public CanvasGroup _makeNickNameCanvasGroup; // 닉네임 생성 UI

        public TMP_Text _gameQuitTxt; // 게임 강제 종료 이유 text

        public float _fadeDuration; // 페이드 지속 시간
    }
}
// 마지막 작성 일자: 2026.01.30