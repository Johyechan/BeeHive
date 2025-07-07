using InGame.MyUI.MyUIInterface;
using UnityEngine;

namespace InGame.MyUI.MyUIButton
{
    // 작성자: 조혜찬
    // UI에서 기물들을 보여주게 만드는 UI 버튼 클래스
    public class PieceShowButton : ShowButtonBase
    {
        [SerializeField] private RectTransform _cardsUI; // 카드 UI RectTransform 변수 - 비활성화를 위해 필요
        [SerializeField] private RectTransform _piecesUI; // 기물 UI RectTransform 변수 - 활성화를 위해 필요

        [SerializeField] private float _showYPos; // 보여주기 위한 Y축 값
        [SerializeField] private float _showDownYPos; // 안 보여주기 위한 Y축 값

        public override void OnUIButtonClick()
        {
            ShowAnimationY(_piecesUI, _cardsUI, _showYPos, _showDownYPos);
        }
    }
}
// 마지막 작성 일자: 2025.07.07