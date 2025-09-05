using InGame.MyEnum;
using InGame.MyEvent;
using InGame.MyManager;
using MyUtil.MyObjectPool;
using UnityEngine;

public class RoadMaker : MonoBehaviour
{
    private Transform _parent; // 도로 기물 부모

    [SerializeField] private float _xInterval;
    [SerializeField] private float _zInterval;
    [SerializeField] private float _angle;
    [SerializeField] private int _maxXCount;

    private void Awake()
    {
        _parent = GameObject.Find(TeamManager.Instance.RoadParentName).transform; // 도로 객체의 부모 객체 찾기
    }

    private void OnEnable()
    {
        PieceEvents.OnGetRoad += MakeRoad;
        PieceEvents.OnRoadDestroy += DestroyRoad;
    }

    private void OnDisable()
    {
        PieceEvents.OnGetRoad -= MakeRoad;
        PieceEvents.OnRoadDestroy -= DestroyRoad;
    }

    private void DestroyRoad()
    {
        for(int i = _parent.childCount - 1; i >= 0; i--)
        {
            GameObject road = _parent.GetChild(i).gameObject;
            switch (TeamManager.Instance.CurrentTeamType)
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

    private void MakeRoad(int count)
    {
        for(int i = 0; i < count; i++)
        {
            switch (TeamManager.Instance.CurrentTeamType)
            {
                case TeamType.Team1:
                    GameObject road1 = ObjectPoolManager.Instance.GetObject(ObjectPoolType.Team1Road, _parent);
                    PosSet(road1);
                    break;
                case TeamType.Team2:
                    GameObject road2 = ObjectPoolManager.Instance.GetObject(ObjectPoolType.Team2Road, _parent);
                    PosSet(road2);
                    break;
                case TeamType.Team3:
                    GameObject road3 = ObjectPoolManager.Instance.GetObject(ObjectPoolType.Team3Road, _parent);
                    PosSet(road3);
                    break;
            }
        }
    }

    private void PosSet(GameObject obj)
    {
        int count = _parent.childCount - 1; // 도로 객체의 수 구하기(-1을 해서 이전 객체 수일 때를 기준으로 위치를 조정)
        obj.transform.Rotate(0, _angle, 0); // 회전
        obj.transform.localPosition = new Vector3(count % _maxXCount * _xInterval, 0, count / _maxXCount * _zInterval); // 위치
    }
}
// 마지막 작성 일자: 2025.09.05