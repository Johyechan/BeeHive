using System.Collections.Generic;
using DG.Tweening;
using InGame.MyManager.MyCard;
using MyUtil.MyEvent;
using Unity.VisualScripting;
using UnityEngine;

namespace InGame.MyUI
{
    // 작성자: 조혜찬
    // 보유하고 있는 UI 카드의 위치 및 각도 조정 클래스
    public class UICardArray : MonoBehaviour
    {
        [SerializeField] private int _maxCount; // 최대 보유 가능 카드 수

        [SerializeField] private float _maxAngle; // 전체 부채꼴 각도
        [SerializeField] private float _xPosPerCard; // 카드간의 x축 간격
        [SerializeField] private float _yPosPerCard; // 카드간의 y축 간격
        [SerializeField] private float _cardBaseYPos; // 기본 카드의 Y축 위치

        private RectTransform _rectTransform; // 이 스크립트를 가지는 객체의 RectTransform - 보유 중인 UI 카드들 즉, 자식의 수를 알기 위해 필요한 변수

        private float _anglePerCard; // 카드간의 각도 간격
        
        
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>(); // 카드들의 부모 RectTransform을 가져온다
        }

        private void OnEnable()
        {
            DrawEventSystem.OnDraw += ChangeUICardsRotateAndPosition; // 드로우 이벤트 구독
        }

        // 현재 자식(카드)들의 회전 값을 조정하는 함수
        private void ChangeUICardsRotateAndPosition()
        {
            int cardCount = _rectTransform.childCount; // 카드의 총 개수

            DrawManager.Instance.CanDraw = () => cardCount == _maxCount ? false : true; // 현재 보유 카드 수가 최대라면 드로우 불가 상태 아니라면 가능 상태

            if (cardCount <= 0 || cardCount > _maxCount) // 보유 중인 카드가 0개라면 또는 최대 보유 가능 수 초과라면
                return; // 그냥 반환

            if (cardCount == 1) // 보유 중인 카드가 1개 라면
            {
                RectTransform uiCardRectTransform = _rectTransform.GetChild(cardCount - 1).GetComponent<RectTransform>(); // 현재 카드의 RectTransform 할당 - 실제 값은 -1을 하지 않아야 하지만 인덱스로 활용할 것이기 때문에 -1을 하여 배열 크기 초과 오류를 방지
                uiCardRectTransform.DOAnchorPos(new Vector3(0, _cardBaseYPos, 0), 0.1f, true); // 기존 Y축 위치로 이동 + snapping을 활성화하여 정수값으로 떨어지도록 설정
                return;
            }
                

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
                uiCardRectTransform.DOAnchorPos(new Vector3(xPos, _cardBaseYPos + yPos, 0), 0.1f, true); // xPos, yPos만큼 이동 + snapping을 활성화하여 정수값으로 떨어지도록 설정
            }
        }
    }
}
// 마지막 작성 일자: 2025.07.08