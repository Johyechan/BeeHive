using InGame.MyUI.MyUIInterface;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.MyUI
{
    // 작성자: 조혜찬
    // 방을 만드는 버튼
    public class MakeRoomButton : MonoBehaviour, IUIButton
    {
        [SerializeField] private InputField _inputField; // 방 이름을 적을 InputField

        // 클릭 시 실행될 함수
        public void OnUIButtonClick()
        {
            
        }
    }
}
// 마지막 작성 일자: 2025.07.29