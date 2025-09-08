using InGame.MyEnum;
using InGame.MyEvent;
using InGame.MyManager;
using MyUtil.MyObjectPool;
using UnityEngine;

namespace InGame.MySystem
{
    // 작성자: 조혜찬
    // 도로 파괴 클래스
    public class RoadDestroyer : MonoBehaviour
    {
        private void OnEnable()
        {
            PieceEvents.OnDestroyLeftRoad += DestroyLeftRoad;
        }

        private void OnDisable()
        {
            PieceEvents.OnDestroyLeftRoad -= DestroyLeftRoad;
        }

        private void DestroyLeftRoad(Transform parent, TeamType type)
        {
            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                GameObject road = parent.GetChild(i).gameObject;
                switch (type)
                {
                    case TeamType.Team1:
                        ObjectPoolManager.Instance.ReturnObject(ObjectPoolType.Team1Road, road);
                        break;
                    case TeamType.Team2:
                        ObjectPoolManager.Instance.ReturnObject(ObjectPoolType.Team2Road, road);
                        break;
                    case TeamType.Team3:
                        ObjectPoolManager.Instance.ReturnObject(ObjectPoolType.Team3Road, road);
                        break;
                }
            }
        }
    }
}
// 마지막 작성 일자: 2025.09.08