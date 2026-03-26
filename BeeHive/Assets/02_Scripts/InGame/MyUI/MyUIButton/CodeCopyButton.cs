using InGame.MyUI.MyUIInterface;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace InGame.MyUI
{
    // 작성자: 조혜찬
    // 방 ID 복사 버튼
    public class CodeCopyButton : MonoBehaviour, IUIClick
    {
        [SerializeField] private TMP_Text _roomID;

        public void OnUIClick()
        {
            GUIUtility.systemCopyBuffer = _roomID.text; // 방 ID 복사
            EventSystem.current.SetSelectedGameObject(null); // 선택한 객체 초기화
        }
    }
}
// 마지막 작성 일자: 2026.03.26