using InGame.MyManager;
using InGame.MyManager.Global;
using InGame.MyUI.MyUIInterface;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;

namespace InGame.MyUI.MyUIButton
{
    // 작성자: 조혜찬
    // 방 버튼

    public class RoomButton : MonoBehaviour, IUIClick
    {
        [SerializeField] private TMP_Text _roomName; // 방 이름
        [SerializeField] private TMP_Text _currentPlayerCount; // 참가자 수

        private string _roomID; // 방 ID

        private bool _isFull = false; // 참가자 최대 여부
        private bool _isPlaying = false; // 플레이 중 여부

        // 방 버튼 세팅 함수
        public void SetRoomButton(string roomName, string roomID, int currentPlayer, bool isFull, bool isPlaying)
        {
            _roomName.text = roomName;
            _roomID = roomID;
            _currentPlayerCount.text = $"{currentPlayer}/2";
            _isFull = isFull;
            _isPlaying = isPlaying;
        }

        // 버튼 클릭 시 실행될 함수
        public void OnUIClick()
        {
            if(_isPlaying) // 플레이 중이라면
            {
                string isPlay = LocalizationSettings.StringDatabase.GetLocalizedString(
                    "Game",
                    "Game_UI_IsPlay"
                );
                UIManager.Instance.WarningUIMake(isPlay);
            }
            else if(_isFull) // 참가자가 꽉 차있다면
            {
                string isFull = LocalizationSettings.StringDatabase.GetLocalizedString(
                    "Game",
                    "Game_UI_IsFull"
                );
                UIManager.Instance.WarningUIMake(isFull);
            }
            else // 괜찮은 상황이라면
            {
                NetworkManager.Instance.Socket.Emit("joinRoom", _roomID); // 서버에 방을 찾고 있다고 신호를 보냄
            }
            EventSystem.current.SetSelectedGameObject(null); // 선택한 객체 초기화
        }
    }
}
// 마지막 작성 일자: 2026.04.06