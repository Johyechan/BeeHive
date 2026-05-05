using InGame.MyEnum;
using InGame.MyObject;

namespace InGame.MySystem.Game.FindSystem
{
    // 작성자: 조혜찬
    // 탐색할 때 필요한 기능들을 가지는 클래스
    public class FindPlanesUtil
    {
        public bool CheckNearRoad(TeamType teamType, RoadPlacePlaneObject road)
        {
            if (road.TeamType == teamType || road.TeamType == TeamType.None) // 현재 기물의 위치와 팀 타입이 같거나 비어있다면
            {
                return true;
            }
            else // 다른 팀일 경우
            {
                return false;
            }
        }
    }
}
// 마지막 작성 일자: 2026.05.05