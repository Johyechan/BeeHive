using MyUtil;
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

        private bool _isTwoPlayerGame = true; // 2인용 게임을 시작했는지 3인용 게임을 시작했는지 여부 - 기본 값은 2인용 게임으로 가져가기
        public bool IsTwoPlayerGame { get => _isTwoPlayerGame; set => _isTwoPlayerGame = value; } // 위 변수 프로퍼티

        private void OnEnable()
        {
            SceneManager.sceneLoaded += CheckIsGameScene; // 씬이 로드될 때 자동으로 불리는 이벤트에 구독
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= CheckIsGameScene; // 씬이 로드될 때 자동으로 불리는 이벤트에 구독
        }

        // 게임 씬에 들어왔는지 체크하는 함수
        private void CheckIsGameScene(Scene scene, LoadSceneMode loadSceneMode)
        {
            if(scene.name.ToLower().Contains("game")) // 현재 씬 이름에 game이 들어가 있는지 확인
            {
                DeckManager.Instance.MakeDeck(CurrentRoomID); // 덱 제작
            }
        }
    }
}
// 마지막 작성 일자: 2025.10.20