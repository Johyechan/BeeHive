using InGame.MyManager;
using InGame.MyManager.Enum;
using InGame.MyManager.Global;
using InGame.MyUI;
using InGame.MyUI.MyUIButton;
using MyUtil;
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
        [SerializeField] private GameObject _roomListBlockPanel; // 방 리스트에 버튼이 자식으로 추가될 부모 객체

        private async void Awake()
        {
            NetworkManager.Instance.Socket.On("roomListSet", (value) => // 방 목록 세팅 이벤트
            {
                MainThreadDispatcher.Enqueue(() =>
                {
                    _roomListBlockPanel.SetActive(true); // UI 클릭 방지 시작

                    // 기존에 있는 버튼 삭제
                    for (int i = 0; i < _roomListContent.transform.childCount; i++) // 기존에 있는 버튼 순회
                    {
                        GameObject roomBtnObj = _roomListContent.transform.GetChild(i).gameObject;
                        ObjectPoolManager.Instance.ReturnObject(ObjectPoolType.RoomButton, roomBtnObj); // 버튼 반환
                    }
                });

                string json = value.GetValue().ToString();

                RoomPacket roomPacket = JsonUtility.FromJson<RoomPacket>(json);

                MainThreadDispatcher.Enqueue(() =>
                {
                    // 새로운 버튼 생성
                    for (int i = 0; i < roomPacket.roomArr.Length; i++)
                    {
                        RoomData roomData = roomPacket.roomArr[i]; // 방 정보
                        if (roomData.isPublic) // 방 공개라면
                        {
                            GameObject roomBtnObj = ObjectPoolManager.Instance.GetObject(ObjectPoolType.RoomButton, _roomListContent.transform); // 방 버튼 생성
                            RoomButton roomBtn = roomBtnObj.GetComponent<RoomButton>(); // 방 버튼 클래스 가져오기
                            roomBtn.SetRoomButton(roomData.roomName, roomData.roomID, roomData.currentPlayer, roomData.isFull, roomData.isPlaying); // 방 버튼 세팅
                        }
                    }

                    LobbyReady.Gate.Completed(); // 로비 준비 완료

                    _roomListBlockPanel.SetActive(false); // UI 클릭 방지 종료
                });
            });

            NetworkManager.Instance.Socket.Emit("callRoomListSet");

            await LobbyReady.Gate.WaitAsync(); // 로비 준비 대기

            if (SoundManager.Instance.IsFirstStart) // 게임을 이제 처음 시작하는 거라면(로비에 처음 왔다면)
            {
                SoundManager.Instance.IsFirstStart = false; // 이제 처음 시작이 아님
                SoundManager.Instance.SFXPlay(SFXType.BGM); // BGM 실행
            }
        }

        private void OnDisable()
        {
            NetworkManager.Instance.Socket.Off("roomListSet"); // 소켓 이벤트 연결 해제
        }
    }
}
// 마지막 작성 일자: 2026.02.18