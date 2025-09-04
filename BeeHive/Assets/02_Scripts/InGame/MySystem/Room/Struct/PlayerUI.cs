using System;
using TMPro;
using UnityEngine.UI;

namespace InGame.MySystem
{
    // 작성자: 조혜찬
    // 플레이어 정보들을 보여주는 UI를 가지는 구조체
    [Serializable] // 직렬화하여 인스펙터 창에서 값을 할당 가능 + 플레이어 정보 구조체
    public struct PlayerUI
    {
        public TMP_Text playerNameText; // 플레이어 이름
        public TMP_Text readyText; // 준비 여부 텍스트
        public TMP_Text readyButtonText; // 준비 버튼 텍스트
        public Image roomManagerImage; // 방장 여부 토글
        public Image readyImage; // 준비 여부 이미지
        public Button exileButton; // 추방 버튼
        public Button roomManagerButton; // 추방 버튼
    }
}
// 마지막 작성 일자: 08.08