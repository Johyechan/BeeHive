using InGame.MyManager;
using InGame.MyManager.Global;
using UnityEngine;

namespace InGame.MySystem.Game
{
    // 작성자: 조혜찬
    // 상대 클라이언트의 변경 사항을 현재 클라이언트가 알 수 있도록 세팅하는 클래스
    public class GameSetting : MonoBehaviour
    {
        [SerializeField] private Wallet _wallet;

        private GoldSetHandle _goldSetEventHandle; // 금화 및 금괴 객체 세팅 핸들러

        private SetPieceHandle _setPieceHandle; // 기물 이동, 생성 핸들러

        private SetRoadHandle _setRoadHandle; // 도로 생성 핸들러

        private SocketEventHandlerListMachine _socketEventHandlerListMachine; // 소켓 이벤트 구독 핸들러 리스트를 한 번에 처리하는 클래스

        private void Awake()
        {
            _goldSetEventHandle = new GoldSetHandle(_wallet); // 금화 및 금괴 객체 세팅 핸들러 생성
            _setPieceHandle = new SetPieceHandle(); // 기물 객체 이동 핸들러 생성
            _setRoadHandle = new SetRoadHandle(); // 도로 객체 생성 핸들러

            var socket = NetworkManager.Instance.Socket; // 서버와 통신하기 위한 객체 받아오기

            if(socket != null) // 서버와 통신하기 위한 객체가 존재할 경우
            {
                _socketEventHandlerListMachine = new SocketEventHandlerListMachine(_goldSetEventHandle, _setPieceHandle, _setRoadHandle);
                _socketEventHandlerListMachine.OnConnected(); // 소켓 이벤트 연결
            }
        }

        private void OnDisable()
        {
            _socketEventHandlerListMachine.OnDisable();
        }
    }
}
// 마지막 작성 일자: 2026.05.19