using InGame.MyManager;
using MyUtil;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace InGame.MyNetwork
{
    // 작성자: 조혜찬
    // 방 정보 구조체
    public struct RoomInfo
    {
        public string ID;
        public string Name;
        public int maxPlayer;
    }

    // 방과 관련된 신호를 서버에서 받아서 처리하는 클래스
    public class RoomNetworkHandler
    {
        // 초기화 함수
        public void Init()
        {
            var socket = NetworkManager.Instance.Socket;

            // 서버로부터 방이 만들어졌다는 신호 이벤트가 온다면 기능 구독
            socket.On("roomCreated", response =>
            {
                string json = response.GetValue().ToString(); // 데이터를 string 값으로 가져오기
                RoomInfo roomInfo = JsonUtility.FromJson<RoomInfo>(json); // 가져온 Json을 RoomInfo 구조체로 변경
                MainThreadDispatcher.Enqueue(() =>
                {
                    SceneManager.LoadScene(1); // 방 씬으로 변경 추가
                });
            });
        }
    }
}
// 마지막 작성 일자: 2025.08.05