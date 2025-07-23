using InGame.MyObject;
using MyUtil;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace InGame.MyManager
{
    // 작성자: 조혜찬
    // 게임의 중요한 기능들을 관리하는 싱글톤 클래스
    public class GameManager : MonoSingleton<GameManager>
    {
        private GameObject _currentMovePiece; // 현재 움직일 기물
        // 위에 변수를 외부에서 사용 및 변경하기 위한 프로퍼티
        public GameObject CurrentMovePiece
        {
            get => _currentMovePiece;
            set => _currentMovePiece = value;
        }

        
        protected override void Awake()
        {
            base.Awake();

            // 변수 초기화
            _currentMovePiece = null;
        }
    }
}
// 마지막 작성 일자: 2025.07.23
