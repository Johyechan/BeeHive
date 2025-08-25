using InGame.MyEvent;
using System;
using System.Collections.Generic;

namespace MyUtil
{
    // 작성자: 조혜찬
    // 순차적인 작업을 할 때 사용하는 클래스
    public class TurnListAction
    {
        private List<Action> _listActions = new List<Action>();

        // 큐에 액션 추가 함수
        public void Add(Action action)
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
        public void ActionlistPlay()
        {
            for(int i = 0; i < _listActions.Count; i++) // 리스트 순회
            {
                Action action = null;
                lock(_listActions)// 다른 스레드에서 접근 금지
                {
                    action = _listActions[i]; // 리스트에서 액션 꺼내기
                }
                action?.Invoke(); // 액션 실행
            }

            TurnEvents.OnChangeTurn?.Invoke(); // 마지막에 턴 변경 이벤트 실행
        }
    }
}
// 마지막 작성 일자: 2025.08.25