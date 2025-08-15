using InGame.MyManager;
using MyUtil;
using SocketIOClient;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

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
                string id = data.GetValue<string>();
                ChangeToRoomScene(id);
            }); // 서버로부터 방이 만들어졌다는 신호가 오면 방 씬으로 이동
            socket.On("joinedRoom", data =>
            {
                int setIndex = 0; // 플레이어에게 할당할 인덱스
                string json = data.GetValue().ToString(); // 문자열 형태로 값 받기
                JoinRoomInfo joinRoomInfo = JsonUtility.FromJson<JoinRoomInfo>(json); // Json 값을 JoinRoomInfo 구조체 형태로 변환

                if(joinRoomInfo.players.Length > 0) // 방에 접속해있는 플레이어의 수가 1명 이상일 경우
                {
                    for(int i = 0; i < joinRoomInfo.players.Length; i++)
                    {
                        if(joinRoomInfo.maxPlayer == 2) // 최대 인원 수가 2인일 경우
                        {
                            setIndex = joinRoomInfo.players[i].index == 0 ? 1 : 0; // 만약 현재 입장해 있는 플레이어의 UI 인덱스 번호가 0이라면 현재 플레이어에게 할당할 인덱스 번호를 1로 할당하고 1이라면 0을 할당해준다
                        }
                        else if(joinRoomInfo.maxPlayer == 3) // 최대 인원 수가 3인일 경우
                        {
                            
                        }
                    }
                }
                Sequence sequence = DOTween.Sequence()
                    .AppendCallback(() => ChangeToRoomScene(joinRoomInfo.roomID))// 서버로부터 방을 찾아서 참가했다는 신호가 오면 방 씬으로 이동
                    .AppendCallback(() =>
                    {
                        IndexSetInfo indexSetInfo = new IndexSetInfo()
                        {
                            roomID = SceneMgr.Instance.CurrentRoomID, // 현재 방 ID 할당
                            targetID = NetworkManager.Instance.CurrentPlayerID, // 현재 클라이언트 ID 할당
                            index = setIndex // 인덱스 할당
                        };

                        string json = JsonUtility.ToJson(indexSetInfo); // Json 형태로 변환
                        socket.Emit("indexSet", json); // 서버에 indexSet 이벤트 전달
                    }); // 현재 플레이어에게 UI 인덱스를 정해주는 이벤트 전달
            }); 
        }

        // 방 씬으로 이동하는 함수(현재 방 ID)
        private void ChangeToRoomScene(string id)
        {
            MainThreadDispatcher.Enqueue(() =>
            {
                SceneMgr.Instance.CurrentRoomID = id; // 현재 참가한 방의 ID 저장
                SceneManager.LoadScene(1); // 방 씬으로 변경 추가
            });

            
        }
    }
}
// 마지막 작성 일자: 2025.08.15