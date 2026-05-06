using InGame.MyEnum;
using InGame.MyEvent;
using InGame.MyManager.Global;
using InGame.MyManager.Local;
using MyUtil.GameMode;
using MyUtil.MyObjectPool;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace InGame.MySystem
{
    public class RoadMaker : MonoBehaviour
    {
        [SerializeField] private float _zInterval; // z축 간격
        [SerializeField] private float _angle; // 도로 각도
        [SerializeField] private int _makeDelayMillisecond; // 생성 딜레이

        private void OnEnable()
        {
            PieceEvents.OnGetRoad += MakeRoad;
        }

        private void OnDisable()
        {
            PieceEvents.OnGetRoad -= MakeRoad;
        }

        // 도로 생성 함수(생성 개수, 어떤 팀의 도로인지)
        private async Task MakeRoad(int count, TeamType type, Transform parent)
        {
            ObjectPoolType objectPoolType = type switch
            {
                TeamType.Team1 => ObjectPoolType.Team1Road, // 팀1은 팀1 도로 타입 반환
                TeamType.Team2 => ObjectPoolType.Team2Road, // 팀2은 팀2 도로 타입 반환
                _ => throw new ArgumentOutOfRangeException() // type이 허용 범위를 벗어남
            };

            for (int i = 0; i < count; i++)
            {
                await MakeRoad(type, objectPoolType, parent, i);
            }
        }

        private async Task MakeRoad(TeamType type, ObjectPoolType objectPoolType, Transform parent, int count)
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
            }

            if(GameModeManager.Instance.CurrentGameMode.IsTutorial()) // 튜토리얼 일 경우
            {
                GameObject road = ObjectPoolManager.Instance.GetObject(objectPoolType, parent);
                road.transform.localPosition = new Vector3(pos.x, ObjectPoolManager.Instance.AnimationYPos, pos.z);
                road.transform.Rotate(0, _angle, 0);
                ObjectPoolManager.Instance.Animation(road, true, true, pos.y);
                await Task.Delay(_makeDelayMillisecond);
            }
            else // 튜토리얼이 아닐 때
            {
                ObjectPoolManager.Instance.MakeObject(objectPoolType, pos, parent, true, -1, _angle); // 도로 생성
                await Task.Delay(_makeDelayMillisecond);
            }
        }
    }
}
// 마지막 작성 일자: 2026.05.06