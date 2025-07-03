using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

namespace InGame.MyUI
{
    // 작성자: 조혜찬
    // 보유하고 있는 UI 카드의 위치 및 각도 조정 클래스
    public class UICardArray : MonoBehaviour
    {
        [SerializeField] private float _maxAngle; // 전체 부채꼴 각도
        [SerializeField] private float _xPosPerCard; // 카드간의 x축 간격
        [SerializeField] private float _yPosPerCard; // 카드간의 y축 간격

        private RectTransform _rectTransform; // 이 스크립트를 가지는 객체의 RectTransform - 보유 중인 UI 카드들 즉, 자식의 수를 알기 위해 필요한 변수

        private float _anglePerCard; // 카드간의 각도 간격
        
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>(); // 카드들의 부모 RectTransform을 가져온다
        }

        void Update()
        {
            ChangeUICardsRotateAndPosition();
        }

        // 현재 자식(카드)들의 회전 값을 조정하는 함수
        private void ChangeUICardsRotateAndPosition()
        {
            int cardCount = _rectTransform.childCount; // 카드의 총 개수

            if (cardCount <= 1)
                return;

            _anglePerCard = _maxAngle / (cardCount - 1); // 카드간의 각도 간격
            float xPosPerCard = _xPosPerCard * (cardCount - 1); // 카드간의 x축 간격
            float yPosPerCard = _yPosPerCard * (cardCount - 1); // 카드간의 y축 간격

            for(int i = 0; i < cardCount; i++)
            {
                float indexFromCenter = (float)i - ((float)cardCount - 1) / 2; // 중앙 즉 0에서 부터 얼마나 떨어진 상태인지 확인
                float t = (float)i / ((float)cardCount - 1); // 0~1 값

                float angle = indexFromCenter * _anglePerCard; // 카드 회전 값(중앙으로부터 떨어진 값 * 카드 간의 각도 간격)

                float xPos = indexFromCenter * xPosPerCard; // 카드 x축 값(중앙으로부터 떨어진 값 * 카드 간의 x축 간격)
                float yPos = Mathf.Sin(Mathf.PI * t) * yPosPerCard; // 카드 y축 값(사인 함수(사인 함수의 n 모양을 생각하고 활용) * 카드 간의 y축 간격)

                RectTransform uiCardRectTransform = _rectTransform.GetChild(i).GetComponent<RectTransform>(); // 현재 자식(카드)의 RectTransform을 가져오기

                uiCardRectTransform.DORotate(new Vector3(0, 0, -angle), 0.1f); // angle만큼 회전 (-를 한 이유는 반대로 되야 내가 보기 편해서)
                uiCardRectTransform.DOAnchorPos(new Vector3(xPos, yPos, 0), 0.1f); // xPos, yPos만큼 이동
            }
        }
    }
}
// 마지막 작성 일자: 2025.07.03