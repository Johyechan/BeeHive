using InGame.MyManager;
using InGame.MyUI.MyUIInterface;
using TMPro;
using UnityEngine;

namespace InGame.MyUI
{
    // 작성자: 조혜찬
    // 방을 찾는 버튼 클래스
    public class FindRoomButton : MonoBehaviour, IUIButton
    {
        [SerializeField] private TMP_InputField _inputField; // 방 이름을 적는 inputField

        public void OnUIButtonClick()
        {
        }
    }
}
// 마지막 작성 일자: 2025.07.29