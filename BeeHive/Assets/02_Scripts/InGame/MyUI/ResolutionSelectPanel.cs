using InGame.MyManager.Global;
using TMPro;
using UnityEngine;

namespace InGame.MyUI
{
    // 작성자: 조혜찬
    // 해상도 선택 패널
    public class ResolutionSelectPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown _resolutionDropdown; // 해상도 드롭 다운

        private void OnEnable()
        {
            SetResolution(); // 해상도 변경
        }

        private void SetResolution()
        {
            _resolutionDropdown.value = UIManager.Instance.CurrentResolutionIndex; // 선택된 인덱스를 현재 선택한 해상도 인덱스로 변경
            _resolutionDropdown.RefreshShownValue(); // 보이는 UI 변경
        }
    }
}
// 마지막 작성 일자: 2026.04.14