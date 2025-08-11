using InGame.MyManager;
using InGame.MySystem;
using InGame.MySystem.Room;
using InGame.MyUI.MyUIInterface;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.MyUI.MyUIButton
{
    // 작성자: 조혜찬
    // 준비 버튼 클래스
    public class ReadyButton : MonoBehaviour, IUIClick
    {
        [SerializeField] private TMP_Text _readyButtonText; // 준비 버튼 텍스트

        private bool _isReady; // 준비 여부

        private void Awake()
        {
            _isReady = false; // 준비 안된 상태로 초기화
        }

        // 클릭 했을 때 실행되는 함수
        public void OnUIClick()
        {
            if(_isReady) // 준비 상태라면
            {
                _readyButtonText.text = "준비"; // 버튼 텍스트 변경
                _isReady = false; // 준비 안된 상태로 변경
            }
            else // 준비가 안된 상태라면
            {
                _readyButtonText.text = "취소"; // 버튼 텍스트 변경
                _isReady = true; // 준비된 상태로 변경
            }

            var socket = NetworkManager.Instance.Socket;
            
            if(socket != null) // 서버와 통신하기 위한 Socket.IO 객체가 null이 아니라면
            {
                if (SceneMgr.Instance.CurrentRoomID != "") // 현재 방 ID가 비어있지 않을 경우
                {
                    // 준비 할때 필요한 값을 가지는 구조체 생성
                    ReadyInfo readyInfo = new ReadyInfo
                    {
                        roomID = SceneMgr.Instance.CurrentRoomID, // 현재 방 ID
                        targetID = NetworkManager.Instance.CurrentPlayerID, // 현재 클라이언트의 ID
                        isReady = _isReady // 준비 여부
                    };

                    string json = JsonUtility.ToJson(readyInfo); // json형태로 변환

                    Debug.Log("준비 여부 변경 신호 보냄");
                    socket.Emit("ready", json); // 준비 이벤트 실행
                }
            }
        }
    }
}
// 마지막 작성 일자: 2025.08.11