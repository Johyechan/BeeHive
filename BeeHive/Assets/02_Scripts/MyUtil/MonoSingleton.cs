using UnityEngine;

namespace MyUtil
{
    // 작성자: 조혜찬
    // MonoBehaviour 상속 싱글톤 클래스
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour // 기본적으로 이 클래스와 이 클래스의 자식은 MonoBehaviour를 상속 받고 있으며 T가 MonoBehaviour를 상속받고 있는 상태여야지만 이 클래스를 상속 가능
    {
        // 외부에서 참조 불가능한 인스턴스 
        private static T _instance;

        // 외부에서 참조 가능한 인스턴스 프로퍼티
        public static T Instance
        {
            get
            {
                if(_instance == null) // 만약 _instance가 널이라면
                {
                    _instance = FindFirstObjectByType<T>(); // 씬 내에서 가장 먼저 찾은 T 타입으로 지정

                    if(_instance == null) // 씬 내에 T 타입이 존재하지 않아 _instance가 널일 경우
                    {
                        GameObject newObj = new GameObject(typeof(T).Name); // 새로운 객체를 T 타입의 이름으로 새롭게 생성
                        _instance = newObj.AddComponent<T>(); // 새로운 객체에 AddComponent한 T를 _instance에 할당
                        DontDestroyOnLoad(newObj); // 씬 변경에도 삭제되지 않게 새롭게 생성한 객체를 지정
                    }
                }

                return _instance; // 인스턴스 반환
            }
        }

        protected virtual void Awake()
        {
            var instance = Instance; // 인스턴스 프로퍼티를 받아와서 _instance가 널일 경우는 새롭게 생성, 존재할 경우 그냥 _instance를 할당

            if(instance != this) // 같은 타입의 instance가 이미 존재하는 상태라면
            {
                Destroy(gameObject); // 현재 이 오브젝트를 삭제
            }
        }
    }
}
// 마지막 작성 일자: 2025.07.08