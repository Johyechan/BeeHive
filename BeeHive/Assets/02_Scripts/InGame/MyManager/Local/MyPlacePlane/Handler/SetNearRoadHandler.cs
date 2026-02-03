using InGame.MyEnum;
using InGame.MyManager.Global;
using InGame.MyObject;
using System.Collections.Generic;
using UnityEngine;

namespace InGame.MyManager.MyPlacePlane.Handler
{
    // 작성자: 조혜찬
    // 주위 도로 세팅 핸들러
    public class SetNearRoadHandler
    {
        public void Setting(List<RoadPlacePlaneObject> team1NearRoads, List<RoadPlacePlaneObject> team2NearRoads, List<RoadPlacePlaneObject> team3NearRoads)
        {
            switch (TeamManager.Instance.CurrentTeamType)
            {
                case TeamType.Team1:
                    SetNearRoad(team1NearRoads);
                    break;
                case TeamType.Team2:
                    SetNearRoad(team2NearRoads);
                    break;
                case TeamType.Team3:
                    SetNearRoad(team3NearRoads);
                    break;
            }
        }

        // 리스트에 있는 도로들을 전부 성과 근접한 도로로 만드는 함수(성과 근접한 도로로 만들 도로들을 저장하는 리스트)
        private void SetNearRoad(List<RoadPlacePlaneObject> list)
        {
            foreach (var road in list) // 리스트 순회
            {
                road.isNearToCastle = true; // 성과 근접한 리스트로 만들기
            }
        }
    }
}
// 마지막 작성 일자: 2026.02.03