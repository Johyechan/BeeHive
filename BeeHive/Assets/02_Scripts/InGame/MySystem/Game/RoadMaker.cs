using InGame.MyEnum;
using InGame.MyEvent;
using InGame.MyManager;
using MyUtil;
using MyUtil.MyObjectPool;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace InGame.MySystem
{
    public class RoadMaker : MonoBehaviour
    {
        [SerializeField] private float _xInterval;
        [SerializeField] private float _zInterval;
        [SerializeField] private float _angle;
        [SerializeField] private int _maxXCount;

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
                GameObject road = ObjectPoolManager.Instance.GetObject(objectPoolType, parent);
                PosSet(road, type, i);
            }
        }

        private void PosSet(GameObject obj, TeamType type, int count)
        {
            MainThreadDispatcher.Enqueue(() =>
            {
                obj.transform.Rotate(0, _angle, 0); // 회전
                switch (type)
                {
                    case TeamType.Team1:
                        obj.transform.localPosition = new Vector3(count % _maxXCount * _xInterval, 0, count / _maxXCount * _zInterval); // 위치
                        break;
                    case TeamType.Team2:
                        obj.transform.localPosition = new Vector3(-(count % _maxXCount * _xInterval), 0, count / _maxXCount * _zInterval); // 위치
                        break;
                    case TeamType.Team3:
                        break;
                }
            });
        }
    }
}
// 마지막 작성 일자: 2026.01.19