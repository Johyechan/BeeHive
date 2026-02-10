using InGame.MyManager;
using InGame.MyManager.Enum;
using InGame.MyManager.Global;
using InGame.MyUI;
using InGame.MyUI.MyUIButton;
using MyUtil.MyObjectPool;
using TMPro;
using UnityEngine;

namespace InGame.MySystem.Lobby
{
    // 작성자: 조혜찬
    // 로비 세팅 클래스
    public class LobbySetting : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nickNameText; // 닉네임 텍스트

        [SerializeField] private GameObject _roomListContent; // 방 리스트에 버튼이 자식으로 추가될 부모 객체

        private void Awake()
        {
            if(SoundManager.Instance.IsFirstStart) // 게임을 이제 처음 시작하는 거라면(로비에 처음 왔다면)
            {
                SoundManager.Instance.IsFirstStart = false; // 이제 처음 시작이 아님
                SoundManager.Instance.SFXPlay(SFXType.BGM); // BGM 실행
            }

            NetworkManager.Instance.Socket.On("roomListSet", (value) => // 방 목록 세팅 이벤트
            {
                string json = value.GetValue().ToString();

                RoomPacket roomPacket = JsonUtility.FromJson<RoomPacket>(json);

                for(int i = 0; i < roomPacket.roomArr.Length; i++)
                {
                    GameObject roomBtnObj = ObjectPoolManager.Instance.GetObject(ObjectPoolType.RoomButton, _roomListContent.transform); // 방 버튼 생성
                    RoomButton roomBtn = roomBtnObj.GetComponent<RoomButton>(); // 방 버튼 클래스 가져오기
                    RoomData roomData = roomPacket.roomArr[i]; // 방 정보
                    roomBtn.SetRoomButton(roomData.roomName, roomData.roomID, roomData.currentPlayer, roomData.isFull, roomData.isPlaying); // 방 버튼 세팅
                }

                LobbyReady.Gate.Completed(); // 로비 준비 완료
            });

            NetworkManager.Instance.Socket.Emit("callRoomListSet");
        }

        private void OnDisable()
        {
            NetworkManager.Instance.Socket.Off("roomListSet"); // 소켓 이벤트 연결 해제
        }
    }
}
// 마지막 작성 일자: 2026.02.10