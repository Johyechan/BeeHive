using InGame.MyEnum;
using InGame.MyEvent;
using InGame.MyManager;
using MyUtil;
using MyUtil.MyObjectPool;
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
            for (int i = 0; i < count; i++)
            {
                switch (type)
                {
                    case TeamType.Team1:
                        GameObject road1 = ObjectPoolManager.Instance.GetObject(ObjectPoolType.Team1Road, parent);
                        PosSet(road1, type, i);
                        break;
                    case TeamType.Team2:
                        GameObject road2 = ObjectPoolManager.Instance.GetObject(ObjectPoolType.Team2Road, parent);
                        PosSet(road2, type, i);
                        break;
                    case TeamType.Team3:
                        GameObject road3 = ObjectPoolManager.Instance.GetObject(ObjectPoolType.Team3Road, parent);
                        PosSet(road3, type, i);
                        break;
                }
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
// 마지막 작성 일자: 2026.01.16