using InGame.MyEnum;
using InGame.MyEvent;
using MyUtil.MyObjectPool;
using System;
using UnityEngine;

namespace InGame.MySystem
{
    public class RoadMaker : MonoBehaviour
    {
        [SerializeField] private float _zInterval; // z축 간격
        [SerializeField] private float _angle; // 도로 각도

        private void OnEnable()
        {
            PieceEvents.OnGetRoad += MakeRoad;
        }

        private void OnDisable()
        {
            PieceEvents.OnGetRoad -= MakeRoad;
        }

        // 도로 생성 함수(생성 개수, 어떤 팀의 도로인지)
        private void MakeRoad(int count, TeamType type, Transform parent)
        {
            ObjectPoolType objectPoolType = type switch
            {
                TeamType.Team1 => ObjectPoolType.Team1Road, // 팀1은 팀1 도로 타입 반환
                TeamType.Team2 => ObjectPoolType.Team2Road, // 팀2은 팀2 도로 타입 반환
                TeamType.Team3 => ObjectPoolType.Team3Road, // 팀3은 팀3 도로 타입 반환
                _ => throw new ArgumentOutOfRangeException() // type이 허용 범위를 벗어남
            };

            for (int i = 0; i < count; i++)
            {
                MakeRoad(type, objectPoolType, parent, i);
            }
        }

        private void MakeRoad(TeamType type, ObjectPoolType objectPoolType, Transform parent, int count)
        {
            Vector3 pos = Vector3.zero;

            switch (type)
            {
                case TeamType.Team1:
                    pos = new Vector3(0, 0, count * _zInterval); // 위치
                    break;
                case TeamType.Team2:
                    pos = new Vector3(0, 0, count * _zInterval); // 위치
                    break;
                case TeamType.Team3:
                    break;
            }

            ObjectPoolManager.Instance.MakeObject(objectPoolType, pos, parent, -1, _angle); // 도로 생성
        }
    }
}
// 마지막 작성 일자: 2026.02.21