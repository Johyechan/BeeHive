using MyUtil;
using UnityEngine;

namespace InGame.MySystem.Room
{
    // 작성자: 조혜찬
    // 플레이어 정보 UI 세팅을 하는 핸들러 클래스
    public class PlayerInfoUISettingHandler
    {
        private PlayerInfo _player1; // 플레이어 1 정보 - 방장
        private PlayerInfo _player2; // 플레이어 2 정보
        private PlayerInfo _player3; // 플레이어 3 정보

        private RoomInfo _roomInfo; // 방 정보
        // 방 정보 프로퍼티
        public RoomInfo RoomInfo { get => _roomInfo; set => _roomInfo = value; }

        private bool _isTwoPlayer; // 2인용 게임 방인지 확인하는 변수

        private GameObject _player3UI; // 플레이어 3 객체
        private GameObject _vsUI2; // 두 번째 vs 이미지 객체

        // 생성자에서 변수 초기화
        public PlayerInfoUISettingHandler(PlayerInfo player1, PlayerInfo player2, PlayerInfo player3, GameObject player3UI, GameObject vsUI2)
        {
            _player1 = player1;
            _player2 = player2;
            _player3 = player3;
            _isTwoPlayer = false;
            _player3UI = player3UI;
            _vsUI2 = vsUI2;
        }

        public void Init()
        {
            MainThreadDispatcher.Enqueue(() =>
            {
                if (_roomInfo.maxPlayer < 3) // 최대 입장 가능 플레이어 수가 3인 이하라면
                {
                    _isTwoPlayer = true; // 2인용 플레이어로 할당
                }
            });

            MainThreadDispatcher.Enqueue(() =>
            {
                if (_isTwoPlayer)
                {
                    _player3UI.SetActive(false); // 세 번째 플레이어 정보 UI 비활성화
                    _vsUI2.SetActive(false); // 두 번쨰 vsUI 비활성화 
                }
            });

            // 배열 안에 있는 플레이어의 정보를 서버에서 가지고 클라이언트에 전해줘야 함
            // 플레이어마다 -> 방장인지 여부, 플레이어 ID랑 이름, 준비여부 만 알면 됨
            // 방장은 무조건 준비 완료 상태로 바꾸고, 추방 UI 버튼 활성화 시키고 준비 버튼이 아닌 게임 시작 버튼으로 버튼 활성화, 방장 토글 체크
            // 아닌 애들은 아님
            // 그리고 방장인지 확인하는 건 NetworkManager에서 CurrentPlayerID로 배열 돌면서 확인하기
        }
    }
}
// 마지막 작성 일자: 2025.08.06