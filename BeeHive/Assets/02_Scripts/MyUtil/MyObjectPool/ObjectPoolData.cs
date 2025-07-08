using System;
using UnityEngine;

namespace MyUtil.MyObjectPool
{
    // 작성자: 조혜찬
    // 직렬화되어 인스펙터에서도 보이고 편집이 가능한 형태의 클래스
    [Serializable] // Serializable을 통해 직렬화 - 인스펙터에서 보임 + 할당/편집 가능
    public class ObjectPoolData
    {
        public ObjectPoolType poolType; // 풀링 타입

        public int poolCount; // 처음 생성하여 풀에 담아둘 객체의 수

        public GameObject poolObject; // 풀링될 객체
    }
}
// 2025.07.08