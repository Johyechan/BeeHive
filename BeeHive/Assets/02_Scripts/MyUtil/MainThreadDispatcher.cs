using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace MyUtil
{
    // 작성자: 조혜찬
    // 메인 스레드에서 할 일을 저장 시키고 실행 시키는 기능을 하는 클래스
    public class MainThreadDispatcher : MonoBehaviour
    {
        private static readonly Queue<Action> _executionQueue = new Queue<Action>(); // 메인 스레드에서 작동 되어야 할 작업들을 저장하는 큐

        private void Awake()
        {
            DontDestroyOnLoad(gameObject); // 씬 변경에도 삭제 되지 않는 상태
        }

        // 외부에서 메인 스레드에서 실행할 작업을 등록하는 정적 메서드
        public static void Enqueue(Action action)
        {
            lock(_executionQueue) // lock을 통해서 다른 스레드는 해당 큐에 접근 못하도록
            {
                _executionQueue.Enqueue(action); // 큐에 작업 추가
            }
        }

        void Update()
        {
            while(_executionQueue.Count > 0) // 큐에 작업이 있을 경우
            {
                Action action = null;
                lock(_executionQueue) // 안전하게 큐에서 꺼내기 위한 락
                {
                    action = _executionQueue.Dequeue();
                }
                try
                {
                    action?.Invoke(); // 작업 실행
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            }
        }
    }
}
// 마지막 작성 일자: 2025.08.05