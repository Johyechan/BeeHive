using InGame.MyManager.Global;
using InGame.MyUI.MyUIInterface;
using UnityEngine;

namespace InGame.MyUI.MyUIDropdown
{
    // 작성자: 조혜찬
    // 해상도 드롭다운
    public class ResolutionDropdown : MonoBehaviour, IDropdownChanged
    {
        public void OnDropdownChanged(int index)
        {
            UIManager.Instance.CurrentResolutionIndex = index; // 현재 선택된 해상도 인덱스 저장
            ChangeResolution(); // 해상도 변경
        }

        // 현재 선택된 인덱스로 해상도 바꾸기
        private void ChangeResolution()
        {
            Resolution resolution = UIManager.Instance.ResolutionMap[UIManager.Instance.CurrentResolutionIndex]; // 현재 선택된 해상도 가져오기

            if (Screen.width == resolution.width && Screen.height == resolution.height) // 만약 해상도가 동일하다면
            {
                return;
            }

            Screen.SetResolution(resolution.width, resolution.height, FullScreenMode.FullScreenWindow); // 해상도 변경
        }
    }
}
// 마지막 작성 일자: 2026.04.14