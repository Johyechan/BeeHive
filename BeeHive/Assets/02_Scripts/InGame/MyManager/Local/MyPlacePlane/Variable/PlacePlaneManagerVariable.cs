using InGame.MyManager.MyPlacePlane.Handler;
using InGame.MySystem.Game;

namespace InGame.MyManager.MyPlacePlane.Variable
{
    // 작성자: 조혜찬
    // PlacePlaneManager 변수 모음 클래스
    public class PlacePlaneManagerVariable
    {
        public PlacePlaneMap placePlaneMap; // 전체 기물 판을 가지는 클래스 변수

        public HighLightHandler highLightHandler; // 하이라이트를 키고 끄는 기능을 가지는 클래스 변수

        public FindCanPlacePlaneSystem findCanPlacePlaneSystem; // 배치 가능한 배치 판들을 찾는 시스템 클래스 변수

        public PlacePlaneStateHandler placePlaneStateHandler; // 배치 칸의 상태를 관리하는 핸들러

        public SetNearRoadHandler setNearRoadHandler; // 주위 도로 세팅 핸들러
    }
}
// 마지막 작성 일자: 2025.09.19