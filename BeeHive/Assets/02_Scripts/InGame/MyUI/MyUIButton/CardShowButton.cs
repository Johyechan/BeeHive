using InGame.MyUI.MyUIInterface;
using UnityEngine;

namespace InGame.MyUI.MyUIButton
{
    // 작성자: 조혜찬
    // UI에서 보유한 카드들을 보여주게 만드는 UI 버튼 클래스
    public class CardShowButton : ShowButtonBase
    {
        [SerializeField] private RectTransform _cardsUI; // 카드 UI RectTransform 변수 - 위치 변경을 위해 필요
        [SerializeField] private RectTransform _piecesUI; // 기물 UI RectTransform 변수 - 위치 변경을 위해 필요

        [SerializeField] private float _showYPos; // 보여주기 위한 Y축 값
        [SerializeField] private float _showDownYPos; // 안 보여주기 위한 Y축 값

        public override void OnUIButtonClick()
        {
            ShowAnimationY(_cardsUI, _piecesUI, _showYPos, _showDownYPos);
        }
    }
}
// 마지막 작성 일자: 2025.07.07
