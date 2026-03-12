namespace MyUtil.Interface
{
    // 작성자: 조혜찬
    // 상태 인스턴스
    public interface IState
    {
        public void Enter(); // 들어올 때 실행되는 함수

        public void Update(); // 지속적으로 실행되는 함수

        public void Exit(); // 나갈 때 실행되는 함수
    }
}
// 마지막 작성 일자: 2026.03.12