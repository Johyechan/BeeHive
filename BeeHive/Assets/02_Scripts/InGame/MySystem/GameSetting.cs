using InGame.MyManager;
using InGame.MyManager.MyCard;
using InGame.MySystem.Game;
using UnityEngine;

namespace InGame.MySystem
{
    // 작성자: 조혜찬
    // 상대 클라이언트의 변경 사항을 현재 클라이언트가 알 수 있도록 세팅하는 클래스
    public class GameSetting : MonoBehaviour
    {
        [SerializeField] private Wallet _wallet;

        private GoldSetHandle _goldSetEventHandle; // 금화 및 금괴 객체 세팅 핸들러

        private void Awake()
        {
            var socket = NetworkManager.Instance.Socket; // 서버와 통신하기 위한 객체 받아오기

            if(socket != null) // 서버와 통신하기 위한 객체가 존재할 경우
            {
                socket.On("goldSet", data =>
                {
                    string json = data.GetValue().ToString(); // 문자열로 data 받기
                    PlayerDataWrapper wrapper = JsonUtility.FromJson<PlayerDataWrapper>(json); // PlayerData 구조체의 배열을 가지는 구조체로 값 받기
                    PlayerData[] players = wrapper.players; // PlayerData 배열로 저장
                    _goldSetEventHandle = new GoldSetHandle(players, _wallet); // 금화 및 금괴 객체 세팅 핸들러 생성
                    _goldSetEventHandle.Setting(); // 금화 및 금괴 객체 세팅
                });

                socket.On("setCard", (data) =>
                {
                    string json = data.GetValue().ToString(); // 문자열로 data 받기
                    PlayerDataWrapper wrapper = JsonUtility.FromJson<PlayerDataWrapper>(json); // PlayerData 구조체 배열을 가지는 구조체로 값 받기
                    PlayerData[] players = wrapper.players; // PlayerData 배열로 저장
                    _ = DrawManager.Instance.CardSetHandle.Setting(players); // Task 반환 없이 바로 실행 - 이거 위치 조정 하자
                });
            }
        }
    }
}
// 마지막 작성 일자: 2025.08.28