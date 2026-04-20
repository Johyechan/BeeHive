using InGame.MyManager.Enum;
using InGame.MyManager.Global;
using MyUtil;
using System.Net.Sockets;
using System.Threading.Tasks;
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

        private string _otherNickName; // 상대 닉네임
        public string OtherNickName { get => _otherNickName; set => _otherNickName = value; } // 상대 닉네임 프로퍼티
        
        private bool _isTwoPlayerGame = true; // 2인용 게임을 시작했는지 3인용 게임을 시작했는지 여부 - 기본 값은 2인용 게임으로 가져가기
        public bool IsTwoPlayerGame { get => _isTwoPlayerGame; set => _isTwoPlayerGame = value; } // 위 변수 프로퍼티

        private SceneFlowType _currentSceneFlow; // 현재 씬 흐름 변수

        private SceneType _currentSceneType = SceneType.Boot; // 현재 씬 타입 - 처음 씬 타입을 부팅 씬으로 저장

        protected override void Awake()
        {
            base.Awake();

            _ = Init();
        }

        private void OnDisable()
        {
            NetworkManager.Instance.Socket.Off("goLobby");
        }

        private async Task Init()
        {
            await NetworkManager.Instance.WaitSocketConnected();

            NetworkManager.Instance.Socket.On("goLobby", _ =>
            {
                if (NetworkManager.Instance.IsClientOver) // 클라이언트가 종료 되었다면
                    return; // 반환

                _currentSceneFlow = SceneFlowType.GoLobby; // 로비로 이동하는 흐름으로 변경

                if (_currentSceneType == SceneType.Room) // 현재 씬이 방 씬이라면
                {
                    LoadScene(); // 씬 전환
                }
            });

            Ready();
        }

        // 씬 흐름 변경 함수
        public void ChangeCurrentSceneFlow(SceneFlowType flowType)
        {
            if (_currentSceneFlow != SceneFlowType.None) // 씬 흐름이 존재한다면
            {
                return; // 반환
            }

            _currentSceneFlow = flowType; // 씬 흐름 변경
        }

        // 씬 변경 함수
        public void LoadScene()
        {
            switch(_currentSceneFlow)
            {
                case SceneFlowType.GoLobby: // 로비로 가는 흐름
                    _currentSceneType = SceneType.Lobby;
                    SceneManager.LoadScene((int)_currentSceneType); // 로비 씬으로 이동
                    break;
                case SceneFlowType.GoRoom: // 방으로 가는 흐름
                    _currentSceneType = SceneType.Room;
                    SceneManager.LoadScene((int)_currentSceneType); // 방 씬으로 이동
                    break;
                case SceneFlowType.GoGame: // 게임으로 가는 흐름
                    _currentSceneType = SceneType.Game;
                    SceneManager.LoadScene((int)_currentSceneType); // 게임 씬으로 이동
                    break;
                case SceneFlowType.GoTutorial: // 튜토리얼로 가는 흐름
                    _currentSceneType = SceneType.Tutorial;
                    SceneManager.LoadScene((int)_currentSceneType); // 튜토리얼 씬으로 이동
                    break;
            }
            _currentSceneFlow = SceneFlowType.None; // 흐름 초기화
        }
    }
}
// 마지막 작성 일자: 2026.04.20