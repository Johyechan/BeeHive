using MyUtil.GameMode;
using UnityEngine;

namespace InGame.MyUI
{
    // 작성자: 조혜찬
    // 방 리스트 패널에서 앱 권한에 따라 방 리스트의 활성화 여부를 정하는 클래스
    public class RoomListPanel : MonoBehaviour
    {
        [SerializeField] private GameObject _roomList; // 방 리스트 객체

        private void OnEnable()
        {
            if(GameModeManager.Instance.CurrentLicenseType == LicenseType.FriendPass) // 프랜즈 패스라면
            {
                _roomList.SetActive(false); // 방 리스트 비활성화
            }
            else // 프랜즈 패스가 아닐 경우
            {
                _roomList.SetActive(true); // 방 리스트 활성화
            }
        }
    }
}
// 마지막 작성 일자: 2026.05.25