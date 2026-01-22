using InGame.MyManager;
using MyUtil;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace InGame.MyNetwork
{
    // 작성자: 조혜찬
    // 방과 관련된 신호를 서버에서 받아서 처리하는 클래스
    public class RoomNetworkHandler
    {
        // 초기화 함수
        public void Init()
        {
            var socket = NetworkManager.Instance.Socket;

            socket.On("roomCreated", data =>
            {
                if (NetworkManager.Instance.IsClientOver) // 클라이언트가 종료 되었다면
                    return; // 반환

                string roomID = data.GetValue<string>();
                ChangeToRoomScene(roomID);
            }); // 서버로부터 방이 만들어졌다는 신호가 오면 방 씬으로 이동
            socket.On("joinedRoom", data =>
            {
                if (NetworkManager.Instance.IsClientOver) // 클라이언트가 종료 되었다면
                    return; // 반환

                string roomID = data.GetValue<string>();
                ChangeToRoomScene(roomID);
            }); // 서버로부터 방에 참가했다는 신호가 오면 방 씬으로 이동
        }

        // 방 씬으로 이동하는 함수(현재 방 ID)
        private void ChangeToRoomScene(string id)
        {
            MainThreadDispatcher.Enqueue(() =>
            {
                SceneMgr.Instance.CurrentRoomID = id; // 현재 참가한 방의 ID 저장
                SceneManager.LoadScene(2); // 방 씬으로 변경 추가
            });
        }
    }
}
// 마지막 작성 일자: 2026.01.22