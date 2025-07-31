using TMPro;
using UnityEngine.UI;

namespace InGame.MyUI.TurnUI
{
    // 작성자: 조혜찬
    // 턴마다 나오는 UI 애니메이션 클래스들의 부모 클래스
    public abstract class TurnUIAnimationHandlerBase
    {
        protected Image _backgroundImage; // 애니메이션 백그라운드 이미지 변수

        protected TMP_Text _tmpText; // 현재 턴을 보여줄 text 변수

        public TurnUIAnimationHandlerBase(Image backgroundImage, TMP_Text tmpText)
        {
            _backgroundImage = backgroundImage;
            _tmpText = tmpText;
        }

        // 애니메이션을 구현할 함수
        public abstract void UIAnimationPlay();
    }
}
// 마지막 작성 일자: 2025.07.31