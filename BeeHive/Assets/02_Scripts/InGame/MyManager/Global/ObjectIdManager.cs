using MyUtil;
using MyUtil.Interface;
using System.Collections.Generic;
using UnityEngine;

namespace InGame.MyManager.Global
{
    // 작성자: 조혜찬
    // 객체 id 관리 매니저
    public class ObjectIdManager : MonoSingleton<ObjectIdManager>
    {
        private Dictionary<int, GameObject> _idMap = new Dictionary<int, GameObject>();

        public int Id { get; set; } // 객체 ID

        // 외부에서 id를 통해 객체를 찾을 때 사용하는 함수
        public GameObject FindObject(int id)
        {
            if(CanFindObject(id)) // 객체를 찾을 수 있다면
            {
                return _idMap[id]; // id를 가지는 객체 반환
            }

            return null;
        }

        // id를 통해 객체를 찾는 함수
        private bool CanFindObject(int id)
        {
            if (_idMap.ContainsKey(id)) // 만약 id가 존재한다면
                return true;

            return false;
        }

        public void AddObject(int id, GameObject obj)
        {
            if (CanFindObject(id)) // 만약 id를 가진 객체가 존재한다면 
                return; // 반환

            _idMap[id] = obj; // id 및 객체 추가
        }

        // 초기화 함수
        public void ResetMap()
        {
            Id = 0;
            _idMap.Clear();
        }
    }
}
// 마지막 작성 일자: 2026.02.12