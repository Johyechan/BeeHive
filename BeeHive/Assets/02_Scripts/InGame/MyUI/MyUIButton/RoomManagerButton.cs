using InGame.MyUI.MyUIInterface;
using TMPro;
using UnityEngine;

namespace InGame.MyUI.MyUIButton
{
    // 작성자: 조혜찬
    // 방장 버튼 클래스
    public class RoomManagerButton : MonoBehaviour, IUIClick
    {
        [SerializeField] private int _currentIndex; // 현재 인덱스

        [SerializeField] private TMP_Text _askText; // 방장을 넘길지 묻는 텍스트

        [SerializeField] private RoomManagerChangeButton _roomManagerChangeButton; // 방장 변경을 수락하는 버튼

        public void OnUIClick()
        {
            _roomManagerChangeButton.TargetIndex = _currentIndex; // 현재 인덱스를 변경될 방장의 인덱스로 설정
            _askText.text = "방장을 넘기시겠습니까?"; // 텍스트 설정
        }
    }
}
// 마지막 작성 일자: 2025.08.11