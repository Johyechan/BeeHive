using InGame.MyManager;
using InGame.MySystem.Game;
using InGame.MySystem.Game.Handler;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace InGame.MySystem
{
    // 작성자: 조혜찬
    // 소켓 이벤트 연결 핸들러를 리스트로 관리하는 클래스
    public class SocketEventHandlerListMachine
    {
        private List<BaseSocketEventHandler> _socketEventList = new List<BaseSocketEventHandler>(); // 소켓 이벤트 구독 핸들러를 모아두는 리스트

        // 생성자에서 리스트 초기화 및 핸들러 추가
        public SocketEventHandlerListMachine(GoldSetHandle goldSetHandle, SetPieceHandle setPieceHandle, SetRoadHandle setRoadHandle)
        {
            CardSocketEventHandler cardSocketEventHandler = new CardSocketEventHandler(); // 카드 관련
            GoldSocketEventHandler goldSocketEventHandler = new GoldSocketEventHandler(goldSetHandle); // 금 관련
            PieceSocketEventHandler pieceSocketEventHandler = new PieceSocketEventHandler(setPieceHandle); // 기물 관련
            RoadSocketEventHandler roadSocketEventHandler = new RoadSocketEventHandler(setRoadHandle); // 도로 관련
            GameSocketEventHandler gameSocketEventHandler = new GameSocketEventHandler(); // 게임 관련

            // 리스트에 추가
            _socketEventList.Add(cardSocketEventHandler);
            _socketEventList.Add(goldSocketEventHandler);
            _socketEventList.Add(pieceSocketEventHandler);
            _socketEventList.Add(roadSocketEventHandler);
            _socketEventList.Add(gameSocketEventHandler);
        }

        // 소켓 이벤트에 연결하는 함수
        public void OnConnected()
        {
            foreach(var socketEvent in _socketEventList)
            {
                socketEvent.OnConnect(); // 각 핸들러에서 소켓 이벤트 연결
            }
        }

        public void OnDisable()
        {
            foreach(var socketEvent in _socketEventList)
            {
                socketEvent.OnDisconnect();
            }
        }
    }
}
// 마지막 작성 일자: 2026.01.30