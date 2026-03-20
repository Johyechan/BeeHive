using InGame.MyManager.Global;
using MyUtil.Interface;

namespace MyUtil.FSM
{
    // 작성자: 조혜찬
    // 상태 관리 클래스

    public class StateMachine
    {
        private IState _currentState; // 현재

        // 상태 변경 함수
        public void ChangeState(IState changeState)
        {
            _currentState?.Exit(); // 현재 상태 탈출

            _currentState = changeState; // 현재 상태를 변경 상태로 할당

            _currentState.Enter(); // 현재 상태 진입
        }

        // 지속적으로 실행되는 함수
        public void Update()
        {
            _currentState.Update(); // 현재 상태에서 지속적으로 실행되야 할 기능 실행
        }

        // 현재 상태 확인 함수
        public IState CurrentState()
        {
            return _currentState;
        }
    }
}
// 마지막 작성 일자: 2026.03.12