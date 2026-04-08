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

        private string _otherNickName; // 상대 닉네임
        public string OtherNickName { get => _otherNickName; set => _otherNickName = value; } // 상대 닉네임 프로퍼티
        
        private bool _isTwoPlayerGame = true; // 2인용 게임을 시작했는지 3인용 게임을 시작했는지 여부 - 기본 값은 2인용 게임으로 가져가기
        public bool IsTwoPlayerGame { get => _isTwoPlayerGame; set => _isTwoPlayerGame = value; } // 위 변수 프로퍼티

        protected override void Awake()
        {
            base.Awake();

            Ready();
        }

    }
}
// 마지막 작성 일자: 2026.04.08