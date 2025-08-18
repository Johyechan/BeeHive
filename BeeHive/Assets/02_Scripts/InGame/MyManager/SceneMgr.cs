using DG.Tweening;
using MyUtil;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace InGame.MyManager
{
    // 작성자: 조혜찬
    // 씬 매니저 싱글톤 클래스
    public class SceneMgr : MonoSingleton<SceneMgr>
    {
        private string _currentRoomID; // 현재 방 ID 저장 변수
        // 외부에서 접근 가능한 현재 방 ID 저장 변수 프로퍼티
        public string CurrentRoomID { get => _currentRoomID; set => _currentRoomID = value; }

        public void Start()
        {
            var socket = NetworkManager.Instance.Socket; // 서버와 통신하기 위한 객체 받아오기

            if (socket != null) // 서버와 통신하기 위한 객체가 null이 아닐 경우
            {
                Debug.Log("씬 전환 이벤트 구독 Start");

                socket.On("goLobby", _ => MainThreadDispatcher.Enqueue(() => SceneManager.LoadScene(0)));// 로비 씬으로 이동

                socket.On("goGame", _ =>
                {
                    MainThreadDispatcher.Enqueue(() =>
                    {
                        Sequence sequence = DOTween.Sequence()
                            .AppendCallback(() => SceneManager.LoadScene(2)) // 게임 씬으로 이동
                            .AppendCallback(() => socket.Emit("setTeam")); // 팀을 정해달라는 이벤트를 서버에게 전달
                    }); 
                }); 
            }
        }
    }
}
// 마지막 작성 일자: 2025.08.19