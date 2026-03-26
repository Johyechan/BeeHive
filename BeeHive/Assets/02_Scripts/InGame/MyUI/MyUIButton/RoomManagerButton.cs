using InGame.MyUI.MyUIInterface;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace InGame.MyUI.MyUIButton
{
    // 작성자: 조혜찬
    // 방장 버튼 클래스
    public class RoomManagerButton : MonoBehaviour, IUIClick
    {
        private int _targetIndex = 1; // 방장 대상 인덱스 (기본 값을 1로 정하는 이유는 방을 처음 생성 했을 때 방장이 인덱스 0번째에 배치 되기 때문에)
        public int TargetIndex { get => _targetIndex; set => _targetIndex = value; } // 방장 대상 인덱스 프로퍼티

        [SerializeField] private TMP_Text _askText; // 방장을 넘길지 묻는 텍스트

        [SerializeField] private RoomManagerChangeButton _roomManagerChangeButton; // 방장 변경을 수락하는 버튼

        public void OnUIClick()
        {
            _roomManagerChangeButton.TargetIndex = _targetIndex; // 방장 대상 인덱스를 변경될 방장의 인덱스로 설정
            _askText.text = "방장을 넘기시겠습니까?"; // 텍스트 설정
            EventSystem.current.SetSelectedGameObject(null); // 선택한 객체 초기화
        }
    }
}
// 마지막 작성 일자: 2026.03.26