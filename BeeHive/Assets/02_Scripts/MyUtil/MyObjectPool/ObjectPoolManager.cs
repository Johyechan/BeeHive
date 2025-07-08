using System.Collections.Generic;
using UnityEngine;

namespace MyUtil.MyObjectPool
{
    // 작성자: 조혜찬
    // 오브젝트 풀링 싱글톤 클래스
    public class ObjectPoolManager : MonoSingleton<ObjectPoolManager>
    {
        [SerializeField] private List<ObjectPoolData> _poolDataList; // 인스펙터에서 풀링할 데이터를 담는 리스트 변수

        private Dictionary<ObjectPoolType, ObjectPoolData> _poolDataMap = new(); // 풀링 맵 - 타입에 맞는 풀링 데이트를 할당
        private Dictionary<ObjectPoolType, Queue<GameObject>> _pool = new(); // 실제 풀 - 여기에 풀링 객체를 풀링 타입에 맞게 추가

        protected override void Awake()
        {
            Init(); // 풀 생성
        }

        // 풀 생성 함수
        private void Init()
        {
            foreach(var data in _poolDataList) // 풀링할 데이터가 담긴 리스트 순회
            {
                _poolDataMap.Add(data.poolType, data); // 풀링 맵에 리스트에 담겨있던 데이터의 값에서 가져온 풀링 타입과 데이터를 추가
            }

            foreach(var data in _poolDataMap) // 풀링 맵 순회
            {
                var poolType = data.Key; // 풀링 타입 지역 변수
                var poolData = data.Value; // 풀링 데이터 지역 변수

                _pool.Add(poolType, new Queue<GameObject>()); // 풀에 풀링 타입과 새로운 큐 추가

                for(int i = 0; i < poolData.poolCount; i++) // 풀링 데이터에서 가져온 풀에 담을 객체의 수만큼 반복
                {
                    GameObject poolObject = CreateObject(poolType); // 새로운 풀링 객체를 생성
                    _pool[poolType].Enqueue(poolObject); // 생성한 풀링 객체를 풀링 타입의 큐에 추가
                }
            }
        }

        // 새로운 풀링 객체 생성 함수(매개변수로 풀링 타입을 받는다)
        private GameObject CreateObject(ObjectPoolType type)
        {
            GameObject newObject = Instantiate(_poolDataMap[type].poolObject, transform); // 새로운 객체를 풀링 맵에서 풀링 타입의 객체를 가져온 후 부모를 풀 매니저로 지정
            newObject.transform.position = Vector3.zero; // 새로 생성한 객체 위치 초기화
            newObject.transform.rotation = Quaternion.identity; // 새로 생성한 객체 회전 초기화
            newObject.SetActive(false); // 새로 생성한 객체 비활성화
            return newObject; // 새로 생성한 객체 반환
        }

        // 외부에서 풀에서 객체를 가져올 때 부르는 함수(매개 변수로 풀링 타입, 부모 = 기본 값 null을 받는다)
        public GameObject GetObject(ObjectPoolType type, Transform parent = null)
        {
            if (_pool[type].Count > 0) // 풀링 타입의 풀에 객체가 존재한다면
            {
                GameObject obj = _pool[type].Dequeue(); // 풀링 타입의 풀에 있는 객체를 가져온다.
                obj.transform.SetParent(parent); // 풀에서 꺼낸 객체의 부모를 할당
                obj.SetActive(true); // 풀에서 꺼낸 객체 활성화
                return obj; // 풀에서 꺼낸 객체 반환
            }
            else // 만약 풀링 타입의 풀에 객체가 존재하지 않는다면
            {
                GameObject newObj = CreateObject(type); // 새롭게 풀링 타입에 맞는 객체 생성
                newObj.transform.SetParent(parent); // 새롭게 생성한 객체의 부모를 할당
                newObj.SetActive(true); // 새롭게 생성한 객체 활성화
                return newObj; // 새롭게 생성한 객체 반환
            }
        }

        // 외부에서 사용했던 객체를 다시 풀에 넣을 때 부르는 함수(매개 변수로 풀링 타입, 반환할 객체를 받는다)
        public void ReturnObject(ObjectPoolType type, GameObject returnObj)
        {
            returnObj.transform.SetParent(transform); // 반환하는 객체의 부모를 풀 매니저로 지정
            returnObj.transform.position = Vector3.zero; // 반환하는 객체의 위치 초기화
            returnObj.transform.rotation = Quaternion.identity; // 반환하는 객체의 회전 초기화

            returnObj.SetActive(false); // 반환하는 객체 비활성화

            _pool[type].Enqueue(returnObj); // 풀링 타입의 풀에 객체 추가
        }
    }
}
// 마지막 작성 일자: 2025.07.08