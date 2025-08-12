using MyUtil;
using UnityEngine;

namespace InGame.MyManager
{
    // 작성자: 조혜찬
    // 씬 매니저 싱글톤 클래스
    public class SceneMgr : MonoSingleton<SceneMgr>
    {
        private string _currentRoomID; // 현재 방 ID 저장 변수
        // 외부에서 접근 가능한 현재 방 ID 저장 변수 프로퍼티
        public string CurrentRoomID { get => _currentRoomID; set => _currentRoomID = value; }

    }
}
// 마지막 작성 일자: 2025.08.06