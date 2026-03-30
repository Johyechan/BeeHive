using InGame;
using InGame.MyManager.Global;
using InGame.MyManager.Local;
using InGame.MyObject;
using InGame.MyUI.Card;
using System.Collections.Generic;
using Tutorial.Struct;
using UnityEngine;

namespace Tutorial
{
    // 작성자: 조혜찬
    // 튜토리얼 세팅 클래스
    public class TutorialSetting : MonoBehaviour
    {
        [SerializeField] private List<TutorialUICardData> _uiCardList; // 미리 할당된 UI 카드 리스트
        [SerializeField] private List<TutorialCardObjectData> _cardList; // 미리 할당된 객체 카드 리스트

        private async void Awake()
        {
            await GameReady.Gate.WaitAsync();

            Init();
        }

        // 초기화 함수
        private void Init()
        {
            foreach(var uiCard in _uiCardList) // UI 카드 리스트 순회
            {
                foreach(var card in _cardList) // 카드 객체 리스트 순회
                {
                    if(uiCard.id == card.id) // UI 카드와 카드 객체의 id가 동일하다면(즉 매칭이 된다면)
                    {
                        uiCard.uiCard.UICardVariable.cardObj = card.cardObj.CurrentObject; // UI가 자신의 카드 객체를 id 매칭이 된 카드 객체로 할당
                        break; // 카드 객체 리스트 순회 반복문 탈출
                    }
                }
            }
        }
    }
}
// 마지막 작성 일자: 2026.03.30