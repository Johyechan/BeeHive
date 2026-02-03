using InGame.MyManager;
using MyUtil.Interface;
using UnityEngine;

namespace MyUtil
{
    // 작성자: 조혜찬
    // MonoBehaviour 상속 싱글톤 클래스
    public class MonoSingleton<T> : MonoBehaviour, ISingletonManager where T : MonoBehaviour // 기본적으로 이 클래스와 이 클래스의 자식은 MonoBehaviour를 상속 받고 있으며 T가 MonoBehaviour를 상속받고 있는 상태여야지만 이 클래스를 상속 가능
    {
        [SerializeField] private bool _destroyOnLoadScene = false; // 씬이 변경되었을 때 삭제 여부

        // 외부에서 참조 불가능한 인스턴스 - static 타입의 변수는 각 제네릭 타입에 따라 한 개씩만 저장 (객체마다 하나 씩 저장 X) 
        private static T _instance;

        // 외부에서 참조 가능한 인스턴스 프로퍼티
        public static T Instance { get => _instance; }

        public bool IsReady { get; private set; } // 준비 완료 여부

        protected virtual void Awake()
        {
            if(_instance != null && _instance != this) // 같은 타입의 instance가 이미 존재하는 상태라면
            {
                Destroy(gameObject); // 현재 이 오브젝트를 삭제
            }
            else // 아니라면
            {
                _instance = this as T; // 이 객체를 T 타입으로 변형해서 인스턴스 할당
                if(!_destroyOnLoadScene) // 씬 변경 시 삭제 시키지 않을 때
                {
                    DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 삭제되지 않게 설정
                }
            }

            Ready();
        }

        protected virtual void OnDestroy()
        {
            if(_instance == this) // 삭제되는 인스턴스가 현재 타입과 같다면
            {
                _instance = null; // 인스턴스 초기화
            }
        }

        // 준비 완료 함수
        public void Ready()
        {
            IsReady = true;
        }
    }
}
// 마지막 작성 일자: 2026.02.03