using InGame.MyManager.Global;
using MyUtil.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace InGame.MyManager.Local.Boot
{
    public class GlobalManagersSetChecker : CheckerBase
    {
        // 작성자: 조혜찬
        // 글로벌 싱글톤 매니저 세팅 완료 검증 클래스

        private List<ISingletonManager> _managerList; // 글로벌 싱글톤 매니저들을 저장하는 리스트

        private float _waitTime; // 최대 대기 시간

        public GlobalManagersSetChecker(List<GameObject> list, float waitTime)
        {
            _managerList = new List<ISingletonManager>(); // 새로운 리스트 생성
            foreach(var gameObj in list)
            {
                ISingletonManager singletonManager; // 인스턴스 변수

                if(gameObj.TryGetComponent(out singletonManager)) // 생성자에서 받은 객체가 ISingletonManager 인터페이스를 가지고 있다면
                {
                    _managerList.Add(singletonManager); // 리스트에 추가
                }
            }
            _waitTime = waitTime;
        }

        protected override async Task<bool> Check()
        {
            foreach(var manager in _managerList)
            {
                float currentTime = 0;

                while (!manager.IsReady) // 매니저가 준비되지 않았다면
                {
                    if(manager == null) // 매니저의 객체가 파괴 되었다면
                    {
                        await Task.CompletedTask;
                        return false; // false 반환
                    }

                    currentTime += Time.unscaledDeltaTime;

                    if(currentTime >= _waitTime) // 최대 대기 시간을 넘어갔다면
                    {
                        await Task.CompletedTask;
                        return false; // 실패 반환
                    }

                    await Task.Yield(); // 한 프레임 대기
                }
            }

            await Task.CompletedTask;
            return true;
        }
    }
}
// 마지막 작성 일자: 2026.02.03