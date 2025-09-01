using InGame.MyEvent;
using InGame.MyManager;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyUtil
{
    // 작성자: 조혜찬
    // 순차적인 작업을 할 때 사용하는 클래스
    public class TurnListFunc
    {
        private List<Func<Task>> _listActions = new List<Func<Task>>();

        // 큐에 액션 추가 함수
        public void Add(Func<Task> action)
        {
            lock (_listActions) // 다른 스레드의 접근 막기
                _listActions.Add(action); // 리스트에 액션 추가
        }

        // 큐를 비우는 함수
        public void Clear()
        {
            _listActions.Clear(); // 리스트 비우기
        }

        // 리스트에 담긴 액션들 순차적으로 실행
        public async Task ActionlistPlay()
        {
            NetworkManager.Instance.Socket.Emit("debug", $"생산 턴 액션 개수는 총 {_listActions.Count} 개");
            for(int i = 0; i < _listActions.Count; i++) // 리스트 순회
            {
                Func<Task> action = null;
                lock(_listActions)// 다른 스레드에서 접근 금지
                {
                    action = _listActions[i]; // 리스트에서 액션 꺼내기
                }
                await action?.Invoke(); // 액션 실행

                NetworkManager.Instance.Socket.Emit("debug", $"{action} 실행");
            }
        }
    }
}
// 마지막 작성 일자: 2025.08.26